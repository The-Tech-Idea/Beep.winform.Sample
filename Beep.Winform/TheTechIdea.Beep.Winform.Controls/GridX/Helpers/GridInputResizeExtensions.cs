using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using TheTechIdea.Beep.Winform.Controls.Models;

namespace TheTechIdea.Beep.Winform.Controls.GridX.Helpers
{
    /// <summary>
    /// Consolidated resize handling moved out of BeepGridPro main file (Phase 1 of plan)
    /// This partial-like helper is attached to GridInputHelper through extension style static methods.
    /// </summary>
    internal static class GridInputResizeExtensions
    {
        internal static bool HandleHeaderBorderDoubleClick(BeepGridPro grid, MouseEventArgs e)
        {
            if (!grid.EnableAutoFitOnDoubleClick || e.Button != MouseButtons.Left || e.Clicks < 2) return false;
            if (!grid.Layout.ShowColumnHeaders || !grid.Layout.HeaderRect.Contains(e.Location)) return false;
            int hit = HitTestColumnBorder(grid, e.Location);
            if (hit < 0) return false;
            grid.AutoFitColumn(hit);
            return true;
        }

        internal static int HitTestColumnBorder(BeepGridPro grid, Point p)
        {
            if (!grid.AllowUserToResizeColumns || !grid.Layout.ShowColumnHeaders) return -1;
            var rects = grid.Layout.HeaderCellRects; if (rects == null) return -1;
            for (int i = 0; i < rects.Length; i++)
            {
                var r = rects[i]; if (r.IsEmpty) continue;
                if (p.X >= r.Right - grid.ColumnResizeMargin && p.X <= r.Right + grid.ColumnResizeMargin && r.ContainsY(p.Y)) return i;
            }
            return -1;
        }
        internal static int HitTestRowBorder(BeepGridPro grid, Point p)
        {
            if (!grid.AllowUserToResizeRows) return -1;
            var rowsRect = grid.Layout.RowsRect; if (!rowsRect.Contains(p)) return -1;
            int y = rowsRect.Top;
            for (int i = 0; i < grid.Data.Rows.Count; i++)
            {
                int h = grid.Data.Rows[i].Height > 0 ? grid.Data.Rows[i].Height : grid.RowHeight;
                var rowRect = new Rectangle(rowsRect.Left, y, rowsRect.Width, h);
                if (p.Y >= rowRect.Bottom - grid.RowResizeMargin && p.Y <= rowRect.Bottom + grid.RowResizeMargin) return i;
                y += h; if (y > rowsRect.Bottom) break;
            }
            return -1;
        }
        private static bool ContainsY(this Rectangle r, int y) => y >= r.Top && y <= r.Bottom;
    }
}
