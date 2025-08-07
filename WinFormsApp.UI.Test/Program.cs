using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.Helpers;
using TheTechIdea.Beep.Winform.Controls.Integrated;
using TheTechIdea.Beep.Winform.Default.Views;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.ConfigUtil;

namespace WinFormsApp.UI.Test
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // CRITICAL: Set DPI awareness FIRST, before any Windows API calls
            RegisterBeepWinformServices.SetHighDpiMode();

            StartApp();
        }

        private static void StartApp()
        {
            // Create HostApplicationBuilder
            var builder = Host.CreateApplicationBuilder();

            // Register Beep Services using the existing method
            BeepDesktopServices.RegisterServices(builder);

            // Register Beep Winform Controls and Managers
            RegisterBeepWinformServices.RegisterDialogManager(builder.Services);

            // Build the host
            var host = builder.Build();

            // Configure services using the existing method
            BeepDesktopServices.ConfigureServices(host);
            BeepDesktopServices.ConfigureControlsandMenus();

            // Configure AppManager (exact same configuration)
            BeepDesktopServices.AppManager.Title = "Beep Data Management Platform";
            BeepDesktopServices.AppManager.Theme = "DefaultTheme";
            BeepDesktopServices.AppManager.WaitFormType = typeof(BeepWait);
            BeepDesktopServices.AppManager.IconUrl = "simpleinfoapps.ico";
            BeepDesktopServices.AppManager.LogoUrl = "simpleinfoapps.svg";
            BeepDesktopServices.AppManager.HomePageName = "MainFrm";
            BeepDesktopServices.AppManager.HomePageDescription = "homePageDescription";
            
            // Subscribe to events for custom routes and resources
   
            SubscribeToBeepEvents();
        
            
       
            var result = BeepDesktopServices.StartLoading(new string[] { "BeepEnterprize", "TheTechIdea", "Beep" }, showWaitForm: true);
            
            if (result.Flag == Errors.Ok)
            {
                Debug.WriteLine("3 - Loading completed successfully");
            }
            else
            {
                Debug.WriteLine($"3 - Loading failed: {result.Message}");
                MessageBox.Show($"Loading failed: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            BeepDesktopServices.AppManager.ShowHome();
            
            // Dispose services when application exits
            BeepDesktopServices.DisposeServices();

            // Dispose the host
            host.Dispose();
        }

        /// <summary>
        /// Subscribe to events before calling StartApp()
        /// </summary>
        private static void SubscribeToBeepEvents()
        {
            // Add custom routes
            BeepDesktopServices.OnRegisterRoutes += (routingManager) =>
            {
                Debug.WriteLine("Registering custom routes...");
                routingManager.RegisterRouteByName("MainFrm", "MainFrm");
                routingManager.RegisterRouteByName("uc_ConnnectionDrivers", "uc_ConnnectionDrivers");
                routingManager.RegisterRouteByName("uc_FilterForm", "uc_FilterForm");
                routingManager.RegisterRouteByName("uc_RDBMSConnections", "uc_RDBMSConnections");
                routingManager.RegisterRouteByName("uc_FileConnections", "uc_FileConnections");
                routingManager.RegisterRouteByName("uc_EntityEditor", "uc_EntityEditor");
                routingManager.RegisterRouteByName("uc_CreateLocalDB", "uc_CreateLocalDB");
                routingManager.RegisterRouteByName("uc_diagraming", "uc_diagraming");
                routingManager.RegisterRouteByName("uc_FunctiontoFunctionMapping", "uc_FunctiontoFunctionMapping");
                routingManager.RegisterRouteByName("uc_DataEdit", "uc_DataEdit");
                routingManager.RegisterRouteByName("uc_CopyEntities", "uc_CopyEntities");
                routingManager.RegisterRouteByName("uc_DataConnections", "uc_DataConnections");
            };

            // Add custom graphics paths
            BeepDesktopServices.OnLoadGraphics += (graphicsLocations) =>
            {
                Debug.WriteLine("Adding custom graphics paths...");
                // Only add paths that actually exist to avoid errors
                var customPaths = new[] { @"C:\MyApp\Graphics", @".\Resources\Images" };
                foreach (var path in customPaths)
                {
                    if (Directory.Exists(path))
                    {
                        graphicsLocations.Add(path);
                        Debug.WriteLine($"Added graphics path: {path}");
                    }
                }
            };

            // Add custom font paths
            BeepDesktopServices.OnLoadFonts += (fontLocations) =>
            {
                Debug.WriteLine("Adding custom font paths...");
                // Only add paths that actually exist to avoid errors
                var customPaths = new[] { @"C:\MyApp\Fonts", @".\Resources\Fonts" };
                foreach (var path in customPaths)
                {
                    if (Directory.Exists(path))
                    {
                        fontLocations.Add(path);
                        Debug.WriteLine($"Added font path: {path}");
                    }
                }
            };
        }
    }
}