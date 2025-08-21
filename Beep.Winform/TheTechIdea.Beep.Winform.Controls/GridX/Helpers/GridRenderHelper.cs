            drawer.Theme = _grid.Theme;
            drawer.BackColor = backColor;
            drawer.ForeColor = foreColor;
            drawer.Bounds = rect;

            // Populate list-based controls BEFORE setting value, so they can resolve SelectedItem
            if (drawer is BeepComboBox combo)
            {
                var items = GetFilteredItems(column, cell);
                combo.ListItems = new BindingList<SimpleItem>(items);
            }
            else if (drawer is BeepListBox listBox)
            {
                var items = GetFilteredItems(column, cell);
                listBox.ListItems = new BindingList<SimpleItem>(items);
            }
            else if (drawer is BeepListofValuesBox lov)
            {
                var items = GetFilteredItems(column, cell);
                lov.ListItems = new List<SimpleItem>(items);
            }

            // Special handling for BeepComboBox: clear selection when value is null/empty
            if (drawer is BeepComboBox comboDrawer)
            {
                var val = cell?.CellValue;
                bool isEmpty = val == null || (val is string sv && string.IsNullOrWhiteSpace(sv));
                if (isEmpty)
                {
                    try { ((IBeepUIComponent)comboDrawer).ClearValue(); } catch { }
                    try { comboDrawer.SelectedIndex = -1; } catch { }
                    try { comboDrawer.Text = string.Empty; } catch { }
                }
                else
                {
                    try { ((IBeepUIComponent)comboDrawer).SetValue(val); } catch { }
                }
            }
            else if (drawer is IBeepUIComponent ic)
            {
                try { ic.SetValue(cell.CellValue); } catch { }
            }
            else if (drawer is BeepButton btn)
            {
                btn.Text = cell.CellValue?.ToString() ?? string.Empty;
            }

            // Draw via component to match BeepSimpleGrid look
            drawer.Draw(g, rect);