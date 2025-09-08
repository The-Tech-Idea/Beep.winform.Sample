// ...existing code...
        private void HandleSearchClick(Rectangle searchRect)
        {
            if (!_searchBoxAddedToControls)
            {
                // Add the actual search box control at the right position
               // _searchBox.Location = searchRect.Location;
               
                
                // CRITICAL: Force size and prevent Material Design from overriding it
                _searchBox.EnableMaterialStyle = false;

                // Ensure the control is positioned correctly relative to this AppBar
                // Use SetBounds so Left/Top/Width/Height are set in one call
                _searchBox.SetBounds(searchRect.Left, searchRect.Top, searchRect.Width, searchRect.Height);
                // Clear right-anchor to avoid designer/runtime anchoring interfering with manual positioning
                _searchBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                _searchBox.Visible = true;
                Controls.Add(_searchBox);
                _searchBoxAddedToControls = true;

                // Focus the search box and select all text if any
                _searchBox.Focus();
                _searchBox.SelectAll();

                // Register for lost focus to switch back to drawing mode
                _searchBox.LostFocus += SearchBox_LostFocus;
            }
            else
            {
                // If already added, reposition it in case layout changed (important in edit/design mode), then focus
                _searchBox.SetBounds(searchRect.Left, searchRect.Top, searchRect.Width, searchRect.Height);
                _searchBox.Focus();
                _searchBox.SelectAll();
            }

            var arg = new BeepAppBarEventsArgs("Search");
            arg.Selectedstring = _searchBox.Text;
            OnSearchBoxSelected?.Invoke(this, arg);
        }
// ...existing code...