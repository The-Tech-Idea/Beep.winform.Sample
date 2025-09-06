# BeepGridPro How-To Guide

This guide explains how BeepGridPro is structured and how to use/extend it. The control is intentionally decomposed into helper classes ("mini subsystems") to keep drawing, data, input, selection, scrolling, sizing, filtering, dialogs, theming and navigation concerns isolated.

## 1. Core Partial / Main Class
`BeepGridPro` (partial) owns: state, public API, properties (layout / behavior), event surface, style application, and high?level orchestration. It delegates to helpers created in the constructor (except in design mode). It also now contains a light inline column/row resizing implementation (#region Resizing Logic) – planned to be migrated into `GridInputHelper`.

### Key Public Features
- Data binding (`DataSource`, `DataMember`, `Uow`)
- Column / row configuration (`Columns`, `RowHeight`, `ColumnHeaderHeight`)
- Styling (`GridStyle`, theme integration)
- Selection, navigation, filtering, sorting
- Auto / manual sizing (column & row)
- Excel?like filter icons (planned extension)

## 2. Helper Classes Overview
| Helper | Responsibility |
|--------|----------------|
| `GridDataHelper` | Data binding, wrapping, row/column collections, system columns. |
| `GridLayoutHelper` | Calculates rectangles (header, rows, navigator, scroll areas), header cell hit rectangles. |
| `GridRenderHelper` | All drawing (background, headers, cells, grid lines, resize guideline, navigator placeholder). |
| `GridSelectionHelper` | Active cell & multi?row selection logic. |
| `GridInputHelper` | Mouse & keyboard interaction (selection, column resize – basic). |
| `GridScrollHelper` | Logical pixel & row index scroll state. |
| `GridScrollBarsHelper` | Owner?drawn scrollbar rendering & interaction. |
| `GridSortFilterHelper` | Column sorting & (future) filtering integration. |
| `GridEditHelper` | In?cell editing initiation / commit hooks (dialogs or inline planned). |
| `GridSizingHelper` | Auto size algorithms for columns & rows. |
| `GridThemeHelper` | Applies theme colors / fonts. |
| `GridNavigatorHelper` | Record navigation (first/prev/next/last, insert/delete/save hooks). |
| `GridDialogHelper` | External popup dialogs (search, column config, filter editor, cell editor). |
| `GridUnitOfWorkBinder` | (Optional) Unit of Work binding adapter. |
| `ExcelFilterHelper` / `BeepGridProFilterExtensions` | Extension entry points for future Excel?like filter UI. |

## 3. Typical Usage
```csharp
var grid = new BeepGridPro { Dock = DockStyle.Fill };
grid.DataSource = myList;           // IList / BindingSource / IEnumerable
grid.AutoGenerateColumns();         // Or define columns manually before binding
grid.GridStyle = BeepGridStyle.Material;
grid.EnableExcelFilter();           // Show filter icons
grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
// Optional manual width
grid.SetColumnWidth("Name", 180);
// Selection change
grid.RowSelectionChanged += (s,e)=> { /* handle */ };
```

## 4. Column / Row Resizing (Current vs Target)
Current state:
- Basic live column resizing implemented *both* in `GridInputHelper` (simple) and newly in `BeepGridPro` (#region Resizing Logic) for richer guideline + row height support.
- Row resizing not yet centralized (only new logic in `BeepGridPro`).
Target state:
- Remove duplication and move unified logic into `GridInputHelper` (feature parity with BeepSimpleGrid: margins, sticky constraints, min widths, invalidate optimization, DPI awareness).

## 5. Extending / Customizing
| Need | Extension Point |
|------|-----------------|
| Custom painting | Subclass and override `DrawContent` after calling `base`; or wrap via a new render helper. |
| Per-column custom cell painting | Enhance `GridRenderHelper.DrawRows` (add cell type dispatch / control cache). |
| Inline editors | Extend `GridEditHelper` and add editor host panel (similar to BeepSimpleGrid `_editingControl`). |
| Virtualization | Replace `Data.Rows` population strategy (paged window) + adapt `GridScrollHelper`. |
| Excel filter popup | Implement popup in `DialogHelper` + wire header icon hit test in `GridInputHelper`. |

## 6. Planned Refactors (See plan.md)
- Consolidate resize logic into `GridInputHelper`.
- Introduce cell virtualization window fill.
- Add sticky columns & horizontal viewport diff drawing (like BeepSimpleGrid). 
- Unify selection & checkbox column sync path.

## 7. Important Internal Contracts
- **Layout** must be recalculated ( `Layout.Recalculate()` ) after structural changes (adding/removing columns, resize, style change).
- **Invalidate()** only after minimal state changes (avoid chained recalculations inside loops – batch when possible).
- **Theme** changes must call `ThemeHelper.ApplyTheme()` + `ApplyGridStyle()`.

## 8. Migration Notes
If moving from `BeepSimpleGrid`:
- Many naming parallels: `Columns`, `Rows`, `RowHeight`, selection & sorting events.
- Replace direct painting overrides with helper injections where possible.

## 9. Safety & Performance
- Avoid performing measurement per paint where possible (cache fonts, column width decisions).
- Future optimization: row window virtualization & dirty region invalidation.

## 10. Quick Troubleshooting
| Symptom | Likely Cause | Action |
|---------|--------------|--------|
| Columns overlap | Layout not recalculated | Call `Layout.Recalculate()` after width changes. |
| Scrollbars wrong | Data count changed | `ScrollBars.UpdateBars()` after `Data.RefreshRows()`. |
| Header flicker | Excess invalidations | Batch updates (`SuspendLayout` + `ResumeLayout`). |
| Resize guideline missing | `__isColumnResizing` not set | Ensure BeginColumnResize executed. |

---
**See `plan.md` for roadmap & technical tasks.**
