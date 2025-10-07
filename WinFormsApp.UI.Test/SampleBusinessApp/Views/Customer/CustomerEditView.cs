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
using Microsoft.Extensions.DependencyInjection.Extensions;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;
using WinFormsApp.UI.Test.SampleBusinessApp.Services;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Winform.Controls.TextFields;
using TheTechIdea.Beep.Winform.Controls.Numerics;
using TheTechIdea.Beep.Winform.Controls.ComboBoxes;


namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Customer
{
    [AddinAttribute(Caption = "Customer Editor", Name = "CustomerEditView", misc = "SampleBusinessApp", menu = "Customer",
        addinType = AddinType.Control, displayType = DisplayType.InControl, ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 201, RootNodeName = "Sample Business App", Order = 201, ID = 201,
        BranchText = "Customer Editor", BranchType = EnumPointType.Function,
        IconImageName = "user-edit.svg", BranchClass = "ADDIN",
        BranchDescription = "Create or edit customer information")]
    public partial class CustomerEditView : TemplateUserControl, IAddinVisSchema
    {
        private readonly CustomerService _customerService;

        // UI
        private BeepPanel _headerPanel;
        private BeepPanel _formPanel;
        private BeepPanel _footerPanel;
        private BeepLabel _titleLabel;

        private BeepTextBox txtName;
        private BeepTextBox txtEmail;
        private BeepTextBox txtPhone;
        private BeepTextBox txtAddress;
        private BeepTextBox txtCity;
        private BeepTextBox txtPostalCode;
        private BeepTextBox txtCountry;
        private BeepTextBox txtCompanyName;
        private BeepTextBox txtContactPerson;
        private BeepTextBox txtTaxId;
        private BeepNumericUpDown numCreditLimit;
        private BeepComboBox cboPaymentTerms;
        private BeepComboBox cboStatus;
        private BeepComboBox cboCustomerType;

        private BeepButton btnSave;
        private BeepButton btnCancel;

        // Data
        private Models.Customer _customer = new Models.Customer();
        private bool _isEditMode = false;

        // Notify parent views when a customer is saved
        public event EventHandler<Models.Customer> CustomerSaved;

        private ErrorProvider _errors;

        #region IAddinVisSchema
        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 201;
        public int ID { get; set; } = 201;
        public string BranchText { get; set; } = "Customer Editor";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 201;
        public string IconImageName { get; set; } = "user-edit.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Create or edit customer information";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }
        #endregion

        public CustomerEditView(IServiceProvider services) : base(services)
        {
            _customerService = services.GetRequiredService<CustomerService>();
            InitializeComponent();
            BuildUI();
            WireEvents();
        }

        private void BuildUI()
        {
            SuspendLayout();

            _headerPanel = new BeepPanel
            {
                Dock = DockStyle.Top,
                Height = 64,
                ShowTitle = false,
                BackColor = Color.White,
                Padding = new Padding(16),
                Theme = this.Theme
            };
            _titleLabel = new BeepLabel
            {
                Text = "?? Customer Editor",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Theme = this.Theme
            };
            _headerPanel.Controls.Add(_titleLabel);

            _formPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = false,
                BackColor = Color.White,
                Padding = new Padding(16),
                Theme = this.Theme
            };

            // Create fields
            int x1 = 16, x2 = 400, y = 16, dy = 40, w = 320;
            txtName = NewTextBox("Name", x1, y, w); y += dy;
            txtEmail = NewTextBox("Email", x1, y, w); y += dy;
            txtPhone = NewTextBox("Phone", x1, y, w); y += dy;
            txtAddress = NewTextBox("Address", x1, y, w); y += dy;
            txtCity = NewTextBox("City", x1, y, w); y += dy;
            txtPostalCode = NewTextBox("Postal Code", x1, y, w); y += dy;
            txtCountry = NewTextBox("Country", x1, y, w);

            // Right column
            y = 16;
            txtCompanyName = NewTextBox("Company Name", x2, y, w); y += dy;
            txtContactPerson = NewTextBox("Contact Person", x2, y, w); y += dy;
            txtTaxId = NewTextBox("Tax ID", x2, y, w); y += dy;

            numCreditLimit = new BeepNumericUpDown
            {
                Location = new Point(x2, y),
                Size = new Size(w, 30),
                Value = 0,
                MinimumValue = 0,
                MaximumValue = 1000000,
                IncrementValue = 100,
                Theme = this.Theme
            };
            _formPanel.Controls.Add(Labeled("Credit Limit", x2, y - 18));
            _formPanel.Controls.Add(numCreditLimit);
            y += dy;

            cboPaymentTerms = NewComboBox("Payment Terms", x2, y, w, new[] { "Net 15", "Net 30", "Net 45", "Net 60" }); y += dy;
            cboStatus = NewComboBox("Status", x2, y, w, new[] { "Active", "Inactive", "Suspended" }); y += dy;
            cboCustomerType = NewComboBox("Customer Type", x2, y, w, new[] { "Standard", "Premium", "VIP" });

            _footerPanel = new BeepPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                ShowTitle = false,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(16),
                Theme = this.Theme
            };
            btnSave = new BeepButton
            {
                Text = "?? Save",
                Size = new Size(100, 34),
                Location = new Point(Width - 240, 12),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                IsRounded = true,
                BorderRadius = 6,
                Theme = this.Theme
            };
            btnCancel = new BeepButton
            {
                Text = "? Cancel",
                Size = new Size(100, 34),
                Location = new Point(Width - 128, 12),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                IsRounded = true,
                BorderRadius = 6,
                Theme = this.Theme
            };
            _footerPanel.Controls.Add(btnSave);
            _footerPanel.Controls.Add(btnCancel);

            Controls.Add(_formPanel);
            Controls.Add(_footerPanel);
            Controls.Add(_headerPanel);

            ResumeLayout(false);
        }

        private BeepLabel Labeled(string text, int x, int y)
            => new BeepLabel { Text = text, Location = new Point(x, y), AutoSize = true, Theme = this.Theme };

        private BeepTextBox NewTextBox(string label, int x, int y, int w)
        {
            var lbl = Labeled(label, x, y - 18);
            var tb = new BeepTextBox { Location = new Point(x, y), Size = new Size(w, 30), Theme = this.Theme };
            _formPanel.Controls.Add(lbl);
            _formPanel.Controls.Add(tb);
            return tb;
        }

        private BeepComboBox NewComboBox(string label, int x, int y, int w, string[] options)
        {
            var lbl = Labeled(label, x, y - 18);
            var cb = new BeepComboBox { Location = new Point(x, y), Size = new Size(w, 30), Theme = this.Theme };
            foreach (var opt in options)
                cb.Items.Add(new SimpleItem { Text = opt, Value = opt, Name = opt });
            cb.SelectedIndex = 0;
            _formPanel.Controls.Add(lbl);
            _formPanel.Controls.Add(cb);
            return cb;
        }

        private void WireEvents()
        {
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => FindForm()?.Close();

            // Inline validation providers
            _errors = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Name is required"); return; }
            if (string.IsNullOrWhiteSpace(txtEmail.Text)) { MessageBox.Show("Email is required"); return; }
            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) { MessageBox.Show("Email is not valid"); return; }
            if (string.IsNullOrWhiteSpace(txtPhone.Text)) { MessageBox.Show("Phone is required"); return; }
            if (!Regex.IsMatch(txtPhone.Text, @"^\+?\d{10,15}$")) { MessageBox.Show("Phone number is not valid"); return; }

            // Map UI to model
            _customer.Name = txtName.Text.Trim();
            _customer.Email = txtEmail.Text.Trim();
            _customer.Phone = txtPhone.Text.Trim();
            _customer.Address = txtAddress.Text.Trim();
            _customer.City = txtCity.Text.Trim();
            _customer.PostalCode = txtPostalCode.Text.Trim();
            _customer.Country = txtCountry.Text.Trim();
            _customer.CompanyName = txtCompanyName.Text.Trim();
            _customer.ContactPerson = txtContactPerson.Text.Trim();
            _customer.TaxId = txtTaxId.Text.Trim();
            _customer.CreditLimit = numCreditLimit.Value;
            _customer.PaymentTerms = cboPaymentTerms.Text;
            _customer.Status = cboStatus.Text;
            _customer.CustomerType = cboCustomerType.Text;

            ErrorsInfo result;
            if (_isEditMode)
                result = _customerService.UpdateCustomer(_customer);
            else
                result = _customerService.CreateCustomer(_customer);

            if (result.Flag == Errors.Ok)
            {
                MessageBox.Show("Customer saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CustomerSaved?.Invoke(this, _customer);
                FindForm()?.Close();
            }
            else
            {
                MessageBox.Show(result.Message ?? "Failed to save customer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerToUI(Models.Customer c)
        {
            txtName.Text = c.Name;
            txtEmail.Text = c.Email;
            txtPhone.Text = c.Phone;
            txtAddress.Text = c.Address;
            txtCity.Text = c.City;
            txtPostalCode.Text = c.PostalCode;
            txtCountry.Text = c.Country;
            txtCompanyName.Text = c.CompanyName;
            txtContactPerson.Text = c.ContactPerson;
            txtTaxId.Text = c.TaxId;
            numCreditLimit.Value = c.CreditLimit;
            cboPaymentTerms.Text = c.PaymentTerms;
            cboStatus.Text = c.Status;
            cboCustomerType.Text = c.CustomerType;
        }

        public override void Configure(Dictionary<string, object> settings)
        {
            base.Configure(settings);
        }

        public override void OnNavigatedTo(Dictionary<string, object> parameters)
        {
            base.OnNavigatedTo(parameters);

            _isEditMode = false;
            _customer = new Models.Customer();
            _titleLabel.Text = "?? New Customer";

            if (parameters != null)
            {
                if (parameters.TryGetValue("Customer", out var obj) && obj is Models.Customer ec)
                {
                    _customer = ec;
                    _isEditMode = true;
                    _titleLabel.Text = "?? Edit Customer";
                    LoadCustomerToUI(_customer);
                }
                else if (parameters.TryGetValue("CustomerId", out var idObj) && idObj is int id && id > 0)
                {
                    var found = _customerService.GetCustomerById(id);
                    if (found != null)
                    {
                        _customer = found;
                        _isEditMode = true;
                        _titleLabel.Text = "?? Edit Customer";
                        LoadCustomerToUI(_customer);
                    }
                }
            }
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
            Name = nameof(CustomerEditView);
            Size = new Size(900, 520);
            ResumeLayout(false);
        }

        private bool ValidateInputs()
        {
            _errors.Clear();
            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            { _errors.SetError(txtName, "Name is required"); ok = false; }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            { _errors.SetError(txtEmail, "Email is required"); ok = false; }
            else if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            { _errors.SetError(txtEmail, "Invalid email format"); ok = false; }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            { _errors.SetError(txtPhone, "Phone is required"); ok = false; }
            else if (!Regex.IsMatch(txtPhone.Text, @"^\+?\d{7,15}$"))
            { _errors.SetError(txtPhone, "Invalid phone format"); ok = false; }

            return ok;
        }
    }
}
