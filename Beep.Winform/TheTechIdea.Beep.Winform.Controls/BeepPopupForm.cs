        private void InitializePopupForm()
        {
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            InPopMode = true;

            // Initialize DPI scaling first
            UpdateDpiScaling();
            Padding = new Padding(4);
            
            // Set BorderRadius safely - only if the form has valid dimensions
            // or defer until the form is properly initialized
            if (IsHandleCreated && ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                BorderRadius = 3;
            }
            else
            {
                // Set the field directly without triggering UpdateFormRegion
                _borderRadius = 3;
            }
            
            BorderThickness = 2;
            _closeTimer = new System.Windows.Forms.Timer { Interval = _closeTimeout };
            _closeTimer.Tick += CloseTimer_Tick;

            this.MouseEnter += BeepPopupForm_MouseEnter;
            this.MouseLeave += BeepPopupForm_MouseLeave;

            // Handle form closing to clean up
            FormClosed += (s, e) =>
            {
                if (TriggerControl != null)
                {
                    TriggerControl.MouseEnter -= TriggerControl_MouseEnter;
                    TriggerControl.MouseLeave -= TriggerControl_MouseLeave;
                }
                StopTimers();
            };
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            
            // Now that the form is shown and has valid dimensions, update the region if needed
            if (_borderRadius > 0 && ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                UpdateFormRegion();
            }
        }