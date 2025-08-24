using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.TextFields;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;
using WinFormsApp.UI.Test.SampleBusinessApp.ViewModels;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Product
{
    [AddinAttribute(Caption = "Category Management", Name = "CategoryManagementView", misc = "SampleBusinessApp", menu = "Product",
        addinType = AddinType.Control, displayType = DisplayType.InControl, ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 320, RootNodeName = "Sample Business App", Order = 320, ID = 320,
        BranchText = "Category Management", BranchType = EnumPointType.Function,
        IconImageName = "categories.svg", BranchClass = "ADDIN",
        BranchDescription = "Manage product categories")]
    public class CategoryManagementView : TemplateUserControl, IAddinVisSchema
    {
        private readonly IDMEEditor _editor;
        private BeepPanel _toolbar;
        private BeepPanel _gridPanel;
        private BeepSimpleGrid _grid;
        private BeepTextBox _search;
        private BeepButton _addBtn;
        private BeepButton _editBtn;
        private BeepButton _deleteBtn;
        private BeepButton _refreshBtn;

        private List<Category> _categories = new();
        private Category _selected;

        #region Schema
        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 320;
        public int ID { get; set; } = 320;
        public string BranchText { get; set; } = "Category Management";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 320;
        public string IconImageName { get; set; } = "categories.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Manage product categories";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }
        #endregion

        public CategoryManagementView(IServiceProvider services) : base(services)
        {
            _editor = services.GetRequiredService<IDMEEditor>();
            InitializeComponent();
            BuildUI();
            WireEvents();
            LoadData();
        }

        private void BuildUI()
        {
            _toolbar = new BeepPanel { Dock = DockStyle.Top, Height = 64, ShowTitle = false, Padding = new Padding(12), Theme = this.Theme };
            _search = new BeepTextBox { PlaceholderText = "Search categories...", Location = new Point(12, 18), Size = new Size(220, 30), Theme = this.Theme };
            _addBtn = new BeepButton { Text = "➕ Add", Location = new Point(244, 18), Size = new Size(80, 30), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _editBtn = new BeepButton { Text = "✏️ Edit", Location = new Point(328, 18), Size = new Size(80, 30), BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Enabled = false, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _deleteBtn = new BeepButton { Text = "🗑️ Delete", Location = new Point(412, 18), Size = new Size(80, 30), BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, Enabled = false, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _refreshBtn = new BeepButton { Text = "🔄 Refresh", Location = new Point(496, 18), Size = new Size(90, 30), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _toolbar.Controls.AddRange(new Control[] { _search, _addBtn, _editBtn, _deleteBtn, _refreshBtn });

            _gridPanel = new BeepPanel { Dock = DockStyle.Fill, ShowTitle = true, TitleText = "🗂️ Categories", BackColor = Color.White, IsRounded = true, BorderRadius = 8, ShowShadow = true, Padding = new Padding(12), Theme = this.Theme };
            _grid = new BeepSimpleGrid { Dock = DockStyle.Fill, Theme = this.Theme };
            _gridPanel.Controls.Add(_grid);

            Controls.Add(_gridPanel);
            Controls.Add(_toolbar);
        }

        private void WireEvents()
        {
            _refreshBtn.Click += (s, e) => LoadData();
            _search.TextChanged += (s, e) => ApplyFilter();
            _grid.SelectedRowsChanged += (s, e) => UpdateSelection();
            _addBtn.Click += (s, e) => AddCategory();
            _editBtn.Click += (s, e) => EditCategory();
            _deleteBtn.Click += (s, e) => DeleteCategory();
        }

        private void LoadData()
        {
            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as TheTechIdea.Beep.DataBase.IRDBSource;
            ds?.Openconnection();
            _categories = ds?.GetData<Category>("SELECT Id, Name, Description, CreatedAt FROM Categories ORDER BY Name") ?? new List<Category>();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var list = _categories;
            if (!string.IsNullOrWhiteSpace(_search.Text))
            {
                var t = _search.Text.Trim().ToLower();
                list = list.Where(c => (c.Name ?? string.Empty).ToLower().Contains(t) || (c.Description ?? string.Empty).ToLower().Contains(t)).ToList();
            }
            _grid.DataSource = list;
            _editBtn.Enabled = _grid.SelectedRows.Count > 0;
            _deleteBtn.Enabled = _grid.SelectedRows.Count > 0;
        }

        private void UpdateSelection()
        {
            _selected = null;
            if (_grid.SelectedRows.Count > 0 && _grid.DataSource is List<Category> data)
            {
                var i = _grid.SelectedRows[0];
                if (i >= 0 && i < data.Count) _selected = data[i];
            }
        }

        private void AddCategory()
        {
            var name = Prompt("Category Name:");
            if (string.IsNullOrWhiteSpace(name)) return;
            name = name.Trim();
            if (_categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Category name must be unique.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as TheTechIdea.Beep.DataBase.IRDBSource;
            ds?.Openconnection();
            ds?.SaveData("INSERT INTO Categories (Name, CreatedAt) VALUES (@Name, @CreatedAt)", new { Name = name, CreatedAt = DateTime.UtcNow })?.Wait();
            LoadData();
        }

        private void EditCategory()
        {
            if (_selected == null) return;
            var name = Prompt("Edit Name:", _selected.Name);
            if (string.IsNullOrWhiteSpace(name)) return;
            name = name.Trim();
            if (_categories.Any(c => c.Id != _selected.Id && c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Another category with the same name exists.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as TheTechIdea.Beep.DataBase.IRDBSource;
            ds?.Openconnection();
            ds?.SaveData("UPDATE Categories SET Name=@Name WHERE Id=@Id", new { Name = name, Id = _selected.Id })?.Wait();
            LoadData();
        }

        private void DeleteCategory()
        {
            if (_selected == null) return;
            if (MessageBox.Show($"Delete category '{_selected.Name}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            var ds = Data.AppDbContext.EnsureSqliteDataSource(_editor) as TheTechIdea.Beep.DataBase.IRDBSource;
            ds?.Openconnection();
            ds?.SaveData("DELETE FROM Categories WHERE Id=@Id", new { Id = _selected.Id })?.Wait();
            LoadData();
        }

        private string Prompt(string title, string defaultText = "")
        {
            using var f = new Form { StartPosition = FormStartPosition.CenterParent, Width = 420, Height = 140, Text = title, MinimizeBox = false, MaximizeBox = false, ShowIcon = false, ShowInTaskbar = false };
            var tb = new TextBox { Left = 12, Top = 12, Width = 380, Text = defaultText };
            var ok = new Button { Text = "OK", Left = 232, Top = 50, DialogResult = DialogResult.OK };
            var cancel = new Button { Text = "Cancel", Left = 314, Top = 50, DialogResult = DialogResult.Cancel };
            f.Controls.AddRange(new Control[] { tb, ok, cancel });
            f.AcceptButton = ok; f.CancelButton = cancel;
            return f.ShowDialog(FindForm()) == DialogResult.OK ? tb.Text : string.Empty;
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = nameof(CategoryManagementView);
            Size = new Size(1000, 700);
            ResumeLayout(false);
        }

        public override void Configure(Dictionary<string, object> settings) => base.Configure(settings);
        public override void OnNavigatedTo(Dictionary<string, object> parameters) { base.OnNavigatedTo(parameters); LoadData(); }
        public override void Initialize() => base.Initialize();
    }
}
