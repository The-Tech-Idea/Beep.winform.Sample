using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.ConfigUtil;
using WinFormsApp.UI.Test.SampleBusinessApp.Models;
using WinFormsApp.UI.Test.SampleBusinessApp.Services;
using TheTechIdea.Beep.Winform.Controls.Models;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Winform.Controls.TextFields;
 

namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Customer
{
    /// <summary>
    /// Customer List Management View - Enterprise CRUD operations
    /// Inherits from TemplateUserControl following Beep framework patterns
    /// </summary>
    [AddinAttribute(Caption = "Customer List", Name = "CustomerListView", misc = "SampleBusinessApp", menu = "Customer",
        addinType = AddinType.Control, displayType = DisplayType.InControl, ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 200, RootNodeName = "Sample Business App", Order = 200, ID = 200,
        BranchText = "Customer List", BranchType = EnumPointType.Function,
        IconImageName = "customers.svg", BranchClass = "ADDIN",
        BranchDescription = "Customer List Management and CRUD Operations")]
    public partial class CustomerListView : TemplateUserControl, IAddinVisSchema
    {
        #region Fields
        private readonly CustomerService _customerService;
        private readonly IServiceProvider _services;
        
        // Layout controls
        private BeepMultiSplitter _mainLayout;
        private BeepPanel _headerPanel;
        private BeepPanel _toolbarPanel;
        private BeepPanel _gridPanel;
        private BeepPanel _detailPanel;

        // Toolbar controls
        private BeepButton _addButton;
        private BeepButton _editButton;
        private BeepButton _deleteButton;
        private BeepButton _refreshButton;
        private BeepButton _viewProfileButton;
        private BeepTextBox _searchTextBox;
        private BeepComboBox _statusFilterCombo;
        private BeepComboBox _typeFilterCombo;

        // Data grid
        private BeepSimpleGrid _customersGrid;

        // Detail view controls
        private BeepLabel _customerNameLabel;
        private BeepLabel _customerEmailLabel;
        private BeepLabel _customerPhoneLabel;
        private BeepLabel _customerAddressLabel;
        private BeepLabel _customerStatusLabel;
        private BeepLabel _lastContactLabel;

        // Data
        private List<Models.Customer> _customers;
        private Models.Customer _selectedCustomer;
        #endregion

        #region IAddinVisSchema Properties
        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 200;
        public int ID { get; set; } = 200;
        public string BranchText { get; set; } = "Customer List";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 200;
        public string IconImageName { get; set; } = "customers.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Customer List Management and CRUD Operations";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }
        #endregion

        #region Constructor
        public CustomerListView(IServiceProvider services) : base(services)
        {
            _services = services;
            _customerService = services.GetRequiredService<CustomerService>();
            InitializeComponent();
            InitializeCustomerList();
        }
        #endregion

        #region Initialization
        private void InitializeCustomerList()
        {
            CreateLayout();
            CreateToolbar();
            CreateDataGrid();
            CreateDetailPanel();
            WireEvents();
            LoadCustomers();
        }

        private void CreateLayout()
        {
            // Main layout: header, toolbar, grid + detail
            _mainLayout = new BeepMultiSplitter
            {
                Dock = DockStyle.Fill,
                Theme = this.Theme
            };
            _mainLayout.tableLayoutPanel.RowCount = 3;
            _mainLayout.tableLayoutPanel.ColumnCount = 2;

            // Row styles: header, toolbar, content
            _mainLayout.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            _mainLayout.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            _mainLayout.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Column styles: main content, detail panel
            _mainLayout.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            _mainLayout.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            // Header panel
            _headerPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = false,
                BackColor = Color.White,
                IsRounded = true,
                BorderRadius = 8,
                ShowShadow = true,
                Padding = new Padding(20),
                Theme = this.Theme
            };

            var titleLabel = new BeepLabel
            {
                Text = "👥 Customer Management",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51),
                Location = new Point(20, 20),
                AutoSize = true,
                Theme = this.Theme
            };

            var subtitleLabel = new BeepLabel
            {
                Text = "Manage customer information, contacts, and relationships",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(102, 102, 102),
                Location = new Point(20, 45),
                AutoSize = true,
                Theme = this.Theme
            };

            _headerPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel });

            this.Controls.Add(_mainLayout);
            _mainLayout.tableLayoutPanel.Controls.Add(_headerPanel, 0, 0);
            _mainLayout.tableLayoutPanel.SetColumnSpan(_headerPanel, 2);
        }

        private void CreateToolbar()
        {
            _toolbarPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = false,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(15),
                Theme = this.Theme
            };

            // Action buttons
            _addButton = new BeepButton
            {
                Text = "➕ Add Customer",
                Location = new Point(15, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                IsRounded = true,
                BorderRadius = 6,
                Theme = this.Theme
            };

            _editButton = new BeepButton
            {
                Text = "✏️ Edit",
                Location = new Point(145, 20),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                IsRounded = true,
                BorderRadius = 6,
                Enabled = false,
                Theme = this.Theme
            };

            _deleteButton = new BeepButton
            {
                Text = "🗑️ Delete",
                Location = new Point(235, 20),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                IsRounded = true,
                BorderRadius = 6,
                Enabled = false,
                Theme = this.Theme
            };

            _refreshButton = new BeepButton
            {
                Text = "🔄 Refresh",
                Location = new Point(325, 20),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                IsRounded = true,
                BorderRadius = 6,
                Theme = this.Theme
            };

            _viewProfileButton = new BeepButton
            {
                Text = "👤 View Profile",
                Location = new Point(425, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(102, 16, 242),
                ForeColor = Color.White,
                IsRounded = true,
                BorderRadius = 6,
                Enabled = false,
                Theme = this.Theme
            };

            // Search and filter controls
            var searchLabel = new BeepLabel
            {
                Text = "Search:",
                Location = new Point(550, 25),
                AutoSize = true,
                Theme = this.Theme
            };

            _searchTextBox = new BeepTextBox
            {
                Location = new Point(600, 22),
                Size = new Size(200, 30),
                PlaceholderText = "Search customers...",
                Theme = this.Theme
            };

            var statusLabel = new BeepLabel
            {
                Text = "Status:",
                Location = new Point(820, 25),
                AutoSize = true,
                Theme = this.Theme
            };

            _statusFilterCombo = new BeepComboBox
            {
                Location = new Point(870, 22),
                Size = new Size(100, 30),
                Theme = this.Theme
            };
            _statusFilterCombo.Items.AddRange(new[] { "All", "Active", "Inactive", "Suspended" });
            _statusFilterCombo.SelectedIndex = 0;

            var typeLabel = new BeepLabel
            {
                Text = "Type:",
                Location = new Point(990, 25),
                AutoSize = true,
                Theme = this.Theme
            };

            _typeFilterCombo = new BeepComboBox
            {
                Location = new Point(1030, 22),
                Size = new Size(100, 30),
                Theme = this.Theme
            };
            _typeFilterCombo.Items.AddRange(new[] { "All", "Standard", "Premium", "VIP" });
            _typeFilterCombo.SelectedIndex = 0;

            _toolbarPanel.Controls.AddRange(new Control[]
            {
                _addButton, _editButton, _deleteButton, _refreshButton, _viewProfileButton,
                searchLabel, _searchTextBox, statusLabel, _statusFilterCombo,
                typeLabel, _typeFilterCombo
            });

            _mainLayout.tableLayoutPanel.Controls.Add(_toolbarPanel, 0, 1);
            _mainLayout.tableLayoutPanel.SetColumnSpan(_toolbarPanel, 2);
        }

        private void CreateDataGrid()
        {
            _gridPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = true,
                TitleText = "📋 Customer List",
                BackColor = Color.White,
                IsRounded = true,
                BorderRadius = 8,
                ShowShadow = true,
                Padding = new Padding(15),
                Theme = this.Theme
            };

            _customersGrid = new BeepSimpleGrid
            {
                Dock = DockStyle.Fill,
                ShowCheckboxes = false,
                Theme = this.Theme
            };

            _gridPanel.Controls.Add(_customersGrid);
            _mainLayout.tableLayoutPanel.Controls.Add(_gridPanel, 0, 2);
        }

        private void CreateDetailPanel()
        {
            _detailPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = true,
                TitleText = "👤 Customer Details",
                BackColor = Color.White,
                IsRounded = true,
                BorderRadius = 8,
                ShowShadow = true,
                Padding = new Padding(15),
                Theme = this.Theme
            };

            // Customer detail labels
            _customerNameLabel = new BeepLabel
            {
                Text = "Name: ",
                Location = new Point(15, 30),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Theme = this.Theme
            };

            _customerEmailLabel = new BeepLabel
            {
                Text = "Email: ",
                Location = new Point(15, 60),
                Size = new Size(250, 25),
                Theme = this.Theme
            };

            _customerPhoneLabel = new BeepLabel
            {
                Text = "Phone: ",
                Location = new Point(15, 90),
                Size = new Size(250, 25),
                Theme = this.Theme
            };

            _customerAddressLabel = new BeepLabel
            {
                Text = "Address: ",
                Location = new Point(15, 120),
                Size = new Size(250, 60),
                Theme = this.Theme
            };

            _customerStatusLabel = new BeepLabel
            {
                Text = "Status: ",
                Location = new Point(15, 190),
                Size = new Size(250, 25),
                Theme = this.Theme
            };

            _lastContactLabel = new BeepLabel
            {
                Text = "Last Contact: ",
                Location = new Point(15, 220),
                Size = new Size(250, 25),
                Theme = this.Theme
            };

            _detailPanel.Controls.AddRange(new Control[]
            {
                _customerNameLabel, _customerEmailLabel, _customerPhoneLabel,
                _customerAddressLabel, _customerStatusLabel, _lastContactLabel
            });

            _mainLayout.tableLayoutPanel.Controls.Add(_detailPanel, 1, 2);
        }
        #endregion

        #region Data Operations
        private void LoadCustomers()
        {
            try
            {
                _customers = _customerService.GetAllCustomers();
                ApplyFilters();
                UpdateUI();
            }
            catch (Exception ex)
            {
                ShowError($"Error loading customers: {ex.Message}");
                Editor?.AddLogMessage("CustomerListView", $"Error loading customers: {ex.Message}",
                    DateTime.Now, -1, null, Errors.Failed);
            }
        }

        private void ApplyFilters()
        {
            if (_customers == null) return;

            var filteredCustomers = _customers.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(_searchTextBox?.Text))
            {
                var searchText = _searchTextBox.Text.ToLower();
                filteredCustomers = filteredCustomers.Where(c =>
                    c.Name.ToLower().Contains(searchText) ||
                    c.Email.ToLower().Contains(searchText) ||
                    c.CompanyName.ToLower().Contains(searchText));
            }

            // Apply status filter
            if (_statusFilterCombo?.SelectedItem?.ToString() != "All")
            {
                if (_statusFilterCombo?.SelectedItem is null) return;
                var status = _statusFilterCombo.SelectedItem.ToString();
                filteredCustomers = filteredCustomers.Where(c => c.Status == status);
            }

            // Apply type filter
            if (_typeFilterCombo?.SelectedItem?.ToString() != "All")
            {
                if (_typeFilterCombo?.SelectedItem is null) return;
                var type = _typeFilterCombo.SelectedItem.ToString();
                filteredCustomers = filteredCustomers.Where(c => c.CustomerType == type);
            }

            _customersGrid.DataSource = filteredCustomers.ToList();
        }

        private void UpdateCustomerDetails(Models.Customer customer)
        {
            if (customer == null)
            {
                ClearCustomerDetails();
                return;
            }

            _selectedCustomer = customer;

            _customerNameLabel.Text = $"Name: {customer.DisplayName}";
            _customerEmailLabel.Text = $"Email: {customer.Email}";
            _customerPhoneLabel.Text = $"Phone: {customer.Phone}";
            _customerAddressLabel.Text = $"Address: {customer.FullAddress}";
            _customerStatusLabel.Text = $"Status: {customer.Status} ({customer.CustomerType})";
            _lastContactLabel.Text = $"Last Contact: {customer.LastContactDate?.ToString("MMM dd, yyyy") ?? "Never"}";

            // Enable action buttons
            _editButton.Enabled = true;
            _deleteButton.Enabled = true;
            _viewProfileButton.Enabled = true;
        }

        private void ClearCustomerDetails()
        {
            _selectedCustomer = null;

            _customerNameLabel.Text = "Name: ";
            _customerEmailLabel.Text = "Email: ";
            _customerPhoneLabel.Text = "Phone: ";
            _customerAddressLabel.Text = "Address: ";
            _customerStatusLabel.Text = "Status: ";
            _lastContactLabel.Text = "Last Contact: ";

            // Disable action buttons
            _editButton.Enabled = false;
            _deleteButton.Enabled = false;
            _viewProfileButton.Enabled = false;
        }

        private void UpdateUI()
        {
            // Update grid and clear selection
            ClearCustomerDetails();

            // Update status display
            var totalCount = _customers?.Count ?? 0;
            var filteredCount = (_customersGrid.DataSource as List<Models.Customer>)?.Count ?? 0;
            
            if (totalCount != filteredCount)
            {
                _gridPanel.TitleText = $"📋 Customer List ({filteredCount} of {totalCount})";
            }
            else
            {
                _gridPanel.TitleText = $"📋 Customer List ({totalCount})";
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Event Handlers
        private void WireEvents()
        {
            _addButton.Click += AddButton_Click;
            _editButton.Click += EditButton_Click;
            _deleteButton.Click += DeleteButton_Click;
            _refreshButton.Click += RefreshButton_Click;
            _viewProfileButton.Click += ViewProfileButton_Click;

            _searchTextBox.TextChanged += SearchTextBox_TextChanged;
            _statusFilterCombo.SelectedItemChanged += FilterCombo_SelectedItemChanged;
            _typeFilterCombo.SelectedItemChanged += FilterCombo_SelectedItemChanged;

            _customersGrid.SelectedRowsChanged += CustomersGrid_SelectedRowsChanged;
            _customersGrid.DoubleClick += CustomersGrid_DoubleClick;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var editor = new Views.Customer.CustomerEditView(_services);
            editor.CustomerSaved += Editor_CustomerSaved;
            ShowAsDialog(editor);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (_selectedCustomer != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "CustomerId", _selectedCustomer.Id }
                };
                var editor = new Views.Customer.CustomerEditView(_services);
                editor.CustomerSaved += Editor_CustomerSaved;
                editor.OnNavigatedTo(parameters);
                ShowAsDialog(editor);
            }
        }

        private void Editor_CustomerSaved(object? sender, Models.Customer e)
        {
            LoadCustomers();
        }

        private void ShowAsDialog(Control control)
        {
            using var popupForm = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowIcon = false,
                ShowInTaskbar = false,
                Text = "Customer Editor"
            };
            control.Dock = DockStyle.Fill;
            popupForm.Controls.Add(control);
            popupForm.Padding = new Padding(8);
            popupForm.ShowDialog(FindForm());
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_selectedCustomer == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete customer '{_selectedCustomer.Name}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var deleteResult = _customerService.DeleteCustomer(_selectedCustomer.Id);
                    if (deleteResult.Flag == Errors.Ok)
                    {
                        ShowSuccess("Customer deleted successfully");
                        LoadCustomers();
                    }
                    else
                    {
                        ShowError($"Error deleting customer: {deleteResult.Message}");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error deleting customer: {ex.Message}");
                }
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void ViewProfileButton_Click(object sender, EventArgs e)
        {
            if (_selectedCustomer == null) return;
            var profile = new Views.Customer.CustomerProfileView(_services);
            profile.OnNavigatedTo(new Dictionary<string, object> { ["CustomerId"] = _selectedCustomer.Id });
            ShowAsDialog(profile);
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
            UpdateUI();
        }

        private void FilterCombo_SelectedItemChanged(object? sender, SelectedItemChangedEventArgs e)
        {
            ApplyFilters();
            UpdateUI();
        }

        private void CustomersGrid_SelectedRowsChanged(object sender, EventArgs e)
        {
            if (_customersGrid.SelectedRows.Count > 0)
            {
                var dataSource = _customersGrid.DataSource;
                if (dataSource is List<Models.Customer> customers)
                {
                    var selectedRowIndex = _customersGrid.SelectedRows[0];
                    if (selectedRowIndex >= 0 && selectedRowIndex < customers.Count)
                    {
                        var selectedCustomer = customers[selectedRowIndex];
                        UpdateCustomerDetails(selectedCustomer);
                    }
                }
            }
            else
            {
                ClearCustomerDetails();
            }
        }

        private void CustomersGrid_DoubleClick(object? sender, EventArgs e)
        {
            if (_selectedCustomer == null) return;
            var profile = new Views.Customer.CustomerProfileView(_services);
            profile.OnNavigatedTo(new Dictionary<string, object> { ["CustomerId"] = _selectedCustomer.Id });
            ShowAsDialog(profile);
        }
        #endregion

        #region Override Methods
        public override void Configure(Dictionary<string, object> settings)
        {
            base.Configure(settings);
            // Apply any specific settings
        }

        public override void OnNavigatedTo(Dictionary<string, object> parameters)
        {
            base.OnNavigatedTo(parameters);
            
            // Refresh data when navigated to
            LoadCustomers();

            // Handle any parameters
            if (parameters?.ContainsKey("RefreshData") == true && (bool)parameters["RefreshData"])
            {
                RefreshButton_Click(null, null);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            // Any additional initialization
        }
        #endregion

        #region Designer Support
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomerListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CustomerListView";
            this.Size = new System.Drawing.Size(1200, 800);
            this.ResumeLayout(false);
        }
        #endregion
    }
}