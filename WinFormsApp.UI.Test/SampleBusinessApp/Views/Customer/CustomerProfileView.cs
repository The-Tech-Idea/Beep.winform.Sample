using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
 
using TheTechIdea.Beep.Winform.Default.Views.Template;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;
using WinFormsApp.UI.Test.SampleBusinessApp.Services;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Customer
{
    [AddinAttribute(Caption = "Customer Profile", Name = "CustomerProfileView", misc = "SampleBusinessApp", menu = "Customer",
        addinType = AddinType.Control, displayType = DisplayType.InControl, ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 202, RootNodeName = "Sample Business App", Order = 202, ID = 202,
        BranchText = "Customer Profile", BranchType = EnumPointType.Function,
        IconImageName = "user-profile.svg", BranchClass = "ADDIN",
        BranchDescription = "Detailed customer information and activity")]
    public class CustomerProfileView : TemplateUserControl, IAddinVisSchema
    {
        private readonly CustomerService _service;
        private readonly IServiceProvider _services;

        // Layout
        private BeepMultiSplitter _layout;
        private BeepPanel _header;
        private BeepPanel _infoCard;
        private BeepPanel _statsCard;
        private BeepPanel _notesCard;

        // Header
        private BeepLabel _title;
        private BeepButton _editBtn;
        private BeepButton _closeBtn;

        // Info fields
        private BeepLabel _name;
        private BeepLabel _email;
        private BeepLabel _phone;
        private BeepLabel _address;
        private BeepLabel _status;
        private BeepLabel _type;
        private BeepLabel _company;
        private BeepLabel _contact;
        private BeepLabel _credit;
        private BeepLabel _terms;
        private BeepLabel _created;
        private BeepLabel _updated;
        private BeepLabel _lastContact;

        private Models.Customer _customer;

        #region IAddinVisSchema
        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 202;
        public int ID { get; set; } = 202;
        public string BranchText { get; set; } = "Customer Profile";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 202;
        public string IconImageName { get; set; } = "user-profile.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Detailed customer information and activity";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }
        #endregion

        public CustomerProfileView(IServiceProvider services) : base(services)
        {
            _services = services;
            _service = services.GetRequiredService<CustomerService>();
            InitializeComponent();
            BuildUI();
            WireEvents();
        }

        private void BuildUI()
        {
            _layout = new BeepMultiSplitter { Dock = DockStyle.Fill, Theme = this.Theme };
            _layout.TableLayoutPanel.RowCount = 2;
            _layout.TableLayoutPanel.ColumnCount = 1;
            _layout.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            _layout.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Header
            _header = new BeepPanel { Dock = DockStyle.Fill, ShowTitle = false, Padding = new Padding(12), Theme = this.Theme };
            _title = new BeepLabel { Text = "👤 Customer Profile", Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Theme = this.Theme };
            _editBtn = new BeepButton { Text = "✏️ Edit", Size = new Size(80, 30), Location = new Point(680, 14), BackColor = Color.FromArgb(0,123,255), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _closeBtn = new BeepButton { Text = "✖ Close", Size = new Size(90, 30), Location = new Point(770, 14), BackColor = Color.FromArgb(108,117,125), ForeColor = Color.White, IsRounded = true, BorderRadius = 6, Theme = this.Theme };
            _header.Controls.AddRange(new Control[] { _title, _editBtn, _closeBtn });

            // Content split
            var content = new BeepMultiSplitter { Dock = DockStyle.Fill, Theme = this.Theme };
            content.TableLayoutPanel.RowCount = 1;
            content.TableLayoutPanel.ColumnCount = 2;
            content.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            content.TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            content.TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            // Info Card (left)
            _infoCard = new BeepPanel { Dock = DockStyle.Fill, ShowTitle = true, TitleText = "📇 Information", IsRounded = true, BorderRadius = 8, ShowShadow = true, Padding = new Padding(12), Theme = this.Theme };
            int y = 24; int dy = 26; int x1 = 12;
            _name = NewInfo(_infoCard, "Name:", x1, y); y += dy;
            _email = NewInfo(_infoCard, "Email:", x1, y); y += dy;
            _phone = NewInfo(_infoCard, "Phone:", x1, y); y += dy;
            _address = NewInfo(_infoCard, "Address:", x1, y); y += dy;
            _company = NewInfo(_infoCard, "Company:", x1, y); y += dy;
            _contact = NewInfo(_infoCard, "Contact:", x1, y); y += dy;
            _credit = NewInfo(_infoCard, "Credit Limit:", x1, y); y += dy;
            _terms = NewInfo(_infoCard, "Terms:", x1, y); y += dy;
            _status = NewInfo(_infoCard, "Status:", x1, y); y += dy;
            _type = NewInfo(_infoCard, "Type:", x1, y); y += dy;
            _created = NewInfo(_infoCard, "Created:", x1, y); y += dy;
            _updated = NewInfo(_infoCard, "Updated:", x1, y); y += dy;
            _lastContact = NewInfo(_infoCard, "Last Contact:", x1, y);

            // Stats/Notes (right)
            _statsCard = new BeepPanel { Dock = DockStyle.Fill, ShowTitle = true, TitleText = "📊 Summary", IsRounded = true, BorderRadius = 8, ShowShadow = true, Padding = new Padding(12), Theme = this.Theme };
            _notesCard = new BeepPanel { Dock = DockStyle.Fill, ShowTitle = true, TitleText = "📝 Notes", IsRounded = true, BorderRadius = 8, ShowShadow = true, Padding = new Padding(12), Theme = this.Theme };

            var rightStack = new BeepMultiSplitter { Dock = DockStyle.Fill, Theme = this.Theme };
            rightStack.TableLayoutPanel.RowCount = 2;
            rightStack.TableLayoutPanel.ColumnCount = 1;
            rightStack.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            rightStack.TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            rightStack.TableLayoutPanel.Controls.Add(_statsCard, 0, 0);
            rightStack.TableLayoutPanel.Controls.Add(_notesCard, 0, 1);

            Controls.Add(_layout);
            _layout.TableLayoutPanel.Controls.Add(_header, 0, 0);
            _layout.TableLayoutPanel.Controls.Add(content, 0, 1);

            content.TableLayoutPanel.Controls.Add(_infoCard, 0, 0);
            content.TableLayoutPanel.Controls.Add(rightStack, 1, 0);
        }

        private BeepLabel NewInfo(BeepPanel parent, string label, int x, int y)
        {
            var lbl = new BeepLabel { Text = label, Location = new Point(x, y), AutoSize = true, Theme = this.Theme };
            parent.Controls.Add(lbl);
            return lbl;
        }

        private void WireEvents()
        {
            _editBtn.Click += (s, e) =>
            {
                if (_customer == null) return;
                var editor = new Customer.CustomerEditView(_services);
                editor.OnNavigatedTo(new Dictionary<string, object> { ["CustomerId"] = _customer.Id });
                ShowDialog(editor, "Edit Customer");
            };
            _closeBtn.Click += (s, e) => FindForm()?.Close();
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

        public override void OnNavigatedTo(Dictionary<string, object> parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.TryGetValue("CustomerId", out var idObj) && idObj is int id && id > 0)
                {
                    var c = _service.GetCustomerById(id);
                    if (c != null)
                    {
                        _customer = c;
                        Populate(c);
                    }
                }
                else if (parameters.TryGetValue("Customer", out var obj) && obj is Models.Customer cust)
                {
                    _customer = cust;
                    Populate(cust);
                }
            }
        }

        private void Populate(Models.Customer c)
        {
            _title.Text = $"👤 {c.DisplayName}";
            _name.Text = $"Name: {c.Name}";
            _email.Text = $"Email: {c.Email}";
            _phone.Text = $"Phone: {c.Phone}";
            _address.Text = $"Address: {c.FullAddress}";
            _company.Text = $"Company: {c.CompanyName}";
            _contact.Text = $"Contact: {c.ContactPerson}";
            _credit.Text = $"Credit Limit: {c.CreditLimit:C}";
            _terms.Text = $"Terms: {c.PaymentTerms}";
            _status.Text = $"Status: {c.Status}";
            _type.Text = $"Type: {c.CustomerType}";
            _created.Text = $"Created: {c.CreatedAt:MMM dd, yyyy}";
            _updated.Text = $"Updated: {(c.UpdatedAt.HasValue ? c.UpdatedAt.Value.ToString("MMM dd, yyyy") : "-")}";
            _lastContact.Text = $"Last Contact: {(c.LastContactDate.HasValue ? c.LastContactDate.Value.ToString("MMM dd, yyyy") : "Never")}";
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
            Name = nameof(CustomerProfileView);
            Size = new Size(900, 600);
            ResumeLayout(false);
        }
    }
}
