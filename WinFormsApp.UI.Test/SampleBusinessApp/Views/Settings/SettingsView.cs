using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Vis.Modules.Managers;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Default.Views.Template;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Settings
{
    [AddinAttribute(Caption = "Settings", Name = "SettingsView", misc = "SampleBusinessApp", menu = "Administration", ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 500, RootNodeName = "Sample Business App", Order = 500, ID = 500,
        BranchText = "Settings", BranchType = EnumPointType.Function,
        IconImageName = "settings.svg", BranchClass = "ADDIN",
        BranchDescription = "Application settings and preferences")]
    public class SettingsView : TemplateUserControl, IAddinVisSchema
    {
        private BeepPanel _header;
        private BeepPanel _content;
        private BeepLabel _title;
        private BeepComboBox _themeCombo;
        private BeepButton _applyBtn;
        private BeepButton _cancelBtn;

        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 500;
        public int ID { get; set; } = 500;
        public string BranchText { get; set; } = "Settings";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 500;
        public string IconImageName { get; set; } = "settings.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Application settings and preferences";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }

        public SettingsView(IServiceProvider services) : base(services)
        {
            InitializeComponent();
            BuildUI();
            WireEvents();
        }

        private void BuildUI()
        {
            _header = new BeepPanel { Dock = DockStyle.Top, Height = 60, ShowTitle = false, Padding = new Padding(12), Theme = this.Theme };
            _title = new BeepLabel { Text = "?? Application Settings", AutoSize = true, Font = new Font("Segoe UI", 16, FontStyle.Bold), Theme = this.Theme };
            _header.Controls.Add(_title);

            _content = new BeepPanel { Dock = DockStyle.Fill, ShowTitle = false, Padding = new Padding(16), Theme = this.Theme };

            var themeLbl = new BeepLabel { Text = "Theme:", Location = new Point(16, 20), AutoSize = true, Theme = this.Theme };
            _themeCombo = new BeepComboBox { Location = new Point(16, 44), Size = new Size(240, 30), Theme = this.Theme };
            _themeCombo.Items.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Text = "BusinessProfessional", Value = "BusinessProfessional", Name = "BusinessProfessional" });
            _themeCombo.Items.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Text = "DefaultTheme", Value = "DefaultTheme", Name = "DefaultTheme" });
            _themeCombo.Items.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Text = "DarkTheme", Value = "DarkTheme", Name = "DarkTheme" });
            _themeCombo.SelectedIndex = 0;

            _applyBtn = new BeepButton { Text = "Apply", Location = new Point(16, 90), Size = new Size(90, 32), BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _cancelBtn = new BeepButton { Text = "Close", Location = new Point(118, 90), Size = new Size(90, 32), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };

            _content.Controls.AddRange(new Control[] { themeLbl, _themeCombo, _applyBtn, _cancelBtn });

            Controls.Add(_content);
            Controls.Add(_header);
        }

        private void WireEvents()
        {
            _applyBtn.Click += (s, e) => ApplySettings();
            _cancelBtn.Click += (s, e) => FindForm()?.Focus();
        }

        private void ApplySettings()
        {
            var selected = _themeCombo?.SelectedItem?.ToString() ?? "BusinessProfessional";
            // Update this view and notify parent form
            this.Theme = selected;
            if (FindForm() is Forms.MainBusinessForm main)
            {
                main.Theme = selected;
                main.UpdateStatus($"Theme applied: {selected}");
            }
        }

        public override void Configure(Dictionary<string, object> settings) => base.Configure(settings);
        public override void OnNavigatedTo(Dictionary<string, object> parameters) => base.OnNavigatedTo(parameters);
        public override void Initialize() => base.Initialize();

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = nameof(SettingsView);
            Size = new Size(900, 600);
            ResumeLayout(false);
        }
    }
}
