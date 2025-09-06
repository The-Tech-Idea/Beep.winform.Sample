# BeepGridPro – Resizing & Interaction Enhancement Plan

Goal: Achieve feature and UX parity (or better) with BeepSimpleGrid for column and row resizing while keeping the helper?based architecture clean, testable, and extensible.

## 1. Current State Summary
- Column resizing logic exists in two places:
  1. `GridInputHelper` – simple delta apply per `MouseMove` (no guideline, no min propagation, no row height logic).
  2. `BeepGridPro` (#region Resizing Logic) – richer: guideline, start/stop encapsulation, row resizing support.
- Row resizing only in main class; not exposed through helper.
- No DPI normalization for margins yet.
- No double?click auto size on header border (common UX expectation).
- No persisted preferred width cache (future: per column state, user overrides vs auto sizing).
- Render helper draws guideline if `__isColumnResizing` is true.

## 2. Problems / Risks
| Issue | Impact |
|-------|--------|
| Duplication of logic | Harder maintenance, inconsistent behavior. |
| Mixed responsibilities | Main control class growing bloated. |
| Missing UX (double?click to AutoFit) | Lower usability vs standard grids. |
| No min/max width policy | Risk of zero width / visual collapse. |
| Row resize not synchronized with `RowHeight` global policy | Inconsistent row height semantics. |

## 3. Design Principles
1. Single ownership: All interactive resize logic consolidated into `GridInputHelper`.
2. Passive main class: `BeepGridPro` exposes only public toggles & state flags.
3. Render-only guideline: `GridRenderHelper` keeps drawing the visual line, but gets transient X from input helper.
4. Extensible hooks: Provide events:
   - `ColumnResizing(int columnIndex, int proposedWidth, ref int finalWidth)` (Cancelable via ref)
   - `ColumnResized(int columnIndex, int finalWidth)`
   - `RowResizing(int rowIndex, int proposedHeight, ref int finalHeight)`
   - `RowResized(int rowIndex, int finalHeight)`
5. AutoFit on double click implemented via sizing helper measuring content (`GridSizingHelper.GetColumnWidth`).

## 4. Task Breakdown
### 4.1 Consolidation
- [ ] Move: `HitTestColumnBorder`, `HitTestRowBorder`, `Begin/Update/EndColumnResize`, `Begin/Update/EndRowResize` from `BeepGridPro` ? `GridInputHelper`.
- [ ] Replace internal flags with: `Input.IsColumnResizing`, `Input.ColumnResizeVisualX`.
- [ ] Keep backwards compat fields (`__isColumnResizing`, `__columnResizeVisualX`) updated for render helper until fully refactored.

### 4.2 Events API
- [ ] Add events to `BeepGridPro` (public):
  ```csharp
  public event EventHandler<GridColumnResizingEventArgs> ColumnResizing;
  public event EventHandler<GridColumnResizedEventArgs> ColumnResized;
  public event EventHandler<GridRowResizingEventArgs> RowResizing;
  public event EventHandler<GridRowResizedEventArgs> RowResized;
  ```
- [ ] Define EventArgs types (column index, proposed size, original size, allow modify/ref override).
- [ ] Fire during update + end operations.

### 4.3 AutoFit (Double Click)
- [ ] Detect double click on a column border ? call measurement path: `Sizing.GetColumnWidth(col, includeHeader:true, allRows: AutoSizeColumnsMode implies?)`.
- [ ] Apply width with min/max constraint.
- [ ] Raise resized events.

### 4.4 Min/Max Policy
- [ ] Add per column: `MinWidth` (default 20), `MaxWidth` (default 1000 or 0 = unlimited).
- [ ] Enforce during update & autofit.

### 4.5 Row Resizing Semantics
- Decision: Row resizing sets *that row's* `Height` (per-row variable heights supported already) OR toggles a global `RowHeight`? Keep per-row.
- [ ] Add optional `UniformRowHeights` flag – if true, resizing one row updates all.

### 4.6 DPI Awareness
- [ ] Normalize effective margins: `effectiveMargin = DpiScaleFactor * BaseMargin`.
- [ ] Use scaled margin in hit testing.

### 4.7 Performance Optimizations
- [ ] During live drag: avoid full `Layout.Recalculate()` each pixel. Strategy:
  - Update target column width inline.
  - Delay full recalculation until `MouseUp` or threshold (e.g., every 8 px). Minimal header rectangle invalidation otherwise.
- [ ] Batch invalidations ? `Invalidate(new Rectangle(changedColumnArea))`.

### 4.8 Testing / Validation Checklist
- [ ] Resize first, middle, last column.
- [ ] Resize hidden-adjacent columns (skip hidden while hit testing).
- [ ] Row resize with stripes enabled.
- [ ] Guideline draws above rows.
- [ ] AutoFit after manual shrink.
- [ ] DPI scale 125%, 150% hit accuracy.

## 5. API Additions (Draft)
```csharp
public bool EnableAutoFitOnDoubleClick { get; set; } = true;
public bool UniformRowHeights { get; set; } = false;
```
EventArgs sample:
```csharp
public sealed class GridColumnResizingEventArgs : EventArgs {
    public int ColumnIndex { get; }
    public int OriginalWidth { get; }
    public int ProposedWidth { get; set; }
    public bool Cancel { get; set; }
    public GridColumnResizingEventArgs(int index,int orig,int proposed){ColumnIndex=index;OriginalWidth=orig;ProposedWidth=proposed;}
}
```

## 6. Migration Steps
1. Implement new EventArgs and events.
2. Port logic into `GridInputHelper`; keep adapter fields for render.
3. Remove duplicate logic from main class.
4. Add double?click AutoFit.
5. Add min/max width + optional per column properties (extend `BeepColumnConfig`).
6. QA & adjust doc (`BeepGridPro.HowTo.md`).

## 7. Future Considerations
- Column drag reorder (reuse border hit detection zone with offset threshold).
- Resize preview overlay instead of dashed line (semi?transparent rect).
- Snap guidelines (align to 8px grid optionally).
- Persist user sizing (serialize to tag / external settings provider).

## 8. Open Questions
| Topic | Question |
|-------|----------|
| Virtualization | Needed before large datasets? ( >50k rows ) |
| Multi-monitor DPI | Should per-monitor DPI re-trigger margin scaling? |
| Accessibility | Keyboard resize (Ctrl+Arrow)? |

---
Prepared for next iteration. After acceptance we proceed with implementation tasks 4.1 ? 4.6.
