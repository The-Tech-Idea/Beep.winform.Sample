using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.Models;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using Microsoft.Extensions.DependencyInjection;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;
using WinFormsApp.UI.Test.SampleBusinessApp.ViewModels;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Winform.Controls.TextFields;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Product
{
    [AddinAttribute(Caption = "Product Editor", Name = "ProductEditView", misc = "SampleBusinessApp", menu = "Product",
        addinType = AddinType.Control, displayType = DisplayType.InControl, ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 301, RootNodeName = "Sample Business App", Order = 301, ID = 301,
        BranchText = "Product Editor", BranchType = EnumPointType.Function,
        IconImageName = "product-edit.svg", BranchClass = "ADDIN",
        BranchDescription = "Create or edit product information")]
    public class ProductEditView : TemplateUserControl, IAddinVisSchema
    {
        private readonly ProductViewModel _vm;

        private BeepPanel _formPanel;
        private BeepPanel _footer;
        private BeepTextBox txtName;
        private BeepTextBox txtDescription;
        private BeepNumericUpDown numPrice;
        private BeepNumericUpDown numStock;
        private BeepComboBox cboCategory;
        private BeepButton btnSave;
        private BeepButton btnCancel;
        private ErrorProvider _errors;

        private Models.Product _product = new Models.Product();
        private bool _isEditMode = false;

        #region IAddinVisSchema
        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 301;
        public int ID { get; set; } = 301;
        public string BranchText { get; set; } = "Product Editor";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 301;
        public string IconImageName { get; set; } = "product-edit.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Create or edit product information";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }
        #endregion

        public event EventHandler<Models.Product> ProductSaved;

        public ProductEditView(IServiceProvider services) : base(services)
        {
            var editor = services.GetRequiredService<IDMEEditor>();
            _vm = new ProductViewModel(editor);
            InitializeComponent();
            BuildUI();
            WireEvents();
        }

        private void BuildUI()
        {
            _formPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = true,
                TitleText = "?? Product",
                Padding = new Padding(16),
                Theme = this.Theme
            };

            int x = 16, y = 24, w = 360, dy = 40;
            txtName = NewText("Name", x, y, w); y += dy;
            txtDescription = NewText("Description", x, y, w); y += dy;

            numPrice = new BeepNumericUpDown { Location = new Point(x, y), Size = new Size(w, 30), MinimumValue = 0, MaximumValue = 1000000, IncrementValue = 1, DecimalPlaces = 2, Prefix = "$", Theme = this.Theme };
            _formPanel.Controls.Add(new BeepLabel { Text = "Price", Location = new Point(x, y - 18), AutoSize = true, Theme = this.Theme });
            _formPanel.Controls.Add(numPrice); y += dy;

            numStock = new BeepNumericUpDown { Location = new Point(x, y), Size = new Size(w, 30), MinimumValue = 0, MaximumValue = 100000, IncrementValue = 1, Theme = this.Theme };
            _formPanel.Controls.Add(new BeepLabel { Text = "Stock", Location = new Point(x, y - 18), AutoSize = true, Theme = this.Theme });
            _formPanel.Controls.Add(numStock); y += dy;

            cboCategory = new BeepComboBox { Location = new Point(x, y), Size = new Size(w, 30), Theme = this.Theme };
            _formPanel.Controls.Add(new BeepLabel { Text = "Category", Location = new Point(x, y - 18), AutoSize = true, Theme = this.Theme });
            foreach (var c in new[] { "Software", "Books", "Electronics", "Clothing", "General" })
                cboCategory.Items.Add(new SimpleItem { Text = c, Value = c, Name = c });
            cboCategory.SelectedIndex = 0;

            _footer = new BeepPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                ShowTitle = false,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(16),
                Theme = this.Theme
            };

            btnSave = new BeepButton { Text = "?? Save", Size = new Size(100, 34), Location = new Point(Width - 240, 12), Anchor = AnchorStyles.Right | AnchorStyles.Top, BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            btnCancel = new BeepButton { Text = "? Cancel", Size = new Size(100, 34), Location = new Point(Width - 128, 12), Anchor = AnchorStyles.Right | AnchorStyles.Top, BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };

            _footer.Controls.Add(btnSave);
            _footer.Controls.Add(btnCancel);

            Controls.Add(_formPanel);
            Controls.Add(_footer);

            _errors = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };
        }

        private BeepTextBox NewText(string label, int x, int y, int w)
        {
            _formPanel.Controls.Add(new BeepLabel { Text = label, Location = new Point(x, y - 18), AutoSize = true, Theme = this.Theme });
            var tb = new BeepTextBox { Location = new Point(x, y), Size = new Size(w, 30), Theme = this.Theme };
            _formPanel.Controls.Add(tb);
            return tb;
        }

        private void WireEvents()
        {
            btnSave.Click += (s, e) => Save();
            btnCancel.Click += (s, e) => FindForm()?.Close();
        }

        private bool ValidateInputs()
        {
            _errors.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            { _errors.SetError(txtName, "Name is required"); ok = false; }

            if (numPrice.Value < 0)
            { _errors.SetError(numPrice, "Price must be >= 0"); ok = false; }

            if (numStock.Value < 0)
            { _errors.SetError(numStock, "Stock must be >= 0"); ok = false; }

            return ok;
        }

        private void Save()
        {
            if (!ValidateInputs()) return;

            _product.Name = txtName.Text.Trim();
            _product.Description = txtDescription.Text.Trim();
            _product.Price = numPrice.Value;
            _product.Stock = (int)numStock.Value;
            _product.Category = cboCategory.Text;

            ErrorsInfo res;
            if (_isEditMode)
                res = _vm.UpdateProduct(_product);
            else
                res = _vm.AddProduct(_product);

            if (res.Flag == Errors.Ok)
            {
                MessageBox.Show("Product saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ProductSaved?.Invoke(this, _product);
                FindForm()?.Close();
            }
            else
            {
                MessageBox.Show(res.Message ?? "Save failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnNavigatedTo(Dictionary<string, object> parameters)
        {
            base.OnNavigatedTo(parameters);

            _isEditMode = false;
            _product = new Models.Product();

            if (parameters != null && parameters.TryGetValue("ProductId", out var idObj) && idObj is int id && id > 0)
            {
                var p = _vm.GetById(id);
                if (p != null)
                {
                    _product = p;
                    _isEditMode = true;

                    txtName.Text = p.Name;
                    txtDescription.Text = p.Description;
                    numPrice.Value = p.Price;
                    numStock.Value = p.Stock;
                    cboCategory.Text = p.Category ?? "General";
                }
            }
        }

        public override void Configure(Dictionary<string, object> settings)
        {
            base.Configure(settings);
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
            Name = nameof(ProductEditView);
            Size = new Size(720, 480);
            ResumeLayout(false);
        }
    }
}
