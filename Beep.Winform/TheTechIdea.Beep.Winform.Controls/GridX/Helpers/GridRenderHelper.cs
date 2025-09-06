using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using TheTechIdea.Beep.Vis.Modules.Managers;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls.Models;

namespace TheTechIdea.Beep.Winform.Controls.GridX.Helpers
{
    internal class GridRenderHelper
    {
        private readonly BeepGridPro _grid;
        private readonly Dictionary<string, IBeepUIComponent> _columnDrawerCache = new();
        private readonly BeepLabel _lblPageInfo = new();
        public BeepLabel PageInfoLabel => _lblPageInfo;
        private readonly Dictionary<int, Rectangle> _headerFilterIconRects = new();
        public Dictionary<int, Rectangle> HeaderFilterIconRects => _headerFilterIconRects;
        private readonly Dictionary<int, Rectangle> _headerSortIconRects = new();
        public Dictionary<int, Rectangle> HeaderSortIconRects => _headerSortIconRects;

        public GridRenderHelper(BeepGridPro grid)
        {
            _grid = grid;
        }

        // Style flags
        public bool ShowGridLines { get; set; } = true;
        public bool ShowRowStripes { get; set; } = false;
        public DashStyle GridLineStyle { get; set; } = DashStyle.Solid;
        public bool UseElevation { get; set; } = false;
        public bool CardStyle { get; set; } = false;
        public bool UseHeaderGradient { get; set; } = false;
        public bool ShowSortIndicators { get; set; } = true;
        public bool UseHeaderHoverEffects { get; set; } = true;
        public bool UseBoldHeaderText { get; set; } = false;
        public int HeaderCellPadding { get; set; } = 2;

        internal IBeepTheme Theme => _grid.Theme != null ? BeepThemesManager.GetTheme(_grid.Theme) : BeepThemesManager.GetDefaultTheme();

        public void Draw(Graphics g)
        {
            if (g == null) return;
            DrawBackground(g);
            DrawHeaders(g);
            DrawRows(g);
            DrawGridLines(g); // ensure lines for visual snap
            if (_grid.__isColumnResizing)
            {
                using var pen = new Pen(Color.FromArgb(160, Theme.GridHeaderForeColor), 1f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                g.DrawLine(pen, _grid.__columnResizeVisualX, _grid.Layout.HeaderRect.Top, _grid.__columnResizeVisualX, _grid.Layout.RowsRect.Bottom);
            }
            DrawNavigator(g);
        }

        private void DrawBackground(Graphics g)
        {
            g.Clear(Theme?.GridBackColor ?? SystemColors.Window);
        }

        #region Headers
        private void DrawHeaders(Graphics g)
        {
            var headerRect = _grid.Layout.HeaderRect;
            if (headerRect.Width <= 0 || headerRect.Height <= 0) return;

            // Fill header background
            if (UseHeaderGradient)
            {
                using var lg = new LinearGradientBrush(headerRect,
                    ControlPaint.Light(Theme.GridHeaderBackColor, .05f),
                    ControlPaint.Dark(Theme.GridHeaderBackColor, .1f),
                    90f);
                g.FillRectangle(lg, headerRect);
            }
            else
            {
                using var b = new SolidBrush(Theme.GridHeaderBackColor);
                g.FillRectangle(b, headerRect);
            }
            using (var pen = new Pen(ControlPaint.Dark(Theme.GridHeaderBackColor)))
            {
                g.DrawLine(pen, headerRect.Left, headerRect.Bottom - 1, headerRect.Right, headerRect.Bottom - 1);
            }
            _headerFilterIconRects.Clear();
            _headerSortIconRects.Clear();
            var headerRects = new List<Rectangle>();
            int x = headerRect.Left;
            for (int i = 0; i < _grid.Data.Columns.Count; i++)
            {
                var col = _grid.Data.Columns[i];
                if (!col.Visible) continue;
                var cellRect = new Rectangle(x, headerRect.Top, col.Width, headerRect.Height);
                headerRects.Add(cellRect);
                DrawSingleHeader(g, cellRect, col, i);
                x += col.Width;
            }
            _grid.Layout.HeaderCellRects = headerRects.ToArray();
        }

        private void DrawSingleHeader(Graphics g, Rectangle rect, BeepColumnConfig col, int index)
        {
            bool hovered = index == _grid.Layout.HoveredHeaderColumnIndex && UseHeaderHoverEffects;

            // Resolve colors (column override first)
            Color baseBack = col?.HeaderStyle?.BackColor != Color.Empty ? col.HeaderStyle.BackColor : Theme.GridHeaderBackColor;
            Color baseFore = col?.HeaderStyle?.ForeColor != Color.Empty ? col.HeaderStyle.ForeColor : Theme.GridHeaderForeColor;

            if (UseHeaderGradient)
            {
                using var lg = new LinearGradientBrush(rect,
                    ControlPaint.Light(baseBack, .05f),
                    ControlPaint.Dark(baseBack, .1f),
                    90f);
                g.FillRectangle(lg, rect);
            }
            else
            {
                using var b = new SolidBrush(baseBack);
                g.FillRectangle(b, rect);
            }

            if (hovered)
            {
                using var hb = new SolidBrush(Color.FromArgb(40, Theme.GridPrimaryColor));
                g.FillRectangle(hb, rect);
            }

            using (var sepPen = new Pen(ControlPaint.Dark(baseBack)))
            {
                g.DrawLine(sepPen, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom - 1);
            }

            string text = col?.ColumnCaption ?? col?.ColumnName ?? string.Empty;

            Font fontToUse = null;
            bool dispose = false;
            if (col?.HeaderStyle != null && !string.IsNullOrWhiteSpace(col.HeaderStyle.Typography))
            {
                float size = col.HeaderStyle.FontSize > 0 ? col.HeaderStyle.FontSize : Theme.GridHeaderFont?.FontSize ?? _grid.Font.Size;
                size *= _grid.HeaderFontScale <= 0 ? 1f : _grid.HeaderFontScale;
                fontToUse = new Font(col.HeaderStyle.Typography, size, UseBoldHeaderText ? FontStyle.Bold : FontStyle.Regular);
                dispose = true;
            }
            else
            {
                fontToUse = ResolveHeaderFont(col);
            }

            Rectangle textRect = Rectangle.Inflate(rect, -HeaderCellPadding - 1, -2);
            int iconArea = 0;
            if (ShowSortIndicators) iconArea += 14;
            if (col != null && col.ShowFilterIcon) iconArea += 14;
            textRect.Width -= iconArea;
            TextRenderer.DrawText(g, text, fontToUse, textRect, baseFore, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            int iconX = textRect.Right + 2;
            int iconY = rect.Top + (rect.Height - 12) / 2;
            if (ShowSortIndicators && col != null && col.ShowSortIcon)
            {
                var sortRect = new Rectangle(iconX, iconY, 12, 12);
                DrawSortGlyph(g, sortRect, col.SortDirection == SortDirection.Descending, baseFore);
                _headerSortIconRects[index] = sortRect;
                iconX += 14;
            }
            if (col != null && col.ShowFilterIcon)
            {
                var filterRect = new Rectangle(iconX, iconY, 12, 12);
                DrawFilterGlyph(g, filterRect, baseFore);
                _headerFilterIconRects[index] = filterRect;
            }
            if (dispose && fontToUse != null) fontToUse.Dispose();
        }

        private void DrawSortGlyph(Graphics g, Rectangle r, bool desc, Color color)
        {
            Point p1, p2, p3;
            if (desc)
            {
                p1 = new Point(r.Left, r.Top);
                p2 = new Point(r.Right, r.Top);
                p3 = new Point(r.Left + r.Width / 2, r.Bottom);
            }
            else
            {
                p1 = new Point(r.Left + r.Width / 2, r.Top);
                p2 = new Point(r.Left, r.Bottom);
                p3 = new Point(r.Right, r.Bottom);
            }
            using var b = new SolidBrush(color);
            g.FillPolygon(b, new[] { p1, p2, p3 });
        }

        private void DrawFilterGlyph(Graphics g, Rectangle r, Color color)
        {
            Point p1 = new Point(r.Left, r.Top);
            Point p2 = new Point(r.Right, r.Top);
            Point p3 = new Point(r.Left + r.Width / 2, r.Bottom);
            using var pen = new Pen(color, 1);
            g.DrawPolygon(pen, new[] { p1, p2, p3 });
        }
        #endregion

        private void DrawRows(Graphics g)
        {
            var rowsRect = _grid.Layout.RowsRect;
            if (rowsRect.Width <= 0 || rowsRect.Height <= 0) return;
            int y = rowsRect.Top;
            int rowIndex = 0;
            foreach (var row in _grid.Data.Rows)
            {
                int h = row.Height > 0 ? row.Height : _grid.RowHeight;
                if (y > rowsRect.Bottom) break; // stop if outside
                var rowRect = new Rectangle(rowsRect.Left, y, rowsRect.Width, h);
                if (ShowRowStripes && (rowIndex % 2 == 1))
                {
                    using var stripe = new SolidBrush(ControlPaint.Light(Theme.GridBackColor, .95f));
                    g.FillRectangle(stripe, rowRect);
                }
                int x = rowsRect.Left;
                for (int c = 0; c < row.Cells.Count && c < _grid.Data.Columns.Count; c++)
                {
                    var col = _grid.Data.Columns[c];
                    if (!col.Visible) continue;
                    int w = col.Width;
                    var cellRect = new Rectangle(x, y, w, h);
                    var cell = row.Cells[c];
                    cell.Rect = cellRect; // store for hit-testing
                    Font cellFont = ResolveCellFont();
                    Color fore = Theme.GridForeColor;
                    string text = cell.CellValue?.ToString() ?? string.Empty;
                    TextRenderer.DrawText(g, text, cellFont, Rectangle.Inflate(cellRect, -4, -2), fore,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                    x += w;
                }
                y += h;
                rowIndex++;
            }
        }

        private Font _cachedCellFont; private float _cachedCellScale=-1f; private Font _cachedHeaderFont; private float _cachedHeaderScale=-1f; private string _cachedHeaderFamily; private FontStyle _cachedHeaderStyle;
        public void ResetFontCache(){ _cachedCellFont?.Dispose(); _cachedCellFont=null; _cachedHeaderFont?.Dispose(); _cachedHeaderFont=null; }
        private Font ResolveCellFont()
        {
            var themeFont = Theme?.GridCellFont; // TypographyStyle
            float scale = _grid.CellFontScale <= 0 ? 1f : _grid.CellFontScale;
            if (themeFont == null)
            {
                if (_cachedCellFont == null || _cachedCellScale != scale)
                {
                    _cachedCellFont?.Dispose();
                    _cachedCellFont = new Font(_grid.Font.FontFamily, _grid.Font.Size * scale, _grid.Font.Style);
                    _cachedCellScale = scale;
                }
                return _cachedCellFont;
            }
            float size = (themeFont.FontSize > 0 ? themeFont.FontSize : _grid.Font.Size) * scale;
            var style = themeFont.FontStyle != FontStyle.Regular ? themeFont.FontStyle : _grid.Font.Style;
            if (_cachedCellFont == null || Math.Abs(_cachedCellFont.Size - size) > 0.01f || _cachedCellFont.Style != style)
            {
                _cachedCellFont?.Dispose();
                _cachedCellFont = new Font(themeFont.FontFamily ?? _grid.Font.FontFamily.Name, size, style);
                _cachedCellScale = scale;
            }
            return _cachedCellFont;
        }
        private Font ResolveHeaderFont(BeepColumnConfig col)
        {
            // Per-column override already handled inline earlier; here handle theme + scaling cache.
            if (col?.HeaderStyle!=null && !string.IsNullOrWhiteSpace(col.HeaderStyle.Typography)) return null; // caller constructs custom font
            var themeFont = Theme?.GridHeaderFont; float scale = _grid.HeaderFontScale<=0?1f:_grid.HeaderFontScale; string fam = themeFont?.FontFamily ?? _grid.Font.FontFamily.Name; FontStyle style = themeFont?.FontStyle!=FontStyle.Regular? themeFont.FontStyle : (UseBoldHeaderText?FontStyle.Bold: _grid.Font.Style); float size =(themeFont?.FontSize>0?themeFont.FontSize:_grid.Font.Size)*scale;
            if (_cachedHeaderFont==null || Math.Abs(_cachedHeaderFont.Size-size)>0.01f || _cachedHeaderScale!=scale || _cachedHeaderFamily!=fam || _cachedHeaderStyle!=style){ _cachedHeaderFont?.Dispose(); _cachedHeaderFont=new Font(fam,size,style); _cachedHeaderScale=scale; _cachedHeaderFamily=fam; _cachedHeaderStyle=style; }
            return _cachedHeaderFont;
        }

        private void DrawGridLines(Graphics g)
        {
            if (!ShowGridLines) return;
            using var pen = new Pen(ControlPaint.Dark(Theme.GridBackColor), 1) { DashStyle = GridLineStyle };
            // vertical
            int x = _grid.Layout.RowsRect.Left;
            foreach (var col in _grid.Data.Columns.Where(c => c.Visible))
            {
                x += col.Width;
                g.DrawLine(pen, x - 1, _grid.Layout.RowsRect.Top, x - 1, _grid.Layout.RowsRect.Bottom);
            }
            // horizontal
            int y = _grid.Layout.RowsRect.Top;
            foreach (var row in _grid.Data.Rows)
            {
                int h = row.Height > 0 ? row.Height : _grid.RowHeight;
                y += h;
                g.DrawLine(pen, _grid.Layout.RowsRect.Left, y - 1, _grid.Layout.RowsRect.Right, y - 1);
            }
        }

        private void DrawNavigator(Graphics g)
        {
            // placeholder (existing implementation elsewhere)
        }
    }
}
