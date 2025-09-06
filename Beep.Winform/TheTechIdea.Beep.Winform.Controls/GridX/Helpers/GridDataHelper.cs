        public void Bind(object dataSource)
        {
            DataSource = dataSource;
            DataMember = _grid.DataMember; // Update DataMember from grid
            
            // Always ensure system columns are present FIRST
            EnsureSystemColumns();
            
            // Only auto-generate columns if none exist beyond system columns, so user-configured columns are preserved
            if (Columns.Count <= GetSystemColumnCount())
            {
                AutoGenerateColumns();
                if (_grid.AutoExpandColumns) _grid.ExpandColumns();
                return; // AutoGenerateColumns calls RefreshRows and auto-sizing
            }

            // Refresh rows for existing columns (but limit in design mode)
            RefreshRows();

            // Update page info for paging controls
            UpdatePageInfo();

            // Skip auto-sizing in design mode to prevent excessive operations
            if (!System.ComponentModel.LicenseManager.UsageMode.Equals(System.ComponentModel.LicenseUsageMode.Designtime) && 
                _grid.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.None)
            {
                _grid.AutoResizeColumnsToFitContent();
            }
            if (_grid.AutoExpandColumns) _grid.ExpandColumns();
        }
...
            // Apply auto-sizing if enabled
            if (_grid.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.None)
            {
                _grid.AutoResizeColumnsToFitContent();
            }
            if (_grid.AutoExpandColumns) _grid.ExpandColumns();
        }
...
            if (!System.ComponentModel.LicenseManager.UsageMode.Equals(System.ComponentModel.LicenseUsageMode.Designtime) && 
                _grid.AutoSizeColumnsMode != DataGridViewAutoSizeColumnsMode.None)
            {
                _grid.AutoResizeColumnsToFitContent();
            }
            if (_grid.AutoExpandColumns) _grid.ExpandColumns();

            // Update page info after refreshing rows
            UpdatePageInfo();
        }