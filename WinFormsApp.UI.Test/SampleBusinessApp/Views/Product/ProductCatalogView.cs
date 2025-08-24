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
using TheTechIdea.Beep.Winform.Controls.Models;
using TheTechIdea.Beep.Winform.Controls.TextFields;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;
using WinFormsApp.UI.Test.SampleBusinessApp.ViewModels;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Product
{
    [AddinAttribute(Caption = "Product Catalog", Name = "ProductCatalogView", misc = "SampleBusinessApp", menu = "Product",
        addinType = AddinType.Control, displayType = DisplayType.InControl, ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 300, RootNodeName = "Sample Business App", Order = 300, ID = 300,
        BranchText = "Product Catalog", BranchType = EnumPointType.Function,
        IconImageName = "products.svg", BranchClass = "ADDIN",
        BranchDescription = "Browse and manage products")]
    public class ProductCatalogView : TemplateUserControl, IAddinVisSchema
    {
        private readonly ProductViewModel _viewModel;
        private readonly IServiceProvider _services;

        // UI
        private BeepMultiSplitter _layout;
        private BeepPanel _toolbar;
        private BeepPanel _gridPanel;
        private BeepPanel _detailsPanel;

        private BeepButton _addBtn;
        private BeepButton _editBtn;
        private BeepButton _deleteBtn;
        private BeepButton _refreshBtn;
        private BeepTextBox _searchBox;
        private BeepComboBox _categoryFilter;

        private BeepSimpleGrid _grid;

        // Details
        private BeepLabel _nameLbl;
        private BeepLabel _priceLbl;
        private BeepLabel _stockLbl;
        private BeepLabel _categoryLbl;
        private BeepLabel _createdLbl;

        private List<Models.Product> _products = new();
        private Models.Product _selected;

        #region IAddinVisSchema
        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 300;
        public int ID { get; set; } = 300;
        public string BranchText { get; set; } = "Product Catalog";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 300;
        public string IconImageName { get; set; } = "products.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Browse and manage products";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }
        #endregion

        public ProductCatalogView(IServiceProvider services) : base(services)
        {
            _services = services;
            // Build VM using the registered editor
            var editor = services.GetRequiredService<IDMEEditor>();
            _viewModel = new ProductViewModel(editor);

            InitializeComponent();
            BuildUI();
            WireEvents();
            LoadProducts();
        }

        private void BuildUI()
        {
            _layout = new BeepMultiSplitter { Dock = DockStyle.Fill, Theme = this.Theme };
            _layout.tableLayoutPanel.RowCount = 2;
            _layout.tableLayoutPanel.ColumnCount = 2;
            _layout.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            _layout.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _layout.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            _layout.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            // Toolbar
            _toolbar = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = false,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(12),
                Theme = this.Theme
            };

            _addBtn = new BeepButton { Text = "? Add", Size = new Size(80, 32), Location = new Point(12, 20), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _editBtn = new BeepButton { Text = "?? Edit", Size = new Size(80, 32), Location = new Point(98, 20), BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White, Enabled = false, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _deleteBtn = new BeepButton { Text = "??? Delete", Size = new Size(80, 32), Location = new Point(184, 20), BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, Enabled = false, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _refreshBtn = new BeepButton { Text = "?? Refresh", Size = new Size(90, 32), Location = new Point(270, 20), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };

            var searchLbl = new BeepLabel { Text = "Search:", Location = new Point(390, 25), AutoSize = true, Theme = this.Theme };
            _searchBox = new BeepTextBox { Location = new Point(450, 20), Size = new Size(220, 30), PlaceholderText = "Search products...", Theme = this.Theme };

            var catLbl = new BeepLabel { Text = "Category:", Location = new Point(690, 25), AutoSize = true, Theme = this.Theme };
            _categoryFilter = new BeepComboBox { Location = new Point(760, 20), Size = new Size(160, 30), Theme = this.Theme };
            _categoryFilter.Items.AddRange(new[] { "All Categories", "Software", "Books", "Electronics", "Clothing", "General" });
            _categoryFilter.SelectedIndex = 0;

            _toolbar.Controls.AddRange(new Control[] { _addBtn, _editBtn, _deleteBtn, _refreshBtn, searchLbl, _searchBox, catLbl, _categoryFilter });

            // Grid panel
            _gridPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = true,
                TitleText = "?? Products",
                BackColor = Color.White,
                IsRounded = true,
                BorderRadius = 8,
                ShowShadow = true,
                Padding = new Padding(12),
                Theme = this.Theme
            };

            _grid = new BeepSimpleGrid { Dock = DockStyle.Fill, Theme = this.Theme };
            _gridPanel.Controls.Add(_grid);

            // Details
            _detailsPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = true,
                TitleText = "?? Product Details",
                BackColor = Color.White,
                IsRounded = true,
                BorderRadius = 8,
                ShowShadow = true,
                Padding = new Padding(12),
                Theme = this.Theme
            };

            _nameLbl = new BeepLabel { Text = "Name:", Location = new Point(12, 24), AutoSize = true, Theme = this.Theme };
            _priceLbl = new BeepLabel { Text = "Price:", Location = new Point(12, 54), AutoSize = true, Theme = this.Theme };
            _stockLbl = new BeepLabel { Text = "Stock:", Location = new Point(12, 84), AutoSize = true, Theme = this.Theme };
            _categoryLbl = new BeepLabel { Text = "Category:", Location = new Point(12, 114), AutoSize = true, Theme = this.Theme };
            _createdLbl = new BeepLabel { Text = "Created:", Location = new Point(12, 144), AutoSize = true, Theme = this.Theme };

            _detailsPanel.Controls.AddRange(new Control[] { _nameLbl, _priceLbl, _stockLbl, _categoryLbl, _createdLbl });

            Controls.Add(_layout);
            _layout.tableLayoutPanel.Controls.Add(_toolbar, 0, 0);
            _layout.tableLayoutPanel.SetColumnSpan(_toolbar, 2);
            _layout.tableLayoutPanel.Controls.Add(_gridPanel, 0, 1);
            _layout.tableLayoutPanel.Controls.Add(_detailsPanel, 1, 1);
        }

        private void WireEvents()
        {
            _addBtn.Click += (s, e) => AddProduct();
            _editBtn.Click += (s, e) => EditProduct();
            _deleteBtn.Click += (s, e) => DeleteProduct();
            _refreshBtn.Click += (s, e) => LoadProducts();

            _searchBox.TextChanged += (s, e) => { ApplyFilters(); UpdateHeader(); };
            _categoryFilter.SelectedItemChanged += (s, e) => { ApplyFilters(); UpdateHeader(); };

            _grid.SelectedRowsChanged += (s, e) => OnGridSelectionChanged();
        }

        private void LoadProducts()
        {
            _products = _viewModel.GetProducts();
            ApplyFilters();
            UpdateHeader();
        }

        private void ApplyFilters()
        {
            IEnumerable<Models.Product> filtered = _products;

            // Search
            if (!string.IsNullOrWhiteSpace(_searchBox.Text))
            {
                var t = _searchBox.Text.Trim().ToLower();
                filtered = filtered.Where(p => (p.Name ?? string.Empty).ToLower().Contains(t) || (p.Description ?? string.Empty).ToLower().Contains(t));
            }

            // Category
            var cat = _categoryFilter?.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(cat) && cat != "All Categories")
            {
                filtered = _viewModel.GetProductsByCategory(cat);
            }

            _grid.DataSource = filtered.ToList();
            _editBtn.Enabled = _grid.SelectedRows.Count > 0;
            _deleteBtn.Enabled = _grid.SelectedRows.Count > 0;
        }

        private void UpdateHeader()
        {
            var total = _products?.Count ?? 0;
            var filtered = (_grid.DataSource as List<Models.Product>)?.Count ?? 0;
            _gridPanel.TitleText = filtered == total ? $"?? Products ({total})" : $"?? Products ({filtered} of {total})";
        }

        private void OnGridSelectionChanged()
        {
            if (_grid.SelectedRows.Count > 0 && _grid.DataSource is List<Models.Product> data)
            {
                var idx = _grid.SelectedRows[0];
                if (idx >= 0 && idx < data.Count)
                {
                    _selected = data[idx];
                    _nameLbl.Text = $"Name: {_selected.Name}";
                    _priceLbl.Text = $"Price: {_selected.Price:C}";
                    _stockLbl.Text = $"Stock: {_selected.Stock}";
                    _categoryLbl.Text = $"Category: {_selected.Category}";
                    _createdLbl.Text = $"Created: {_selected.CreatedAt:MMM dd, yyyy}";

                    _editBtn.Enabled = true;
                    _deleteBtn.Enabled = true;
                    return;
                }
            }

            _selected = null;
            _nameLbl.Text = "Name:";
            _priceLbl.Text = "Price:";
            _stockLbl.Text = "Stock:";
            _categoryLbl.Text = "Category:";
            _createdLbl.Text = "Created:";
            _editBtn.Enabled = false;
            _deleteBtn.Enabled = false;
        }

        private void AddProduct()
        {
            var dlg = new Views.Product.ProductEditView(_services);
            dlg.ProductSaved += (s, p) => LoadProducts();
            ShowDialog(dlg, "Add Product");
        }

        private void EditProduct()
        {
            if (_selected == null) return;
            var dlg = new Views.Product.ProductEditView(_services);
            dlg.OnNavigatedTo(new Dictionary<string, object> { ["ProductId"] = _selected.Id });
            dlg.ProductSaved += (s, p) => LoadProducts();
            ShowDialog(dlg, "Edit Product");
        }

        private void DeleteProduct()
        {
            if (_selected == null) return;
            if (MessageBox.Show($"Delete product '{_selected.Name}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var res = _viewModel.DeleteProduct(_selected.Id);
                if (res.Flag == Errors.Ok)
                    LoadProducts();
                else
                    MessageBox.Show(res.Message ?? "Delete failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDialog(Control control, string title)
        {
            using var f = new Form
            {
                StartPosition = FormStartPosition.CenterParent,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowIcon = false,
                ShowInTaskbar = false,
                Text = title
            };
            control.Dock = DockStyle.Fill;
            f.Controls.Add(control);
            f.Padding = new Padding(8);
            f.ShowDialog(FindForm());
        }

        public override void Configure(Dictionary<string, object> settings)
        {
            base.Configure(settings);
        }

        public override void OnNavigatedTo(Dictionary<string, object> parameters)
        {
            base.OnNavigatedTo(parameters);
            LoadProducts();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = nameof(ProductCatalogView);
            Size = new Size(1200, 800);
            ResumeLayout(false);
        }
    }
}
