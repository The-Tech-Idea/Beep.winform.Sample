using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.Forms;
using TheTechIdea.Beep.Winform.Controls.Forms.ModernForm;
using TheTechIdea.Beep.Winform.Controls.ThemeManagement;
using FormStyleValue = TheTechIdea.Beep.Vis.Modules.FormStyle;

namespace WinFormsApp.UI.Test
{
    internal sealed class ModernFormDiagnosticsDemoForm : BeepiFormPro
    {
        private readonly bool _isCompanion;
        private readonly TableLayoutPanel _contentLayout;
        private ComboBox _styleComboBox;
        private ComboBox _themeComboBox;
        private ComboBox _paintBackdropComboBox;
        private ComboBox _windowBackdropComboBox;
        private CheckBox _globalSyncCheckBox;
        private CheckBox _searchBoxCheckBox;
        private CheckBox _profileButtonCheckBox;
        private CheckBox _themeButtonCheckBox;
        private CheckBox _styleButtonCheckBox;
        private CheckBox _highContrastCheckBox;
        private Label _statusLabel;
        private TextBox _diagnosticsTextBox;
        private readonly System.Windows.Forms.Timer _diagnosticsTimer;

        private bool _isSynchronizingSelections;
        private ModernFormDiagnosticsDemoForm _companionForm;

        public ModernFormDiagnosticsDemoForm(bool isCompanion = false)
        {
            _isCompanion = isCompanion;

            Text = isCompanion
                ? "ModernForm Diagnostics Demo - Companion"
                : "ModernForm Diagnostics Demo";
            StartPosition = isCompanion ? FormStartPosition.Manual : FormStartPosition.CenterScreen;
            MinimumSize = new Size(1120, 760);
            Size = new Size(1320, 860);

            FormStyle = FormStyleValue.Modern;
            ShowCaptionBar = true;
            ShowSearchBox = true;
            ShowProfileButton = true;
            ShowThemeButton = true;
            ShowStyleButton = true;
            ShowCustomActionButton = false;
            ScreenReaderSupport = true;
            FocusIndicatorStyle = FocusIndicatorStyle.Prominent;
            BackdropEffect = BackdropEffect.None;
            Backdrop = BackdropType.None;

            _contentLayout = BuildContentLayout();
            Controls.Add(_contentLayout);

            _diagnosticsTimer = new System.Windows.Forms.Timer { Interval = 400 };
            _diagnosticsTimer.Tick += DiagnosticsTimer_Tick;

            UpdateStatus(isCompanion
                ? "Companion diagnostics form opened. Use the primary host to test global theme and style synchronization."
                : "Use this diagnostics host to validate local and global theme or style changes, paint and Win32 backdrops, layout rectangles, and multi-form synchronization.");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            PopulateThemeChoices();
            SyncSelectionsFromState();
            UpdateContentPadding();
            RefreshDiagnostics();
            _diagnosticsTimer.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateContentPadding();
            RefreshDiagnostics();
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            RefreshDiagnostics();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (!_isCompanion && _companionForm != null && !_companionForm.IsDisposed)
            {
                try
                {
                    _companionForm.Close();
                }
                catch
                {
                    // Best effort during shutdown only.
                }
            }

            base.OnFormClosed(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _diagnosticsTimer.Tick -= DiagnosticsTimer_Tick;
                _diagnosticsTimer.Dispose();

                if (!_isCompanion && _companionForm != null)
                {
                    _companionForm.FormClosed -= CompanionForm_FormClosed;
                }
            }

            base.Dispose(disposing);
        }

        private TableLayoutPanel BuildContentLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Padding = new Padding(24, 72, 24, 24),
                BackColor = SystemColors.Window
            };

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
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
                Text = _isCompanion ? "ModernForm Diagnostics Companion" : "ModernForm Diagnostics Host"
            };

            var instructionsLabel = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(1160, 0),
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = SystemColors.ControlText,
                Margin = new Padding(0, 0, 0, 16),
                Text = "This sample exposes the live `BeepiFormPro` diagnostics needed for Phase 6 runtime checks: current painter, theme, style, window state, DPI, caption layout rectangles, paint-level backdrop, and Win32 backdrop. Open a companion form to validate global theme and style synchronization across multiple windows without launching the full business-shell host."
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

            controlsPanel.Controls.Add(CreateInlineLabel("Form style:"));
            _styleComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 170,
                Margin = new Padding(0, 4, 16, 0)
            };
            _styleComboBox.Items.AddRange(new object[]
            {
                FormStyleValue.Modern,
                FormStyleValue.MacOS,
                FormStyleValue.Fluent,
                FormStyleValue.GNOME,
                FormStyleValue.Glass,
                FormStyleValue.Neon
            });
            _styleComboBox.SelectedIndexChanged += (_, _) => ApplySelectedStyle();
            controlsPanel.Controls.Add(_styleComboBox);

            controlsPanel.Controls.Add(CreateInlineLabel("Theme:"));
            _themeComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 190,
                Margin = new Padding(0, 4, 16, 0)
            };
            _themeComboBox.SelectedIndexChanged += (_, _) => ApplySelectedTheme();
            controlsPanel.Controls.Add(_themeComboBox);

            controlsPanel.Controls.Add(CreateInlineLabel("Paint backdrop:"));
            _paintBackdropComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Margin = new Padding(0, 4, 16, 0)
            };
            _paintBackdropComboBox.Items.AddRange(Enum.GetValues(typeof(BackdropEffect)).Cast<object>().ToArray());
            _paintBackdropComboBox.SelectedIndexChanged += (_, _) => ApplySelectedPaintBackdrop();
            controlsPanel.Controls.Add(_paintBackdropComboBox);

            controlsPanel.Controls.Add(CreateInlineLabel("Win32 backdrop:"));
            _windowBackdropComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Margin = new Padding(0, 4, 16, 0)
            };
            _windowBackdropComboBox.Items.AddRange(Enum.GetValues(typeof(BackdropType)).Cast<object>().ToArray());
            _windowBackdropComboBox.SelectedIndexChanged += (_, _) => ApplySelectedWindowBackdrop();
            controlsPanel.Controls.Add(_windowBackdropComboBox);

            _globalSyncCheckBox = new CheckBox
            {
                AutoSize = true,
                Margin = new Padding(0, 8, 16, 0),
                Text = "Apply theme and style selections globally"
            };
            controlsPanel.Controls.Add(_globalSyncCheckBox);

            _searchBoxCheckBox = CreateOptionCheckBox("Show search box", value => ShowSearchBox = value);
            controlsPanel.Controls.Add(_searchBoxCheckBox);

            _profileButtonCheckBox = CreateOptionCheckBox("Show profile button", value => ShowProfileButton = value);
            controlsPanel.Controls.Add(_profileButtonCheckBox);

            _themeButtonCheckBox = CreateOptionCheckBox("Show theme button", value => ShowThemeButton = value);
            controlsPanel.Controls.Add(_themeButtonCheckBox);

            _styleButtonCheckBox = CreateOptionCheckBox("Show style button", value => ShowStyleButton = value);
            controlsPanel.Controls.Add(_styleButtonCheckBox);

            _highContrastCheckBox = CreateOptionCheckBox("High contrast mode", value => HighContrastMode = value);
            controlsPanel.Controls.Add(_highContrastCheckBox);

            var actionsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 0, 0, 16),
                Padding = new Padding(0),
                Dock = DockStyle.Fill
            };

            var refreshButton = new Button
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 12, 0),
                Text = "Refresh diagnostics"
            };
            refreshButton.Click += (_, _) =>
            {
                RefreshDiagnostics();
                UpdateStatus("Diagnostics refreshed from the live form state.");
            };
            actionsPanel.Controls.Add(refreshButton);

            var pushGlobalButton = new Button
            {
                AutoSize = true,
                Margin = new Padding(0, 0, 12, 0),
                Text = "Push current theme and style globally"
            };
            pushGlobalButton.Click += (_, _) => PushCurrentStateGlobally();
            actionsPanel.Controls.Add(pushGlobalButton);

            if (!_isCompanion)
            {
                var openCompanionButton = new Button
                {
                    AutoSize = true,
                    Margin = new Padding(0, 0, 12, 0),
                    Text = "Open companion form"
                };
                openCompanionButton.Click += (_, _) => OpenCompanionForm();
                actionsPanel.Controls.Add(openCompanionButton);
            }

            var captionDemoHint = new Label
            {
                AutoSize = true,
                Margin = new Padding(0, 8, 0, 0),
                Text = "Use `--demo custom-caption-region` for the dedicated caption-region interaction checks."
            };
            actionsPanel.Controls.Add(captionDemoHint);

            _statusLabel = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(1160, 0),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(32, 78, 147),
                Margin = new Padding(0, 0, 0, 12),
                Text = string.Empty
            };

            _diagnosticsTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                WordWrap = false,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(249, 250, 252),
                Font = new Font("Consolas", 10f, FontStyle.Regular)
            };

            layout.Controls.Add(titleLabel, 0, 0);
            layout.Controls.Add(instructionsLabel, 0, 1);
            layout.Controls.Add(controlsPanel, 0, 2);
            layout.Controls.Add(actionsPanel, 0, 3);
            layout.Controls.Add(_statusLabel, 0, 4);
            layout.Controls.Add(_diagnosticsTextBox, 0, 5);

            return layout;
        }

        private static Label CreateInlineLabel(string text)
        {
            return new Label
            {
                AutoSize = true,
                Margin = new Padding(0, 8, 8, 0),
                Text = text
            };
        }

        private CheckBox CreateOptionCheckBox(string text, Action<bool> applyAction)
        {
            var checkBox = new CheckBox
            {
                AutoSize = true,
                Margin = new Padding(0, 8, 16, 0),
                Text = text
            };

            checkBox.CheckedChanged += (_, _) =>
            {
                if (_isSynchronizingSelections)
                {
                    return;
                }

                applyAction(checkBox.Checked);
                UpdateContentPadding();
                RefreshDiagnostics();
                UpdateStatus($"{text} is now {checkBox.Checked}.");
            };

            return checkBox;
        }

        private void PopulateThemeChoices()
        {
            _themeComboBox.Items.Clear();

            var themeNames = BeepThemesManager
                .GetThemes()
                .Select(theme => theme.ThemeName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .Cast<object>()
                .ToArray();

            if (themeNames.Length == 0)
            {
                _themeComboBox.Items.Add(CurrentTheme.ThemeName);
            }
            else
            {
                _themeComboBox.Items.AddRange(themeNames);
            }
        }

        private void ApplySelectedStyle()
        {
            if (_isSynchronizingSelections || _styleComboBox.SelectedItem is not FormStyleValue selectedStyle)
            {
                return;
            }

            if (_globalSyncCheckBox.Checked)
            {
                BeepThemesManager.SetCurrentStyle(selectedStyle);
                UpdateStatus($"Pushed {selectedStyle} as the global form style for all open Beep forms.");
            }
            else
            {
                FormStyle = selectedStyle;
                UpdateStatus($"Applied {selectedStyle} locally to this diagnostics form.");
            }

            UpdateContentPadding();
            RefreshDiagnostics();
        }

        private void ApplySelectedTheme()
        {
            if (_isSynchronizingSelections || _themeComboBox.SelectedItem is not string selectedThemeName)
            {
                return;
            }

            if (_globalSyncCheckBox.Checked)
            {
                BeepThemesManager.SetCurrentTheme(selectedThemeName);
                UpdateStatus($"Pushed {selectedThemeName} as the global Beep theme.");
            }
            else
            {
                ApplyTheme(selectedThemeName);
                UpdateStatus($"Applied {selectedThemeName} locally to this diagnostics form.");
            }

            RefreshDiagnostics();
        }

        private void ApplySelectedPaintBackdrop()
        {
            if (_isSynchronizingSelections || _paintBackdropComboBox.SelectedItem is not BackdropEffect selectedBackdropEffect)
            {
                return;
            }

            BackdropEffect = selectedBackdropEffect;
            Invalidate(true);
            RefreshDiagnostics();
            UpdateStatus($"Paint backdrop is now {selectedBackdropEffect}. This affects the custom paint pipeline only.");
        }

        private void ApplySelectedWindowBackdrop()
        {
            if (_isSynchronizingSelections || _windowBackdropComboBox.SelectedItem is not BackdropType selectedBackdrop)
            {
                return;
            }

            Backdrop = selectedBackdrop;
            RefreshDiagnostics();
            UpdateStatus($"Win32 backdrop is now {selectedBackdrop}. This re-applies the native window backdrop when the handle exists.");
        }

        private void PushCurrentStateGlobally()
        {
            BeepThemesManager.SetCurrentTheme(ThemeName);
            BeepThemesManager.SetCurrentStyle(FormStyle);
            UpdateStatus($"Pushed the current theme ({ThemeName}) and form style ({FormStyle}) globally.");
            RefreshDiagnostics();
        }

        private void OpenCompanionForm()
        {
            if (_companionForm != null && !_companionForm.IsDisposed)
            {
                _companionForm.Activate();
                return;
            }

            _companionForm = new ModernFormDiagnosticsDemoForm(isCompanion: true)
            {
                Location = new Point(Location.X + 80, Location.Y + 80)
            };

            CopyCurrentStateTo(_companionForm);
            _companionForm.FormClosed += CompanionForm_FormClosed;
            _companionForm.Show(this);
            UpdateStatus("Opened a companion diagnostics form. Use global theme and style changes to confirm both windows stay synchronized.");
        }

        private void CompanionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_companionForm != null)
            {
                _companionForm.FormClosed -= CompanionForm_FormClosed;
                _companionForm = null;
            }
        }

        private void CopyCurrentStateTo(ModernFormDiagnosticsDemoForm target)
        {
            target.FormStyle = FormStyle;
            target.ApplyTheme(ThemeName);
            target.BackdropEffect = BackdropEffect;
            target.Backdrop = Backdrop;
            target.ShowSearchBox = ShowSearchBox;
            target.ShowProfileButton = ShowProfileButton;
            target.ShowThemeButton = ShowThemeButton;
            target.ShowStyleButton = ShowStyleButton;
            target.HighContrastMode = HighContrastMode;
            target.UpdateStatus("Companion synchronized from the primary diagnostics host.");
        }

        private void DiagnosticsTimer_Tick(object sender, EventArgs e)
        {
            RefreshDiagnostics();
        }

        private void SyncSelectionsFromState()
        {
            _isSynchronizingSelections = true;
            try
            {
                _styleComboBox.SelectedItem = FormStyle;

                if (_themeComboBox.Items.Count > 0 && _themeComboBox.Items.Contains(ThemeName))
                {
                    _themeComboBox.SelectedItem = ThemeName;
                }
                else if (_themeComboBox.Items.Count > 0)
                {
                    _themeComboBox.SelectedItem = _themeComboBox.Items[0];
                }

                _paintBackdropComboBox.SelectedItem = BackdropEffect;
                _windowBackdropComboBox.SelectedItem = Backdrop;
                _searchBoxCheckBox.Checked = ShowSearchBox;
                _profileButtonCheckBox.Checked = ShowProfileButton;
                _themeButtonCheckBox.Checked = ShowThemeButton;
                _styleButtonCheckBox.Checked = ShowStyleButton;
                _highContrastCheckBox.Checked = HighContrastMode;
            }
            finally
            {
                _isSynchronizingSelections = false;
            }
        }

        private void RefreshDiagnostics()
        {
            if (IsDisposed || Disposing)
            {
                return;
            }

            SyncSelectionsFromState();

            var metrics = FormPainterMetrics.DefaultForCached(FormStyle, UseThemeColors ? CurrentTheme : null);
            var builder = new StringBuilder();
            builder.AppendLine(_isCompanion ? "Form Role: Companion" : "Form Role: Primary diagnostics host");
            builder.AppendLine($"Active Painter: {ActivePainter?.GetType().Name ?? "(none)"}");
            builder.AppendLine($"FormStyle: {FormStyle}");
            builder.AppendLine($"ThemeName: {ThemeName}");
            builder.AppendLine($"CurrentTheme: {CurrentTheme?.ThemeName ?? "(none)"}");
            builder.AppendLine($"Global Theme: {BeepThemesManager.CurrentThemeName}");
            builder.AppendLine($"Global Style: {BeepThemesManager.CurrentStyle}");
            builder.AppendLine($"Paint BackdropEffect: {BackdropEffect}");
            builder.AppendLine($"Win32 Backdrop: {Backdrop}");
            builder.AppendLine($"WindowState: {WindowState}");
            builder.AppendLine($"Location: {Location}");
            builder.AppendLine($"ClientSize: {ClientSize.Width} x {ClientSize.Height}");
            builder.AppendLine($"DisplayRectangle: {FormatRectangle(DisplayRectangle)}");
            builder.AppendLine($"CaptionHeight: {CaptionHeight}");
            builder.AppendLine($"DeviceDpi: {DeviceDpi}");
            builder.AppendLine($"HighContrastMode: {HighContrastMode}");
            builder.AppendLine($"ScreenReaderSupport: {ScreenReaderSupport}");
            builder.AppendLine($"Search/Profile/Theme/Style Buttons: {ShowSearchBox}/{ShowProfileButton}/{ShowThemeButton}/{ShowStyleButton}");
            builder.AppendLine();
            builder.AppendLine("Current Metrics:");
            builder.AppendLine($"  CaptionHeight       = {metrics.CaptionHeight}");
            builder.AppendLine($"  ButtonWidth         = {metrics.ButtonWidth}");
            builder.AppendLine($"  AuxiliaryButtonWidth= {metrics.AuxiliaryButtonWidth}");
            builder.AppendLine($"  VisualButtonWidth   = {metrics.VisualButtonWidth}");
            builder.AppendLine($"  SearchBoxWidth      = {metrics.SearchBoxWidth}");
            builder.AppendLine($"  SearchBoxPadding    = {metrics.SearchBoxPadding}");
            builder.AppendLine($"  ButtonsPlacement    = {metrics.ButtonsPlacement}");
            builder.AppendLine();
            builder.AppendLine("CurrentLayout:");
            builder.AppendLine($"  CaptionRect        = {FormatRectangle(CurrentLayout.CaptionRect)}");
            builder.AppendLine($"  ContentRect        = {FormatRectangle(CurrentLayout.ContentRect)}");
            builder.AppendLine($"  SearchBoxRect      = {FormatRectangle(CurrentLayout.SearchBoxRect)}");
            builder.AppendLine($"  ProfileButtonRect  = {FormatRectangle(CurrentLayout.ProfileButtonRect)}");
            builder.AppendLine($"  ThemeButtonRect    = {FormatRectangle(CurrentLayout.ThemeButtonRect)}");
            builder.AppendLine($"  StyleButtonRect    = {FormatRectangle(CurrentLayout.StyleButtonRect)}");
            builder.AppendLine($"  MinimizeButtonRect = {FormatRectangle(CurrentLayout.MinimizeButtonRect)}");
            builder.AppendLine($"  MaximizeButtonRect = {FormatRectangle(CurrentLayout.MaximizeButtonRect)}");
            builder.AppendLine($"  CloseButtonRect    = {FormatRectangle(CurrentLayout.CloseButtonRect)}");
            builder.AppendLine();
            builder.AppendLine("Registered Hit Areas:");

            var hitAreas = GetRegisteredHitAreasSnapshot();
            if (hitAreas.Count == 0)
            {
                builder.AppendLine("  (none)");
            }
            else
            {
                foreach (var hitArea in hitAreas.OrderBy(area => area.Name, StringComparer.OrdinalIgnoreCase))
                {
                    builder.AppendLine($"  {hitArea.Name,-18} = {FormatRectangle(hitArea.Bounds)}");
                }
            }

            builder.AppendLine();
            builder.AppendLine("Suggested checks:");
            builder.AppendLine("  1. Switch styles locally, then globally, and confirm the active painter and rectangles update.");
            builder.AppendLine("  2. Open the companion form and push a global theme/style change.");
            builder.AppendLine("  3. Compare CurrentLayout rectangles and registered hit areas against the current metrics values for the active style.");
            builder.AppendLine("  4. Cycle Paint BackdropEffect and Win32 Backdrop independently.");
            builder.AppendLine("  5. Resize, maximize, restore, and confirm the layout rectangles, hit areas, and metrics relationships remain coherent.");

            _diagnosticsTextBox.Text = builder.ToString();
        }

        private void UpdateContentPadding()
        {
            _contentLayout.Padding = new Padding(24, CaptionHeight + 28, 24, 24);
        }

        private void UpdateStatus(string message)
        {
            _statusLabel.Text = message;
        }

        private static string FormatRectangle(Rectangle rectangle)
        {
            return $"X={rectangle.X}, Y={rectangle.Y}, W={rectangle.Width}, H={rectangle.Height}";
        }
    }
}