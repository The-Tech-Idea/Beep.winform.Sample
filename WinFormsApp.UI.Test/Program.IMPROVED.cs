using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Desktop.Common.Helpers;
using TheTechIdea.Beep.Desktop.Common.Util;
using TheTechIdea.Beep.Desktop.Common.Util.Configuration;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.DialogsManagers;
using TheTechIdea.Beep.Winform.Controls.FontManagement;
using TheTechIdea.Beep.Winform.Controls.Forms;
using TheTechIdea.Beep.Winform.Controls.Helpers;
using TheTechIdea.Beep.Winform.Controls.Integrated;
using TheTechIdea.Beep.Winform.Controls.Styling;
using TheTechIdea.Beep.Winform.Controls.ThemeManagement;
using TheTechIdea.Beep.Winform.Default.Views;
using TheTechIdea.Beep.Winform.Default.Views.Configuration;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using WinFormsApp.UI.Test.SampleBusinessApp.Data;
using WinFormsApp.UI.Test.SampleBusinessApp.Forms;
using WinFormsApp.UI.Test.SampleBusinessApp.Services;
using WinFormsApp.UI.Test.SampleBusinessApp.Views;

namespace WinFormsApp.UI.Test
{
    internal static class Program
    {
        // ✅ NEW: Event handlers stored for proper cleanup
        private static EventHandler<IRoutingManager> _registerRoutesHandler;
        private static EventHandler<List<string>> _loadGraphicsHandler;
        private static EventHandler<List<string>> _loadFontsHandler;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // ✅ Initialize configuration system early
            InitializeConfiguration(args);

            StartSampleBusinessApp();
        }

        #region Environment and Configuration

        /// <summary>
        /// Initialize the configuration system with environment detection
        /// </summary>
        private static void InitializeConfiguration(string[] args)
        {
            try
            {
                // Parse environment from command line args
                string environment = ParseEnvironmentFromArgs(args);

                // Set environment variable if provided via command line
                if (!string.IsNullOrEmpty(environment))
                {
                    Environment.SetEnvironmentVariable("BEEP_ENVIRONMENT", environment);
                    Debug.WriteLine($"✅ Environment set from command line: {environment}");
                }

                // Initialize the configuration system
                var config = UserSettingsManager.Configuration;

                Debug.WriteLine($"✅ Configuration loaded successfully");
                Debug.WriteLine($"   Environment: {config.Environment}");
                Debug.WriteLine($"   Database: {config.Settings.Database.DataSourceType}");
                Debug.WriteLine($"   AutoLogin: {config.Settings.Authentication.AutoLoginInDevelopment}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"⚠️ Configuration initialization error: {ex.Message}");
                Debug.WriteLine($"   Stack trace: {ex.StackTrace}");
                Debug.WriteLine($"   Continuing with default configuration...");
                // Continue with defaults - application will work with hardcoded values
            }
        }

        /// <summary>
        /// Parse environment from command line arguments
        /// Supports: --environment, -e, --env
        /// </summary>
        private static string ParseEnvironmentFromArgs(string[] args)
        {
            if (args == null || args.Length == 0)
                return null;

            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals("--environment", StringComparison.OrdinalIgnoreCase) ||
                    args[i].Equals("-e", StringComparison.OrdinalIgnoreCase) ||
                    args[i].Equals("--env", StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        #endregion

        #region Application Startup

        private static void StartSampleBusinessApp()
        {
            IHost host = null;
            try
            {
                Debug.WriteLine("═══════════════════════════════════════════════════════════");
                Debug.WriteLine("📱 Starting Sample Business App");
                Debug.WriteLine("═══════════════════════════════════════════════════════════");

                // Create HostApplicationBuilder
                var builder = Host.CreateApplicationBuilder();

                // Register Beep Services
                BeepDesktopServices.RegisterServices(builder);

                // Build the host
                host = builder.Build();
                
                Debug.WriteLine("✅ Host built successfully");

                // Configure theme and fonts
                BeepThemesManager.CurrentStyle = FormStyle.Terminal;
                FontListHelper.EnsureFontsLoaded();

                // Configure services
                BeepDesktopServices.ConfigureServices(host);
                BeepDesktopServices.ConfigureControlsandMenus();

                Debug.WriteLine("✅ Services configured");

                // Configure AppManager using configuration settings
                ConfigureAppManager();

                // Subscribe to events for custom routes and resources
                SubscribeToBeepEvents();
                Debug.WriteLine("✅ Event handlers subscribed");

                // Load assemblies and plugins
                var result = BeepDesktopServices.StartLoading(
                    new string[] { "BeepEnterprize", "TheTechIdea", "Beep" }, 
                    showWaitForm: true
                );

                if (result.Flag != Errors.Ok)
                {
                    HandleLoadingError(result);
                    return;
                }

                Debug.WriteLine("✅ Assemblies loaded successfully");

                // Show home page
                BeepDesktopServices.AppManager.ShowHome();
                Debug.WriteLine("✅ Application UI displayed");
            }
            catch (Exception ex)
            {
                HandleCriticalError(ex);
            }
            finally
            {
                // ✅ Proper cleanup sequence
                try
                {
                    UnsubscribeFromBeepEvents();
                    Debug.WriteLine("✅ Event handlers unsubscribed");

                    UserSettingsManager.Dispose();
                    BeepDesktopServices.DisposeServices();
                    PaintersFactory.ClearCache();
                    host?.Dispose();

                    Debug.WriteLine("✅ All resources disposed");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"⚠️ Cleanup error: {ex.Message}");
                }

                Debug.WriteLine("═══════════════════════════════════════════════════════════");
                Debug.WriteLine("📱 Application closed");
                Debug.WriteLine("═══════════════════════════════════════════════════════════");
                Application.Exit();
            }
        }

        /// <summary>
        /// Configure AppManager with all required settings
        /// </summary>
        private static void ConfigureAppManager()
        {
            try
            {
                BeepDesktopServices.AppManager.DialogManager = new BeepDialogManager();
                BeepDesktopServices.AppManager.Title = "Beep Data Management Platform";
                BeepDesktopServices.AppManager.Theme = "TerminalTheme";
                BeepDesktopServices.AppManager.Style = FormStyle.Terminal;
                BeepDesktopServices.AppManager.WaitFormType = typeof(BeepWait);
                BeepDesktopServices.AppManager.IconUrl = "simpleinfoapps.ico";
                BeepDesktopServices.AppManager.LogoUrl = "simpleinfoapps.svg";
                BeepDesktopServices.AppManager.HomePageName = "MainFrm";
                BeepDesktopServices.AppManager.HomePageDescription = "homePageDescription";

                // Set up factory delegates for menu and method handling
                SimpleItemFactory.SetDelegates(
                    HandlersFactory.GlobalMenuItemsProvider,
                    HandlersFactory.RunFunctionHandler,
                    HandlersFactory.RunFunctionWithTreeHandler,
                    HandlersFactory.RunMethodFromObjectHandler,
                    HandlersFactory.RunMethodFromExtensionWithTreeHandler,
                    HandlersFactory.RunMethodFromExtensionHandler
                );

                Debug.WriteLine("✅ AppManager configured");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ AppManager configuration error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Handle loading errors with detailed diagnostics
        /// </summary>
        private static void HandleLoadingError(IErrorsInfo result)
        {
            string errorMessage = $"Loading failed: {result.Message}";
            Debug.WriteLine($"❌ {errorMessage}");

            if (result.Exception != null)
            {
                Debug.WriteLine($"   Exception: {result.Exception.Message}");
                Debug.WriteLine($"   Stack trace: {result.Exception.StackTrace}");
            }

            MessageBox.Show(
                errorMessage,
                "Loading Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /// <summary>
        /// Handle critical unhandled errors
        /// </summary>
        private static void HandleCriticalError(Exception ex)
        {
            Debug.WriteLine($"❌ CRITICAL ERROR: {ex.Message}");
            Debug.WriteLine($"   Stack trace: {ex.StackTrace}");

            var config = UserSettingsManager.Configuration;
            string isDevelopment = config?.Environment?.Equals("Development", StringComparison.OrdinalIgnoreCase) ?? false ? "yes" : "no";

            string errorMessage = isDevelopment == "yes"
                ? $"Critical Error in {config?.Environment ?? "Unknown"} Environment:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}"
                : $"A critical error occurred: {ex.Message}\n\nPlease contact support or check the application logs.";

            MessageBox.Show(
                errorMessage,
                $"Critical Error ({isDevelopment})",
                MessageBoxButtons.OK,
                MessageBoxIcon.Stop
            );
        }

        #endregion

        #region Event Management

        /// <summary>
        /// Subscribe to Beep framework events for custom routes, graphics, and fonts
        /// </summary>
        private static void SubscribeToBeepEvents()
        {
            try
            {
                // Register routes event
                _registerRoutesHandler = (sender, routingManager) =>
                {
                    try
                    {
                        Debug.WriteLine("📍 Registering application routes...");
                        RegisterStandardBeepRoutes(routingManager);
                        RegisterSampleBusinessAppRoutes(routingManager);
                        Debug.WriteLine("✅ Routes registered successfully");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"❌ Error registering routes: {ex.Message}");
                    }
                };

                // Load graphics event
                _loadGraphicsHandler = (sender, graphicsLocations) =>
                {
                    try
                    {
                        Debug.WriteLine("🖼️  Loading custom graphics paths...");
                        var customPaths = new[]
                        {
                            @".\SampleBusinessApp\Resources\Images",
                            @".\Resources\Images",
                        };

                        foreach (var path in customPaths)
                        {
                            if (Directory.Exists(path))
                            {
                                graphicsLocations.Add(path);
                                Debug.WriteLine($"   ✅ Added: {path}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"⚠️ Error loading graphics: {ex.Message}");
                    }
                };

                // Load fonts event
                _loadFontsHandler = (sender, fontLocations) =>
                {
                    try
                    {
                        Debug.WriteLine("🔤 Loading custom font paths...");
                        var customPaths = new[]
                        {
                            @".\SampleBusinessApp\Resources\Fonts",
                            @".\Resources\Fonts",
                        };

                        foreach (var path in customPaths)
                        {
                            if (Directory.Exists(path))
                            {
                                fontLocations.Add(path);
                                Debug.WriteLine($"   ✅ Added: {path}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"⚠️ Error loading fonts: {ex.Message}");
                    }
                };

                // Attach handlers
                BeepDesktopServices.OnRegisterRoutes += _registerRoutesHandler;
                BeepDesktopServices.OnLoadGraphics += _loadGraphicsHandler;
                BeepDesktopServices.OnLoadFonts += _loadFontsHandler;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error subscribing to events: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Unsubscribe from Beep framework events for proper cleanup
        /// </summary>
        private static void UnsubscribeFromBeepEvents()
        {
            try
            {
                if (_registerRoutesHandler != null)
                    BeepDesktopServices.OnRegisterRoutes -= _registerRoutesHandler;

                if (_loadGraphicsHandler != null)
                    BeepDesktopServices.OnLoadGraphics -= _loadGraphicsHandler;

                if (_loadFontsHandler != null)
                    BeepDesktopServices.OnLoadFonts -= _loadFontsHandler;

                Debug.WriteLine("✅ Event handlers cleaned up");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"⚠️ Error unsubscribing from events: {ex.Message}");
            }
        }

        #endregion

        #region Route Registration

        /// <summary>
        /// Register standard Beep framework routes
        /// </summary>
        private static void RegisterStandardBeepRoutes(IRoutingManager routingManager)
        {
            var standardRoutes = new Dictionary<string, string>
            {
                { "MainFrm", "MainFrm" },
                { "Form1", "Form1" },
                { "Form2", "Form2" },
                { "uc_ConnnectionDrivers", "uc_ConnnectionDrivers" },
                { "uc_FilterForm", "uc_FilterForm" },
                { "uc_RDBMSConnections", "uc_RDBMSConnections" },
                { "uc_FileConnections", "uc_FileConnections" },
                { "uc_EntityEditor", "uc_EntityEditor" },
                { "uc_CreateLocalDB", "uc_CreateLocalDB" },
                { "uc_diagraming", "uc_diagraming" },
                { "uc_FunctiontoFunctionMapping", "uc_FunctiontoFunctionMapping" },
                { "uc_DataEdit", "uc_DataEdit" },
                { "uc_CopyEntities", "uc_CopyEntities" },
                { "uc_DataConnections", "uc_DataConnections" }
            };

            RegisterRoutes(routingManager, standardRoutes, isStandard: true);
        }

        /// <summary>
        /// Register Sample Business App specific routes
        /// </summary>
        private static void RegisterSampleBusinessAppRoutes(IRoutingManager routingManager)
        {
            var appRoutes = new Dictionary<string, string>
            {
                // Core Application Views
                { "MainForm", "MainForm" },
                { "LoginForm", "LoginForm" },
                
                // Dashboard and Analytics
                { "DashboardView", "DashboardView" },
                { "AnalyticsView", "AnalyticsView" },
                { "ChartsShowcaseView", "ChartsShowcaseView" },
                { "MetricsView", "MetricsView" },
                
                // Data Management
                { "ProductsView", "ProductsView" },
                { "CustomersView", "CustomersView" },
                { "OrdersView", "OrdersView" },
                { "InventoryView", "InventoryView" },
                
                // Task & Project Management
                { "TasksView", "TasksView" },
                { "ProjectKanbanView", "ProjectKanbanView" },
                { "CalendarView", "CalendarView" },
                { "TimeTrackingView", "TimeTrackingView" },
                
                // Reports and Business Intelligence
                { "ReportsView", "ReportsView" },
                { "KPIDashboardView", "KPIDashboardView" },
                { "BusinessIntelligenceView", "BusinessIntelligenceView" },
                
                // Configuration and Settings
                { "SettingsView", "SettingsView" },
                { "UserProfileView", "UserProfileView" },
                { "SystemConfigView", "SystemConfigView" },
                { "ThemeManagerView", "ThemeManagerView" },
                
                // Enhanced Control Demos
                { "EnhancedProgressBarDemo", "EnhancedProgressBarDemo" },
                { "EnhancedNumericUpDownDemo", "EnhancedNumericUpDownDemo" },
                { "EnhancedStepperBarDemo", "EnhancedStepperBarDemo" },
                { "EnhancedStarRatingDemo", "EnhancedStarRatingDemo" },
                { "EnhancedDatePickerDemo", "EnhancedDatePickerDemo" },
                
                // Specialized Views
                { "WizardShowcaseView", "WizardShowcaseView" },
                { "FormControlsShowcaseView", "FormControlsShowcaseView" },
                { "LayoutControlsShowcaseView", "LayoutControlsShowcaseView" },
                { "BusinessComponentsShowcaseView", "BusinessComponentsShowcaseView" },
                { "DataVisualizationShowcaseView", "DataVisualizationShowcaseView" }
            };

            RegisterRoutes(routingManager, appRoutes, isStandard: false);
        }

        /// <summary>
        /// Register a collection of routes with error handling
        /// </summary>
        private static void RegisterRoutes(IRoutingManager routingManager, Dictionary<string, string> routes, bool isStandard)
        {
            int successCount = 0;
            int failureCount = 0;

            foreach (var route in routes)
            {
                try
                {
                    var result = routingManager.RegisterRouteByName(route.Key, route.Value);
                    
                    if (result?.Flag == Errors.Ok)
                    {
                        Debug.WriteLine($"   ✅ Registered: {route.Key}");
                        successCount++;
                    }
                    else
                    {
                        Debug.WriteLine($"   ❌ Failed: {route.Key} - {result?.Message}");
                        failureCount++;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"   ⚠️ Exception: {route.Key} - {ex.Message}");
                    failureCount++;
                }
            }

            string type = isStandard ? "standard" : "app";
            Debug.WriteLine($"   Summary: {successCount} {type} routes registered, {failureCount} failed");
        }

        #endregion
    }
}
