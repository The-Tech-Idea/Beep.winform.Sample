using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using WinFormsApp.UI.Test.SampleBusinessApp.ViewModels;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Winform.Controls.TextFields;
 

namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Product
{
    [AddinAttribute(Caption = "Inventory", Name = "InventoryView", misc = "SampleBusinessApp", menu = "Product",
        addinType = AddinType.Control, displayType = DisplayType.InControl, ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 310, RootNodeName = "Sample Business App", Order = 310, ID = 310,
        BranchText = "Inventory", BranchType = EnumPointType.Function,
        IconImageName = "inventory.svg", BranchClass = "ADDIN",
        BranchDescription = "Manage product stock levels")]
    public class InventoryView : TemplateUserControl, IAddinVisSchema
    {
        private readonly ProductViewModel _vm;
        private BeepPanel _toolbar;
        private BeepPanel _gridPanel;
        private BeepSimpleGrid _grid;
        private BeepTextBox _search;
        private BeepButton _increaseBtn;
        private BeepButton _decreaseBtn;
        private BeepButton _refreshBtn;
        private BeepNumericUpDown _qty;

        private List<Models.Product> _products = new();
        private Models.Product _selected;

        #region Schema
        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 310;
        public int ID { get; set; } = 310;
        public string BranchText { get; set; } = "Inventory";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 310;
        public string IconImageName { get; set; } = "inventory.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Manage product stock levels";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }
        #endregion

        public InventoryView(IServiceProvider services) : base(services)
        {
            var editor = services.GetRequiredService<IDMEEditor>();
            _vm = new ProductViewModel(editor);
            InitializeComponent();
            BuildUI();
            WireEvents();
            LoadData();
        }

        private void BuildUI()
        {
            _toolbar = new BeepPanel
            {
                Dock = DockStyle.Top,
                Height = 64,
                ShowTitle = false,
                Padding = new Padding(12),
                Theme = this.Theme
            };

            _search = new BeepTextBox { PlaceholderText = "Search products...", Location = new Point(12, 18), Size = new Size(220, 30), Theme = this.Theme };
            _qty = new BeepNumericUpDown { Location = new Point(244, 18), Size = new Size(100, 30), MinimumValue = 1, MaximumValue = 10000, IncrementValue = 1, Value = 1, Theme = this.Theme };
            _increaseBtn = new BeepButton { Text = "➕ Increase", Location = new Point(352, 18), Size = new Size(100, 30), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _decreaseBtn = new BeepButton { Text = "➖ Decrease", Location = new Point(458, 18), Size = new Size(110, 30), BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _refreshBtn = new BeepButton { Text = "🔄 Refresh", Location = new Point(574, 18), Size = new Size(90, 30), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };

            _toolbar.Controls.AddRange(new Control[] { _search, _qty, _increaseBtn, _decreaseBtn, _refreshBtn });

            _gridPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = true,
                TitleText = "📦 Inventory",
                BackColor = Color.White,
                IsRounded = true,
                BorderRadius = 8,
                ShowShadow = true,
                Padding = new Padding(12),
                Theme = this.Theme
            };

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
            _increaseBtn.Click += (s, e) => AdjustStock((int)_qty.Value);
            _decreaseBtn.Click += (s, e) => AdjustStock(-(int)_qty.Value);
        }

        private void LoadData()
        {
            _products = _vm.GetProducts();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var list = _products;
            if (!string.IsNullOrWhiteSpace(_search.Text))
            {
                var t = _search.Text.Trim().ToLower();
                list = list.Where(p => (p.Name ?? string.Empty).ToLower().Contains(t) || (p.Description ?? string.Empty).ToLower().Contains(t)).ToList();
            }
            _grid.DataSource = list;
        }

        private void UpdateSelection()
        {
            _selected = null;
            if (_grid.SelectedRows.Count > 0 && _grid.DataSource is List<Models.Product> data)
            {
                var i = _grid.SelectedRows[0];
                if (i >= 0 && i < data.Count) _selected = data[i];
            }
        }

        private void AdjustStock(int delta)
        {
            if (_selected == null) return;
            var newStock = Math.Max(0, _selected.Stock + delta);
            // Persist using update
            _selected.Stock = newStock;
            var res = _vm.UpdateProduct(_selected);
            if (res.Flag == Errors.Ok)
            {
                LoadData();
            }
            else
            {
                MessageBox.Show(res.Message ?? "Stock update failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void Configure(Dictionary<string, object> settings) => base.Configure(settings);
        public override void OnNavigatedTo(Dictionary<string, object> parameters) { base.OnNavigatedTo(parameters); LoadData(); }
        public override void Initialize() => base.Initialize();

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = nameof(InventoryView);
            Size = new Size(1000, 700);
            ResumeLayout(false);
        }
    }
}
