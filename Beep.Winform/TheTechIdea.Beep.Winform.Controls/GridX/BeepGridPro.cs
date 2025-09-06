using System.ComponentModel;
using System.ComponentModel.Design;
using TheTechIdea.Beep.Winform.Controls.Base;
using TheTechIdea.Beep.Winform.Controls.GridX.Helpers;
using TheTechIdea.Beep.Winform.Controls.Models;
using TheTechIdea.Beep.Winform.Controls.Converters;
using TheTechIdea.Beep.Vis.Modules.Managers;
using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Winform.Controls.GridX
{
    public partial class BeepGridPro : BaseControl
    {
        // ...existing fields...
        #region AutoSize Behavior Overrides
        // New: mimic WinForms DataGridView default (no automatic content sizing unless explicitly requested)
        [Browsable(true)]
        [Category("Layout")]
        [Description("Automatically size columns to their content when data is bound. Default false (DataGridView-like). ")]
        [DefaultValue(false)]
        public bool AutoSizeColumnsOnDataBind { get; set; } = false;

        [Browsable(true)]
        [Category("Layout")]
        [Description("Default width applied to columns when not auto-sizing.")]
        [DefaultValue(100)]
        public int DefaultColumnWidth { get; set; } = 100;

        private void ApplyDefaultColumnWidths()
        {
            if (Data?.Columns == null) return;
            foreach (var col in Data.Columns)
            {
                if col == null) continue;
                // Skip system columns (checkbox, row number, id) – keep their specialized width logic
                if (col.IsSelectionCheckBox || col.IsRowNumColumn || col.IsRowID) continue;
                if (AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.None && !AutoSizeColumnsOnDataBind)
                {
                    // Only override if width not explicitly set (0 or negative)
                    if (col.Width <= 0)
                    {
                        col.Width = DefaultColumnWidth;
                    }
                }
            }
            Layout?.Recalculate();
        }
        #endregion

        #region Column Expansion (new)
        [Browsable(true)]
        [Category("Layout")] 
        [DefaultValue(false)]
        [Description("Automatically expand visible (non-system) columns to fill available width. Call ExpandColumns() or set AutoExpandColumns = true.")]
        public bool AutoExpandColumns { get; set; } = false;

        /// <summary>
        /// Expands visible columns to fill the available DrawingRect (or control client width) by distributing remaining space.
        /// System columns (Sel, RowNum, RowID) kept fixed unless includeSystemColumns = true.
        /// </summary>
        public void ExpandColumns(bool includeSystemColumns = false, bool proportional = true)
        {
            if (Data?.Columns == null || Data.Columns.Count == 0) return;
            // Ensure layout rect is current
            var availRect = DrawingRect;
            int availableWidth = (availRect.Width > 0 ? availRect.Width : ClientSize.Width);
            if (availableWidth <= 0) return;

            // Collect columns to expand
            var expandable = Data.Columns
                .Where(c => c.Visible && (includeSystemColumns || (!c.IsSelectionCheckBox && !c.IsRowNumColumn && !c.IsRowID)))
                .ToList();
            if (expandable.Count == 0) return;

            // Width used by non-expandable visible columns
            int nonExpandableWidth = Data.Columns
                .Where(c => c.Visible && !expandable.Contains(c))
                .Sum(c => Math.Max(10, c.Width));

            // Current total width of expandable columns
            int currentExpandableWidth = expandable.Sum(c => Math.Max(10, c.Width));

            int spaceForExpandable = availableWidth - nonExpandableWidth;
            if (spaceForExpandable <= 0) return; // nothing to distribute

            int extra = spaceForExpandable - currentExpandableWidth;
            if (extra <= 0) return; // already wider than available; do not shrink here

            double totalBase = proportional ? currentExpandableWidth : expandable.Count;
            if (totalBase <= 0) return;

            int distributed = 0;
            for (int i = 0; i < expandable.Count; i++)
            {
                var col = expandable[i];
                int add = proportional
                    ? (int)Math.Floor(extra * (Math.Max(10, col.Width) / totalBase))
                    : extra / expandable.Count;
                // ensure last column absorbs rounding remainder
                if (i == expandable.Count - 1) add = extra - distributed;
                col.Width = Math.Max(10, col.Width + add);
                distributed += add;
            }

            // Apply new widths to cell configs
            foreach (var row in Data.Rows)
            {
                for (int i = 0; i < row.Cells.Count && i < Data.Columns.Count; i++)
                {
                    row.Cells[i].Width = Data.Columns[i].Width;
                }
            }

            Layout?.Recalculate();
            Invalidate();
        }
        #endregion

        // ...existing constructor...
        public BeepGridPro():base   ()
        {
            // ...existing constructor body...
        }

        // Modify DataSource property setter to apply default widths when binding
        public object DataSource 
        {
            get => Data.DataSource;
            set { 
                if (!ReferenceEquals(Data.DataSource, value))
                {
                    Data.Bind(value); // Bind to original data source
                    Navigator.BindTo(value); 
                    Data.InitializeData(); // Sync data after binding
                    // Apply default column widths if auto sizing is disabled
                    if (!AutoSizeColumnsOnDataBind && AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.None)
                    {
                        ApplyDefaultColumnWidths();
                    }
                    else
                    {
                        if (AutoSizeColumnsOnDataBind)
                        {
                            // Defer to sizing helper (if user wants auto sizing)
                            Sizing?.AutoResizeColumnsToFitContent();
                        }
                    }
                    Layout.Recalculate(); 
                    if (!DesignMode) Invalidate();
                }
            }
        }

        // Expose a public method for users to force default widths after changing settings
        public void ResetToDefaultColumnWidths()
        {
            ApplyDefaultColumnWidths();
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateDrawingRect();
            SafeRecalculate();

            if (AutoExpandColumns) ExpandColumns();

            // Do not stretch the editor host; it will be sized to the active cell only
            
            // Only update scrollbars if not in design mode
            if (!DesignMode)
            {
                ScrollBars?.UpdateBars();
                Invalidate();
            }
        }
        private float _headerFontScale = 1.0f;
        [Browsable(true)]
        [Category("Appearance")] 
        [DefaultValue(1.0f)]
        [Description("Scale factor applied to the theme header font (1.0 = original size). Changing it invalidates header rendering.")]
        public float HeaderFontScale
        {
            get => _headerFontScale;
            set
            {
                if (value <= 0) value = 1.0f;
                if (Math.Abs(_headerFontScale - value) > 0.001f)
                {
                    _headerFontScale = value;
                    Render?.ResetFontCache();
                    Invalidate();
                }
            }
        }

        private float _cellFontScale = 1.0f;
        [Browsable(true)]
        [Category("Appearance")] 
        [DefaultValue(1.0f)]
        [Description("Scale factor applied to the theme cell font (1.0 = original size).")]
        public float CellFontScale
        {
            get => _cellFontScale;
            set
            {
                if (value <= 0) value = 1.0f;
                if (Math.Abs(_cellFontScale - value) > 0.001f)
                {
                    _cellFontScale = value;
                    Render?.ResetFontCache();
                    Invalidate();
                }
            }
        }

        // User resize toggles (needed by GridInputHelper)
        [Browsable(true)]
        [Category("Behavior")] 
        [DefaultValue(true)]
        [Description("Allow runtime manual column resize by dragging header borders.")]
        public bool EnableColumnResize { get; set; } = true;

        [Browsable(true)]
        [Category("Behavior")] 
        [DefaultValue(false)]
        [Description("Allow runtime manual row height resize by dragging row borders (applies to all rows globally).")]
        public bool EnableRowResize { get; set; } = false;

        [Browsable(true)]
        [Category("Behavior")] 
        [DefaultValue(5)]
        [Description("Pixel margin around a header vertical border that starts a column resize operation.")]
        public int ColumnResizeMargin { get; set; } = 5;

        [Browsable(true)]
        [Category("Behavior")] 
        [DefaultValue(4)]
        [Description("Pixel margin around a row bottom border that starts a row height resize operation.")]
        public int RowResizeMargin { get; set; } = 4;

        internal bool __isColumnResizing; // live drag flag
        internal int __columnResizeVisualX; // screen X of drag guideline

        #region Resize Events API
        [Browsable(true), Category("Behavior"), Description("Raised continuously while a column is being resized (can cancel or adjust).")]
        public event EventHandler<GridColumnResizingEventArgs> ColumnResizing;
        [Browsable(true), Category("Behavior"), Description("Raised after a column resize operation ends.")]
        public event EventHandler<GridColumnResizedEventArgs> ColumnResized;
        [Browsable(true), Category("Behavior"), Description("Raised continuously while a row is being resized (can cancel or adjust).")]
        public event EventHandler<GridRowResizingEventArgs> RowResizing;
        [Browsable(true), Category("Behavior"), Description("Raised after a row resize operation ends.")]
        public event EventHandler<GridRowResizedEventArgs> RowResized;

        [Browsable(true), Category("Behavior"), DefaultValue(true), Description("Double-click a header border to auto-fit column width.")]
        public bool EnableAutoFitOnDoubleClick { get; set; } = true;

        [Browsable(true), Category("Behavior"), DefaultValue(false), Description("If true, resizing one row applies the new height to all rows uniformly.")]
        public bool UniformRowHeights { get; set; } = false;

        internal bool RaiseColumnResizing(int colIndex, int original, ref int proposed)
        {
            var args = new GridColumnResizingEventArgs(colIndex, original, proposed);
            try { ColumnResizing?.Invoke(this, args); } catch { }
            if (args.Cancel) return false;
            if (args.ProposedWidth != proposed) proposed = args.ProposedWidth;
            return true;
        }
        internal void RaiseColumnResized(int colIndex, int original, int finalW)
        {
            try { ColumnResized?.Invoke(this, new GridColumnResizedEventArgs(colIndex, original, finalW)); } catch { }
        }
        internal bool RaiseRowResizing(int rowIndex, int original, ref int proposed)
        {
            var args = new GridRowResizingEventArgs(rowIndex, original, proposed);
            try { RowResizing?.Invoke(this, args); } catch { }
            if (args.Cancel) return false;
            if (args.ProposedHeight != proposed) proposed = args.ProposedHeight;
            return true;
        }
        internal void RaiseRowResized(int rowIndex, int original, int finalH)
        {
            try { RowResized?.Invoke(this, new GridRowResizedEventArgs(rowIndex, original, finalH)); } catch { }
        }

        internal void AutoFitColumn(int columnIndex, bool includeHeader = true, bool allRows = true)
        {
            if (columnIndex < 0 || columnIndex >= Data.Columns.Count) return;
            var col = Data.Columns[columnIndex];
            int optimal = Sizing.GetColumnWidth(col, includeHeader, allRows);
            // Enforce Min/Max if provided
            int min = col.MinWidth > 0 ? col.MinWidth : 20;
            int max = col.MaxWidth > 0 ? col.MaxWidth : optimal; // if MaxWidth==0 treat as no upper bound
            optimal = Math.Max(min, optimal);
            if (col.MaxWidth > 0) optimal = Math.Min(max, optimal);
            int original = col.Width;
            if (original == optimal) return;
            col.Width = optimal;
            Layout.Recalculate();
            RaiseColumnResized(columnIndex, original, optimal);
            Invalidate();
        }
        #endregion
    }

    #region EventArgs Types
    public sealed class GridColumnResizingEventArgs : EventArgs
    {
        public int ColumnIndex { get; }
        public int OriginalWidth { get; }
        public int ProposedWidth { get; set; }
        public bool Cancel { get; set; }
        public GridColumnResizingEventArgs(int index, int original, int proposed)
        { ColumnIndex = index; OriginalWidth = original; ProposedWidth = proposed; }
    }
    public sealed class GridColumnResizedEventArgs : EventArgs
    {
        public int ColumnIndex { get; }
        public int OriginalWidth { get; }
        public int FinalWidth { get; }
        public GridColumnResizedEventArgs(int index, int original, int finalW)
        { ColumnIndex = index; OriginalWidth = original; FinalWidth = finalW; }
    }
    public sealed class GridRowResizingEventArgs : EventArgs
    {
        public int RowIndex { get; }
        public int OriginalHeight { get; }
        public int ProposedHeight { get; set; }
        public bool Cancel { get; set; }
        public GridRowResizingEventArgs(int index, int original, int proposed)
        { RowIndex = index; OriginalHeight = original; ProposedHeight = proposed; }
    }
    public sealed class GridRowResizedEventArgs : EventArgs
    {
        public int RowIndex { get; }
        public int OriginalHeight { get; }
        public int FinalHeight { get; }
        public GridRowResizedEventArgs(int index, int original, int finalH)
        { RowIndex = index; OriginalHeight = original; FinalHeight = finalH; }
    }
    #endregion
}
