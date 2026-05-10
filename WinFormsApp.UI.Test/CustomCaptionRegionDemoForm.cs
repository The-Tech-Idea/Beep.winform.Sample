using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls.Forms;
using TheTechIdea.Beep.Winform.Controls.Forms.ModernForm;
using FormStyleValue = TheTechIdea.Beep.Vis.Modules.FormStyle;

namespace WinFormsApp.UI.Test
{
    internal sealed class CustomCaptionRegionDemoForm : BeepiFormPro
    {
        private readonly FormRegion _helpRegion;
        private readonly TableLayoutPanel _contentLayout;
        private Label _statusLabel;
        private ComboBox _styleComboBox;
        private CheckBox _screenReaderCheckBox;
        private CheckBox _highContrastCheckBox;
        private CheckBox _searchBoxCheckBox;
        private CheckBox _profileButtonCheckBox;

        public CustomCaptionRegionDemoForm()
        {
            Text = "Custom Caption Region Demo";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(960, 640);
            Size = new Size(1180, 720);

            FormStyle = FormStyleValue.Modern;
            ShowCaptionBar = true;
            ShowSearchBox = true;
            ShowProfileButton = true;
            ShowThemeButton = false;
            ShowStyleButton = false;
            ShowCustomActionButton = false;
            ScreenReaderSupport = true;
            FocusIndicatorStyle = FocusIndicatorStyle.Prominent;

            _contentLayout = BuildContentLayout();
            Controls.Add(_contentLayout);

            _helpRegion = new FormRegion
            {
                Id = "help",
                Dock = RegionDock.Caption,
                IsInteractive = true,
                AccessibleName = "Caption help region",
                AccessibleDescription = "Demonstrates an interactive custom caption region. Press Enter or Space to activate it when focused.",
                AccessibleDefaultActionDescription = "Open demo help",
                AccessibleRole = AccessibleRole.PushButton,
                OnPaint = PaintHelpRegion
            };

            AddRegion(_helpRegion);
            RegionClick += HandleRegionClick;

            UpdateCustomRegionBounds();
            UpdateStatus("Use F6 or Shift+F6 to move focus into the caption, then Enter or Space to activate the custom '?' region.");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            UpdateContentPadding();
            UpdateCustomRegionBounds();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateContentPadding();
            UpdateCustomRegionBounds();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RegionClick -= HandleRegionClick;
            }

            base.Dispose(disposing);
        }

        private TableLayoutPanel BuildContentLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(24, 72, 24, 24),
                BackColor = SystemColors.Window
            };

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var titleLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = SystemColors.ControlText,
                Margin = new Padding(0, 0, 0, 12),
                Text = "Interactive Custom Caption Region"
            };

            var instructionsLabel = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(900, 0),
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = SystemColors.ControlText,
                Margin = new Padding(0, 0, 0, 16),
                Text = "This sample adds a custom '?' region to the caption bar using RegionDock.Caption and explicit caption-relative Bounds. " +
                       "Switch styles to compare right-aligned, traffic-light, and high-effects painters, then validate mouse hit-testing, F6 traversal, and accessibility toggles without changing the main sample-business-app shell."
            };

            var controlsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 0, 16),
                Padding = new Padding(0),
                Dock = DockStyle.Fill
            };

            controlsPanel.Controls.Add(new Label
            {
                AutoSize = true,
                Margin = new Padding(0, 8, 8, 0),
                Text = "Form style:"
            });

            _styleComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 160,
                Margin = new Padding(0, 4, 16, 0)
            };
            _styleComboBox.Items.AddRange(new object[]
            {
                FormStyleValue.Modern,
                FormStyleValue.MacOS,
                FormStyleValue.Neon,
                FormStyleValue.Glass
            });
            _styleComboBox.SelectedItem = FormStyle;
            _styleComboBox.SelectedIndexChanged += (_, _) =>
            {
                if (_styleComboBox.SelectedItem is FormStyleValue selectedStyle)
                {
                    FormStyle = selectedStyle;
                    UpdateCustomRegionBounds();
                    UpdateStatus($"Switched to {selectedStyle}. The custom caption region keeps its own caption-relative bounds.");
                }
            };
            controlsPanel.Controls.Add(_styleComboBox);

            _screenReaderCheckBox = new CheckBox
            {
                AutoSize = true,
                Checked = ScreenReaderSupport,
                Margin = new Padding(0, 8, 16, 0),
                Text = "Screen reader support"
            };
            _screenReaderCheckBox.CheckedChanged += (_, _) =>
            {
                ScreenReaderSupport = _screenReaderCheckBox.Checked;
                UpdateStatus($"ScreenReaderSupport is now {ScreenReaderSupport}.");
            };
            controlsPanel.Controls.Add(_screenReaderCheckBox);

            _highContrastCheckBox = new CheckBox
            {
                AutoSize = true,
                Checked = HighContrastMode,
                Margin = new Padding(0, 8, 16, 0),
                Text = "High contrast mode"
            };
            _highContrastCheckBox.CheckedChanged += (_, _) =>
            {
                HighContrastMode = _highContrastCheckBox.Checked;
                Invalidate(true);
                UpdateStatus($"HighContrastMode is now {HighContrastMode}.");
            };
            controlsPanel.Controls.Add(_highContrastCheckBox);

            _searchBoxCheckBox = new CheckBox
            {
                AutoSize = true,
                Checked = ShowSearchBox,
                Margin = new Padding(0, 8, 16, 0),
                Text = "Show search box"
            };
            _searchBoxCheckBox.CheckedChanged += (_, _) =>
            {
                ShowSearchBox = _searchBoxCheckBox.Checked;
                UpdateStatus($"ShowSearchBox is now {ShowSearchBox}. Re-test F6 traversal after changing optional caption targets.");
            };
            controlsPanel.Controls.Add(_searchBoxCheckBox);

            _profileButtonCheckBox = new CheckBox
            {
                AutoSize = true,
                Checked = ShowProfileButton,
                Margin = new Padding(0, 8, 0, 0),
                Text = "Show profile button"
            };
            _profileButtonCheckBox.CheckedChanged += (_, _) =>
            {
                ShowProfileButton = _profileButtonCheckBox.Checked;
                UpdateStatus($"ShowProfileButton is now {ShowProfileButton}. The custom region should still remain reachable.");
            };
            controlsPanel.Controls.Add(_profileButtonCheckBox);

            _statusLabel = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(900, 0),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(32, 78, 147),
                Margin = new Padding(0, 0, 0, 16),
                Text = string.Empty
            };

            var checklistLabel = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(900, 0),
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = SystemColors.ControlText,
                Text = "Manual checks: 1. Click the '?' region. 2. Use F6 or Shift+F6 until the caption help region is focused, then press Enter. " +
                       "3. Toggle screen reader support and high contrast mode. 4. Switch between Modern, MacOS, Neon, and Glass to confirm the region keeps its own slot instead of stretching across the full caption bar."
            };

            layout.Controls.Add(titleLabel, 0, 0);
            layout.Controls.Add(instructionsLabel, 0, 1);
            layout.Controls.Add(controlsPanel, 0, 2);
            layout.Controls.Add(_statusLabel, 0, 3);
            layout.Controls.Add(checklistLabel, 0, 4);
            layout.RowCount = 5;
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            return layout;
        }

        private void HandleRegionClick(object? sender, RegionEventArgs e)
        {
            if (!ReferenceEquals(e.Region, _helpRegion))
                return;

            SystemSounds.Asterisk.Play();
            UpdateStatus($"Custom caption region activated at {DateTime.Now:T}. Bounds: {e.Bounds}.");
        }

        private void PaintHelpRegion(Graphics graphics, Rectangle bounds)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            Color fillColor = HighContrastMode || SystemInformation.HighContrast
                ? Color.Black
                : Color.FromArgb(36, 98, 196);
            Color borderColor = HighContrastMode || SystemInformation.HighContrast
                ? Color.White
                : Color.FromArgb(19, 69, 150);
            Color glyphColor = HighContrastMode || SystemInformation.HighContrast
                ? Color.White
                : Color.White;

            using var fillBrush = new SolidBrush(fillColor);
            using var borderPen = new Pen(borderColor, HighContrastMode || SystemInformation.HighContrast ? 2f : 1f);

            graphics.FillEllipse(fillBrush, bounds);
            graphics.DrawEllipse(borderPen, bounds);
            TextRenderer.DrawText(
                graphics,
                "?",
                Font,
                bounds,
                glyphColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        private void UpdateContentPadding()
        {
            _contentLayout.Padding = new Padding(24, CaptionHeight + 24, 24, 24);
        }

        private void UpdateCustomRegionBounds()
        {
            int size = Math.Max(20, CaptionHeight - 10);
            int y = Math.Max(4, (CaptionHeight - size) / 2);
            _helpRegion.Bounds = new Rectangle(120, y, size, size);
            Invalidate();
        }

        private void UpdateStatus(string message)
        {
            _statusLabel.Text = message;
        }
    }
}