using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.AppBars;
using TheTechIdea.Beep.Winform.Controls.Base;
using TheTechIdea.Beep.Winform.Controls.Forms;
using TheTechIdea.Beep.Winform.Controls.Forms.ModernForm;
using TheTechIdea.Beep.Winform.Controls.Managers;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using WinFormsApp.UI.Test.SampleBusinessApp.Services;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Forms
{
    /// <summary>
    /// Main business application form inheriting from BeepiForm
    /// Provides enterprise navigation and layout management
    /// </summary>
    public partial class MainBusinessForm : BeepiFormPro
    {
        #region Fields
        private readonly IServiceProvider _services;
        private readonly IBeepService _beepService;
        private readonly AuthService _authService;
        
        // UI Manager and Navigation
        private BeepFormUIManager _uiManager;
        private BeepWebHeaderAppBar _menuBar;
        private BeepSideMenu _sideMenu;
        private BeepPanel _contentPanel;
        private BeepPanel _statusPanel;
        private BeepLabel _statusLabel;
        private BeepLabel _userLabel;

        // Current view tracking
        private TemplateUserControl _currentView;
        private TheTechIdea.Beep.Winform.Controls.SideBar.BeepSideBar beepSideBar1;
        private Dictionary<string, TemplateUserControl> _viewCache;
        #endregion

        #region Constructor
        public MainBusinessForm(IServiceProvider services)
        {
            _services = services;
            _beepService = services.GetRequiredService<IBeepService>();
            _authService = services.GetRequiredService<AuthService>();
            _viewCache = new Dictionary<string, TemplateUserControl>();

            InitializeComponent();
            InitializeUI();
            SetupNavigation();
            ApplyBusinessTheme();
        }
        #endregion

        #region Initialization
        private void InitializeUI()
        {
            // Basic form setup
            this.Text = "SampleBusinessApp - Enterprise Management System";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            // Initialize UI Manager
          
            _uiManager.Initialize(this);
            _uiManager.Title = "SampleBusinessApp";
            _uiManager.LogoImage = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.cool.svg";

            // Create main layout
            CreateMainLayout();
        }

        private void CreateMainLayout()
        {
            this.SuspendLayout();

            // Create menu bar
            _menuBar = new BeepWebHeaderAppBar
            {
                Dock = DockStyle.Top,
                Height = 60,
                Text = "SampleBusinessApp",
                LogoImagePath = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.cool.svg",
                Theme = this.Theme
            };

            // Create side menu
            _sideMenu = new BeepSideMenu
            {
                Dock = DockStyle.Left,
                Width = 250,
                LogoImage = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.cool.svg",
                Theme = this.Theme
            };

            // Create content panel
            _contentPanel = new BeepPanel
            {
                Dock = DockStyle.Fill,
                ShowTitle = false,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(10),
                Theme = this.Theme
            };

            // Create status panel
            _statusPanel = new BeepPanel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                ShowTitle = false,
                BackColor = Color.FromArgb(64, 69, 76),
                Theme = this.Theme
            };

            _statusLabel = new BeepLabel
            {
                Text = "Ready",
                Location = new Point(10, 5),
                ForeColor = Color.White,
                AutoSize = true,
                Theme = this.Theme
            };

            _userLabel = new BeepLabel
            {
                Text = "Not logged in",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                ForeColor = Color.LightGray,
                AutoSize = true,
                Theme = this.Theme
            };

            _statusPanel.Controls.AddRange(new Control[] { _statusLabel, _userLabel });

            // Add controls to form
            this.Controls.AddRange(new Control[] 
            { 
                _contentPanel, 
                _sideMenu, 
                _menuBar, 
                _statusPanel 
            });

            // Assign to UI Manager
            _uiManager.BeepAppBar= _menuBar;
            _uiManager.BeepSideMenu = _sideMenu;

            this.ResumeLayout(false);
        }

        private void SetupNavigation()
        {
            // Setup main menu items
            SetupMainMenu();
            
            // Setup side menu navigation
            SetupSideMenu();
            
            // Wire events
            WireNavigationEvents();
        }

        private void SetupMainMenu()
        {
            // Main menu items for the menu bar
            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("New Order", null, (s, e) => NavigateToView("SalesOrder")),
                new ToolStripMenuItem("New Customer", null, (s, e) => NavigateToView("CustomerEdit")),
                new ToolStripMenuItem("New Product", null, (s, e) => NavigateToView("ProductEdit")),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Import Data", null, (s, e) => NavigateToView("Import")),
                new ToolStripMenuItem("Export Data", null, (s, e) => NavigateToView("Export")),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Exit", null, (s, e) => this.Close())
            });

            var viewMenu = new ToolStripMenuItem("View");
            viewMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("Dashboard", null, (s, e) => NavigateToView("Dashboard")),
                new ToolStripMenuItem("Customer List", null, (s, e) => NavigateToView("CustomerList")),
                new ToolStripMenuItem("Product Catalog", null, (s, e) => NavigateToView("ProductCatalog")),
                new ToolStripMenuItem("Orders", null, (s, e) => NavigateToView("OrderList")),
                new ToolStripMenuItem("Invoices", null, (s, e) => NavigateToView("InvoiceList")),
                new ToolStripMenuItem("Inventory", null, (s, e) => NavigateToView("Inventory")),
                new ToolStripMenuItem("Categories", null, (s, e) => NavigateToView("CategoryManagement"))
            });

            var reportsMenu = new ToolStripMenuItem("Reports");
            reportsMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("Sales Reports", null, (s, e) => NavigateToView("SalesReports")),
                new ToolStripMenuItem("Inventory Reports", null, (s, e) => NavigateToView("InventoryReports")),
                new ToolStripMenuItem("Customer Reports", null, (s, e) => NavigateToView("CustomerReports")),
                new ToolStripMenuItem("Financial Reports", null, (s, e) => NavigateToView("FinancialReports"))
            });

            var adminMenu = new ToolStripMenuItem("Administration");
            adminMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("User Management", null, (s, e) => NavigateToView("UserManagement")),
                new ToolStripMenuItem("Settings", null, (s, e) => NavigateToView("Settings")),
                new ToolStripMenuItem("Backup & Restore", null, (s, e) => NavigateToView("BackupRestore")),
                new ToolStripMenuItem("Audit Log", null, (s, e) => NavigateToView("AuditLog"))
            });

            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("User Guide", null, (s, e) => ShowHelp()),
                new ToolStripMenuItem("About", null, (s, e) => ShowAbout())
            });

            // Add menus to menu bar (this would need to be implemented in BeepMenuAppBar)
            // For now, we'll use events to handle navigation
        }

        private void SetupSideMenu()
        {
            if (_sideMenu == null) return;

            var items = _sideMenu.Items;
            items.Clear();

            // Dashboard
            var dashboard = new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem
            {
                Name = "Dashboard",
                Text = "Dashboard",
                ImagePath = "dashboard.svg"
            };
            items.Add(dashboard);

            // Customers section
            var customers = new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem
            {
                Name = "Customers",
                Text = "Customers",
                ImagePath = "users.svg"
            };
            customers.Children.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Name = "CustomerList", Text = "Customer List", ImagePath = "list.svg" });
            customers.Children.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Name = "CustomerEdit", Text = "Add Customer", ImagePath = "add.svg" });
            items.Add(customers);

            // Products section
            var products = new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem
            {
                Name = "Products",
                Text = "Products",
                ImagePath = "products.svg"
            };
            products.Children.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Name = "ProductCatalog", Text = "Catalog", ImagePath = "catalog.svg" });
            products.Children.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Name = "Inventory", Text = "Inventory", ImagePath = "inventory.svg" });
            products.Children.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Name = "CategoryManagement", Text = "Categories", ImagePath = "categories.svg" });
            items.Add(products);

            // Administration
            var admin = new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem
            {
                Name = "Administration",
                Text = "Administration",
                ImagePath = "settings.svg"
            };
            admin.Children.Add(new TheTechIdea.Beep.Winform.Controls.Models.SimpleItem { Name = "Settings", Text = "Settings", ImagePath = "settings.svg" });
            items.Add(admin);

            // Handle clicks
            _sideMenu.MenuItemClicked -= SideMenu_MenuItemClicked;
            _sideMenu.MenuItemClicked += SideMenu_MenuItemClicked;

            // Default view
            NavigateToView("Dashboard");
        }

        private void SideMenu_MenuItemClicked(TheTechIdea.Beep.Winform.Controls.Models.SimpleItem obj)
        {
            if (string.IsNullOrWhiteSpace(obj?.Name)) return;
            switch (obj.Name)
            {
                case "Dashboard":
                case "CustomerList":
                case "CustomerEdit":
                case "ProductCatalog":
                case "Inventory":
                case "CategoryManagement":
                case "Settings":
                    NavigateToView(obj.Name);
                    break;
            }
        }

        private void WireNavigationEvents()
        {
            // Wire menu bar events
            if (_menuBar != null)
            {
                // _menuBar.MenuItemClicked += OnMenuItemClicked;
            }

            // Wire side menu events
            if (_sideMenu != null)
            {
                // _sideMenu.ItemSelected += OnSideMenuItemSelected;
            }
        }
        #endregion

        #region Navigation Methods
        public void NavigateToView(string viewName, Dictionary<string, object> parameters = null)
        {
            try
            {
                TemplateUserControl view = GetOrCreateView(viewName);
                
                if (view != null)
                {
                    ShowView(view);
                    
                    // If view supports parameters, pass them
                    if (view is IParameterizedView paramView && parameters != null)
                    {
                        paramView.SetParameters(parameters);
                    }

                    UpdateStatus($"Navigated to {viewName}");
                }
                else
                {
                    UpdateStatus($"View '{viewName}' not found");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Navigation error: {ex.Message}");
                _beepService?.DMEEditor?.AddLogMessage("MainBusinessForm", 
                    $"Navigation error to {viewName}: {ex.Message}", 
                    DateTime.Now, -1, null, Errors.Failed);
            }
        }

        private TemplateUserControl GetOrCreateView(string viewName)
        {
            // Check cache first
            if (_viewCache.ContainsKey(viewName))
            {
                return _viewCache[viewName];
            }

            // Create new view instance
            TemplateUserControl view = CreateViewInstance(viewName);
            
            if (view != null)
            {
                _viewCache[viewName] = view;
            }

            return view;
        }

        private TemplateUserControl CreateViewInstance(string viewName)
        {
            // Create view instances based on name
            // This will be expanded as we create more views
            return viewName switch
            {
                "CustomerList" => CreateView<Views.Customer.CustomerListView>(),
                "CustomerEdit" => CreateView<Views.Customer.CustomerEditView>(),
                "Inventory" => CreateView<Views.Product.InventoryView>(),
                "CategoryManagement" => CreateView<Views.Product.CategoryManagementView>(),
                "Dashboard" => CreateView<Views.Dashboard.MainDashboardView>(),
                "Settings" => CreateView<Views.Settings.SettingsView>(),
                "ProductCatalog" => CreateView<Views.Product.ProductCatalogView>(),
                "ProductEdit" => CreateView<Views.Product.ProductEditView>(),
                _ => null
            };
        }

        private T CreateView<T>() where T : TemplateUserControl
        {
            try
            {
                // Try to create with services constructor first
                return (T)Activator.CreateInstance(typeof(T), _services);
            }
            catch
            {
                try
                {
                    // Fallback to parameterless constructor
                    return (T)Activator.CreateInstance(typeof(T));
                }
                catch (Exception ex)
                {
                    _beepService?.DMEEditor?.AddLogMessage("MainBusinessForm",
                        $"Failed to create view {typeof(T).Name}: {ex.Message}",
                        DateTime.Now, -1, null, Errors.Failed);
                    return null;
                }
            }
        }

        private void ShowView(TemplateUserControl view)
        {
            // Hide current view
            if (_currentView != null)
            {
                _currentView.Visible = false;
                _contentPanel.Controls.Remove(_currentView);
            }

            // Show new view
            view.Dock = DockStyle.Fill;
            _contentPanel.Controls.Add(view);
            view.Visible = true;
            _currentView = view;

            // Apply theme to new view
            if (view is IThemeable themeableView)
            {
                themeableView.Theme = this.Theme;
            }
        }

        public void UpdateStatus(string message)
        {
            if (_statusLabel != null)
            {
                _statusLabel.Text = message;
            }
        }

        public void UpdateUserInfo(string username)
        {
            if (_userLabel != null)
            {
                _userLabel.Text = $"User: {username}";
                _userLabel.Location = new Point(_statusPanel.Width - _userLabel.Width - 10, 5);
            }
        }
        #endregion

        #region Event Handlers
        private void OnMenuItemClicked(object sender, EventArgs e)
        {
            // Handle menu bar clicks
        }

        private void OnSideMenuItemSelected(object sender, EventArgs e)
        {
            // Handle side menu selections
        }

        private void ShowHelp()
        {
            MessageBox.Show("User Guide would open here", "Help", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowAbout()
        {
            MessageBox.Show(
                "SampleBusinessApp v1.0\n" +
                "Enterprise Business Management System\n\n" +
                "Built with Beep Framework\n" +
                "© 2024 The Tech Idea",
                "About SampleBusinessApp",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        #endregion

        #region Theme Management
        private void ApplyBusinessTheme()
        {
            // Apply professional business theme
            this.Theme = "BusinessProfessional";
            
            if (_uiManager != null)
            {
                _uiManager.Theme = this.Theme;
            }

            // Apply theme to all child controls
            ApplyThemeToControls();
        }

        private void ApplyThemeToControls()
        {
            var theme = this.Theme;
            
            if (_menuBar != null) _menuBar.Theme = theme;
            if (_sideMenu != null) _sideMenu.Theme = theme;
            if (_contentPanel != null) _contentPanel.Theme = theme;
            if (_statusPanel != null) _statusPanel.Theme = theme;
            if (_statusLabel != null) _statusLabel.Theme = theme;
            if (_userLabel != null) _userLabel.Theme = theme;
        }
        #endregion

        #region Cleanup
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Cleanup resources
            foreach (var view in _viewCache.Values)
            {
                view?.Dispose();
            }
            _viewCache.Clear();

            base.OnFormClosing(e);
        }
        #endregion

        #region Designer Support
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainBusinessForm));
            beepSideBar1 = new TheTechIdea.Beep.Winform.Controls.SideBar.BeepSideBar();
            SuspendLayout();
            // 
            // beepSideBar1
            // 
            beepSideBar1.AccentColor = Color.FromArgb(0, 120, 215);
            beepSideBar1.AnimationType = DisplayAnimationType.None;
            beepSideBar1.ApplyThemeToChilds = true;
            beepSideBar1.AutoDrawHitListComponents = true;
            beepSideBar1.BackColor = Color.FromArgb(255, 255, 255);
            beepSideBar1.BadgeBackColor = Color.Red;
            beepSideBar1.BadgeFont = new Font("Segoe UI", 8F, FontStyle.Bold);
            beepSideBar1.BadgeForeColor = Color.White;
            beepSideBar1.BadgeShape = BadgeShape.Circle;
            beepSideBar1.BadgeText = "";
            beepSideBar1.BlockID = null;
            beepSideBar1.BorderColor = Color.Black;
            beepSideBar1.BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            beepSideBar1.BorderPainter = TheTechIdea.Beep.Winform.Controls.Common.BeepControlStyle.None;
            beepSideBar1.BorderRadius = 8;
            beepSideBar1.BorderStyle = BorderStyle.FixedSingle;
            beepSideBar1.BorderThickness = 1;
            beepSideBar1.BottomoffsetForDrawingRect = 0;
            beepSideBar1.BoundProperty = null;
            beepSideBar1.CanBeFocused = false;
            beepSideBar1.CanBeHovered = true;
            beepSideBar1.CanBePressed = true;
            beepSideBar1.CanBeSelected = true;
            beepSideBar1.Category = DbFieldCategory.String;
            beepSideBar1.CollapseIconPath = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.angle-small-down.svg";
            beepSideBar1.ComponentName = "BaseControl";
            beepSideBar1.DataContext = null;
            beepSideBar1.DataSourceProperty = null;
            beepSideBar1.DefaultItemImagePath = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.menu.svg";
            beepSideBar1.DisabledBackColor = Color.LightGray;
            beepSideBar1.DisabledBorderColor = Color.Gray;
            beepSideBar1.DisabledForeColor = Color.DarkGray;
            beepSideBar1.Dock = DockStyle.Left;
            beepSideBar1.DrawingRect = new Rectangle(4, 4, 116, 850);
            beepSideBar1.Easing = EasingType.Linear;
            beepSideBar1.EnableHighQualityRendering = true;
            beepSideBar1.EnableMaterialStyle = false;
            beepSideBar1.EnableRippleEffect = false;
            beepSideBar1.EnableSplashEffect = false;
            beepSideBar1.ErrorColor = Color.FromArgb(176, 0, 32);
            beepSideBar1.ErrorText = "";
            beepSideBar1.ExpandIconPath = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.angle-small-right.svg";
            beepSideBar1.ExternalDrawingLayer = TheTechIdea.Beep.Winform.Controls.Models.DrawingLayer.AfterAll;
            beepSideBar1.FieldID = null;
            beepSideBar1.FilledBackgroundColor = Color.FromArgb(20, 0, 0, 0);
            beepSideBar1.FloatingLabel = true;
            beepSideBar1.FocusBackColor = Color.LightYellow;
            beepSideBar1.FocusBorderColor = Color.RoyalBlue;
            beepSideBar1.FocusForeColor = Color.Black;
            beepSideBar1.FocusIndicatorColor = Color.RoyalBlue;
            beepSideBar1.Form = null;
            beepSideBar1.GlassmorphismBlur = 10F;
            beepSideBar1.GlassmorphismOpacity = 0.1F;
            beepSideBar1.GradientAngle = 0F;
            beepSideBar1.GradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            beepSideBar1.GradientEndColor = Color.Gray;
            beepSideBar1.GradientStartColor = Color.LightGray;
            beepSideBar1.GridMode = false;
            beepSideBar1.GuidID = "e8dde92c-7726-45e5-8e60-fa9121a4ee8c";
            beepSideBar1.HasError = false;
            beepSideBar1.HelperText = "";
            beepSideBar1.HelperTextOn = false;
            beepSideBar1.HitAreaEventOn = false;
            beepSideBar1.HitTestControl = null;
            beepSideBar1.HoverBackColor = Color.LightBlue;
            beepSideBar1.HoverBorderColor = Color.Blue;
            beepSideBar1.HoveredBackcolor = Color.LightBlue;
            beepSideBar1.HoverForeColor = Color.Black;
            beepSideBar1.IconSize = 20;
            beepSideBar1.Id = -1;
            beepSideBar1.InactiveBorderColor = Color.Gray;
            beepSideBar1.InnerShape = null;
            beepSideBar1.IsAcceptButton = false;
            beepSideBar1.IsBorderAffectedByTheme = true;
            beepSideBar1.IsCancelButton = false;
            beepSideBar1.IsChild = true;
            beepSideBar1.IsCustomeBorder = false;
            beepSideBar1.IsDefault = false;
            beepSideBar1.IsDeleted = false;
            beepSideBar1.IsDirty = false;
            beepSideBar1.IsEditable = true;
            beepSideBar1.IsFocused = false;
            beepSideBar1.IsFrameless = false;
            beepSideBar1.IsHovered = false;
            beepSideBar1.IsNew = false;
            beepSideBar1.IsPressed = false;
            beepSideBar1.IsReadOnly = false;
            beepSideBar1.IsRequired = false;
            beepSideBar1.IsRounded = true;
            beepSideBar1.IsRoundedAffectedByTheme = true;
            beepSideBar1.IsSelected = false;
            beepSideBar1.IsSelectedOptionOn = false;
            beepSideBar1.IsShadowAffectedByTheme = true;
            beepSideBar1.IsTransparentBackground = false;
            beepSideBar1.IsValid = true;
            beepSideBar1.IsVisible = true;
            beepSideBar1.LabelText = "";
            beepSideBar1.LabelTextOn = false;
            beepSideBar1.LeadingIconPath = "";
            beepSideBar1.LeadingImagePath = "";
            beepSideBar1.LeftoffsetForDrawingRect = 0;
            beepSideBar1.LinkedProperty = null;
            beepSideBar1.Location = new Point(1, 41);
            beepSideBar1.MaterialBorderVariant = MaterialTextFieldVariant.Standard;
            beepSideBar1.MaxHitListDrawPerFrame = 0;
            beepSideBar1.ModernGradientType = ModernGradientType.None;
            beepSideBar1.Name = "beepSideBar1";
            beepSideBar1.OverrideFontSize = TypeStyleFontSize.None;
            beepSideBar1.PainterKind = TheTechIdea.Beep.Winform.Controls.Common.BaseControlPainterKind.Classic;
            beepSideBar1.ParentBackColor = Color.FromArgb(255, 255, 255);
            beepSideBar1.ParentControl = null;
            beepSideBar1.PressedBackColor = Color.Gray;
            beepSideBar1.PressedBorderColor = Color.DarkGray;
            beepSideBar1.PressedForeColor = Color.White;
            beepSideBar1.RadialCenter = (PointF)resources.GetObject("beepSideBar1.RadialCenter");
            beepSideBar1.RightoffsetForDrawingRect = 0;
            beepSideBar1.SavedGuidID = null;
            beepSideBar1.SavedID = null;
            beepSideBar1.ScaleMode = ImageScaleMode.KeepAspectRatio;
            beepSideBar1.SelectedBackColor = Color.LightGreen;
            beepSideBar1.SelectedBorderColor = Color.Green;
            beepSideBar1.SelectedForeColor = Color.Black;
            beepSideBar1.SelectedItem = null;
            beepSideBar1.SelectedValue = null;
            beepSideBar1.ShadowColor = Color.Black;
            beepSideBar1.ShadowOffset = 3;
            beepSideBar1.ShadowOpacity = 0.25F;
            beepSideBar1.ShowAllBorders = false;
            beepSideBar1.ShowBottomBorder = false;
            beepSideBar1.ShowFocusIndicator = false;
            beepSideBar1.ShowLeftBorder = false;
            beepSideBar1.ShowRightBorder = false;
            beepSideBar1.ShowShadow = false;
            beepSideBar1.ShowTopBorder = false;
            beepSideBar1.Size = new Size(124, 858);
            beepSideBar1.SlideFrom = SlideDirection.Left;
            beepSideBar1.StaticNotMoving = false;
            beepSideBar1.TabIndex = 0;
            beepSideBar1.Tag = this;
            beepSideBar1.TempBackColor = Color.LightGray;
            beepSideBar1.Text = "beepSideBar1";
            beepSideBar1.TextFont = new Font("Arial", 10F);
            beepSideBar1.Theme = null;
            beepSideBar1.ToolTipText = null;
            beepSideBar1.TopoffsetForDrawingRect = 0;
            beepSideBar1.TrailingIconPath = "";
            beepSideBar1.TrailingImagePath = "";
            beepSideBar1.UseExternalBufferedGraphics = true;
            beepSideBar1.UseFormStylePaint = true;
            beepSideBar1.UseGlassmorphism = false;
            beepSideBar1.UseGradientBackground = false;
            beepSideBar1.UseRichToolTip = true;
            beepSideBar1.UseThemeFont = true;
            // 
            // MainBusinessForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1400, 900);
            Controls.Add(beepSideBar1);
            Location = new Point(0, 0);
            Name = "MainBusinessForm";
            Text = "SampleBusinessApp";
            ResumeLayout(false);
        }
        #endregion
    }

    // Interface for views that accept parameters
    public interface IParameterizedView
    {
        void SetParameters(Dictionary<string, object> parameters);
    }

    // Interface for themeable views
    public interface IThemeable
    {
        string Theme { get; set; }
    }
}