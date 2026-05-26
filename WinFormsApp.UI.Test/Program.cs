using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Desktop.Common.Helpers;
using TheTechIdea.Beep.Desktop.Common.Util;
using TheTechIdea.Beep.Desktop.Common.Util.Configuration;
using TheTechIdea.Beep.Tools;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.DialogsManagers;
using TheTechIdea.Beep.Winform.Controls.FontManagement;
using TheTechIdea.Beep.Winform.Controls.Forms;
using TheTechIdea.Beep.Winform.Controls.Helpers;
using TheTechIdea.Beep.Winform.Controls.Styling;
using TheTechIdea.Beep.Winform.Extensions;
using TheTechIdea.Beep.Winform.Controls.ThemeManagement;
using TheTechIdea.Beep.Winform.Default.Views;
using TheTechIdea.Beep.Winform.Default.Views.Configuration;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using TheTechIdea.Beep.Shared;

namespace WinFormsApp.UI.Test
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            

            // Initialize configuration system early
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
                }

                // Initialize the configuration system
                var config = UserSettingsManager.Configuration;

                Debug.WriteLine($"Sample Business App - Environment: {config.Environment}");
                Debug.WriteLine($"Sample Business App - Configuration loaded from: {config.Environment}");

                // Log configuration details
                var dbSettings = config.Settings.Database;
                Debug.WriteLine($"Database: {dbSettings.DataSourceType} - {dbSettings.ConnectionString}");

                var authSettings = config.Settings.Authentication;
                Debug.WriteLine($"Authentication: RememberCredentials={authSettings.RememberUserCredentials}, " +
                              $"AutoLogin={authSettings.AutoLoginInDevelopment}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Configuration initialization error: {ex.Message}");
                // Continue with defaults if configuration fails
            }
        }

        /// <summary>
        /// Parse environment from command line arguments
        /// </summary>
        private static string ParseEnvironmentFromArgs(string[] args)
        {
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

       
        private static string ParseDemoFromArgs(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals("--demo", StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return null;
        }
        #endregion

        private static void StartSampleBusinessApp()
        {
            // Create HostApplicationBuilder
            var builder = Host.CreateApplicationBuilder();

            // ============================================================
            // MODERN FLUENT REGISTRATION (Option 3)
            // ============================================================
            // Register Beep core services with full control over configuration
            // Using the fluent builder API - returns IBeepServiceBuilder
            builder.Services.AddBeepServices()
                .WithDirectory(AppContext.BaseDirectory)
                .WithAppRepo("Beep")
                .WithConfigType(BeepConfigType.Application)
                .WithMapping(true)
                .WithAssemblyLoading(true)
                .WithAssemblyHandler(AssemblyHandlerType.SharedContext) // Switch between Default and SharedContext
                .WithTimeout(TimeSpan.FromMinutes(5))
                .AsSingleton()
                .Build();

            // Register desktop-specific services
            builder.Services.AddRoutingServices()
                            .AddKeyHandling()
                            .AddAppManager()
                            .AddControlServices();

            // Register views and view models with automatic discovery
            builder.Services.AddViewModels()
                            .AddViews();
            // ============================================================

            // Build the host
            var host = builder.Build();

            // Configure services using the existing method
            BeepDesktopServices.ConfigureServices(host);

            // Add-in / menu / tree command wiring + SimpleItemFactory
            host.ConfigureBeepAddInUi();

            // Configure AppManager using configuration settings
            var config = UserSettingsManager.Configuration;

            // Configure AppManager
            BeepDesktopServices.AppManager.DialogManager = new BeepDialogManager();
            BeepDesktopServices.AppManager.Title = "Beep Data Management Platform";
            BeepDesktopServices.AppManager.WaitFormType = typeof(BeepWait);
            BeepDesktopServices.AppManager.IconUrl = "simpleinfoapps.ico";
            BeepDesktopServices.AppManager.LogoUrl = "simpleinfoapps.svg";
            BeepDesktopServices.AppManager.HomePageName = "MainFrm";
            BeepDesktopServices.AppManager.HomePageDescription = "homePageDescription";
            BeepDesktopServices.AppManager.Theme = "BrutalistTheme";
            BeepDesktopServices.AppManager.Style = FormStyle.Brutalist;


            // Set the theme and style before loading fonts

            BeepThemesManager.CurrentStyle = FormStyle.Brutalist;
            TheTechIdea.Beep.Winform.Controls.FontManagement.FontListHelper.EnsureFontsLoaded();

            // Subscribe to events for custom routes and resources
            SubscribeToBeepEvents();
            BeepDesktopServices.AppManager.DialogManager = new BeepDialogManager((Form)BeepDesktopServices.AppManager.MainDisplay);

            // Start loading with progress
            var result = BeepDesktopServices.StartLoading(
                new string[] { "BeepEnterprize", "TheTechIdea", "Beep" },
                showWaitForm: true);

            if (result.Flag == Errors.Ok)
            {
                Debug.WriteLine("Sample Business App - Loading completed successfully");
            }
            else
            {
                Debug.WriteLine($"Sample Business App - Loading failed: {result.Message}");
                MessageBox.Show($"Loading failed: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Cleanup configuration on error
                UserSettingsManager.Dispose();
                return;
            }

            // Show home page
            BeepDesktopServices.AppManager.ShowHome();

            // Cleanup configuration system
            UserSettingsManager.Dispose();

            // Dispose services when application exits
            BeepDesktopServices.DisposeServices();
            PaintersFactory.ClearCache();

            // Dispose the host
            host.Dispose();
            Application.Exit();
        }

        #region Subscribing to Events for Beep FrameWork to load images and font and register routes
        /// <summary>
        /// Subscribe to events before calling StartApp()
        /// </summary>
        private static void SubscribeToBeepEvents()
        {
            // Add custom routes for Sample Business App
            BeepDesktopServices.OnRegisterRoutes += (routingManager) =>
            {
                Debug.WriteLine("Registering Sample Business App routes...");
                RegisterStandardBeepRoutes(routingManager);
            };

            // Add custom graphics paths for Sample Business App
            BeepDesktopServices.OnLoadGraphics += (graphicsLocations) =>
            {
                Debug.WriteLine("Adding Sample Business App graphics paths...");
                var customPaths = new[]
                {
                    @".\SampleBusinessApp\Resources\Images",
                    @".\Resources\Images",
                    @"C:\SampleBusinessApp\Graphics"
                };

                foreach (var path in customPaths)
                {
                    if (Directory.Exists(path))
                    {
                        graphicsLocations.Add(path);
                        Debug.WriteLine($"Added Sample Business App graphics path: {path}");
                    }
                }
            };

            // Add custom font paths for Sample Business App
            BeepDesktopServices.OnLoadFonts += (fontLocations) =>
            {
                Debug.WriteLine("Adding Sample Business App font paths...");
                var customPaths = new[]
                {
                    @".\SampleBusinessApp\Resources\Fonts",
                    @".\Resources\Fonts",
                    @"C:\SampleBusinessApp\Fonts"
                };

                foreach (var path in customPaths)
                {
                    if (Directory.Exists(path))
                    {
                        fontLocations.Add(path);
                        Debug.WriteLine($"Added Sample Business App font path: {path}");
                    }
                }
            };
        }
        #endregion

        #region Routes Registration
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
                { "uc_DataConnections", "uc_DataConnections" },
                { "uc_NuggetsManageWizardLauncher_new", "uc_NuggetsManageWizardLauncher_new" },
                { "uc_ImportExportWizardLauncher", "uc_ImportExportWizardLauncher" }
            };

            foreach (var route in standardRoutes)
            {
                try
                {
                    var result = routingManager.RegisterRouteByName(route.Key, route.Value);
                    if (result.Flag == Errors.Ok)
                    {
                        Debug.WriteLine($"Successfully registered standard route: {route.Key}");
                    }
                    else
                    {
                        Debug.WriteLine($"Failed to register standard route {route.Key}: {result.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception registering standard route {route.Key}: {ex.Message}");
                }
            }
        }
        #endregion
    }
}
