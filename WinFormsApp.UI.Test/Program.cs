using Autofac;
//using Beep.Python.Model;
//using Beep.Python.RuntimeEngine.Helpers;
//using Beep.Python.RuntimeEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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


namespace WinFormsApp.UI.Test
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(int awareness);

        // DPI Awareness levels
        private const int PROCESS_DPI_UNAWARE = 0;
        private const int PROCESS_SYSTEM_DPI_AWARE = 1;
        private const int PROCESS_PER_MONITOR_DPI_AWARE = 2;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // CRITICAL: Set DPI awareness FIRST, before any Windows API calls
            SetDpiAwareness();


            // Configure high DPI settings
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            StartApp();
        }
        private static void SetDpiAwareness()
        {
            try
            {
                // Try the modern approach first (Windows 8.1+)
                if (Environment.OSVersion.Version >= new Version(6, 3))
                {
                    SetProcessDpiAwareness(PROCESS_PER_MONITOR_DPI_AWARE);
                }
                else if (Environment.OSVersion.Version >= new Version(6, 0))
                {
                    // Fallback for Windows Vista/7/8
                    SetProcessDPIAware();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DPI awareness setting failed: {ex.Message}");
                // Continue anyway - the manifest should handle it
            }
        }

        static string pythonRuntimePath= "C:\\Python311"; // Set your Python runtime path here
        private static void StartApp()
        {
            // Create Autofac ContainerBuilder
            var builder = new ContainerBuilder();
            
            // Register Beep Services with Autofac
            BeepServices.RegisterServices(builder);

            // Register Beep Winform Controls and Managers
            RegisterBeepWinformServices.RegisterControlManager(builder);
            // Looking for Python runtimes
           // PythonEnvironmentDiagnostics.PythonRunTimes=  PythonEnvironmentDiagnostics.GetPythonRuntimesInstallations();
           // if (PythonEnvironmentDiagnostics.PythonRunTimes.Count > 0)
           // {
           //     pythonRuntimePath= PythonEnvironmentDiagnostics.PythonRunTimes[0].RuntimePath;
           //     PythonServicesAutofac.RegisterPythonServices(builder, PythonEnvironmentDiagnostics.PythonRunTimes[0].RuntimePath);
           // }
           //// Build the Autofac container
           var container = builder.Build();

           // // if you want to use the Python runtime, you can initialize it here
           // if (PythonEnvironmentDiagnostics.PythonRunTimes.Count > 0)
           // {
           //     PythonServicesAutofac.ConfigureContainer(container);

           //     // Verify the path using diagnostics
           //     var diagnostics = PythonEnvironmentDiagnostics.RunFullDiagnostics(pythonRuntimePath);
           //     if (diagnostics.PythonFound)
           //     {
           //         // Initialize the runtime manager
           //         var manager = PythonServicesAutofac.GetPythonRunTimeManager();
           //         // You may need to pass additional config objects as required by Initialize
           //         manager.Initialize(pythonRuntimePath);
           //     }
           //     else
           //     {
           //         // Handle error: Python not found at the path
           //     }
           // }
            
            // Resolve and configure services
            BeepServices.ConfigureServices(container);
            BeepThemesManager.InitializeThemes();

            BeepServices.beepService.LoadServices();
            BeepServices.beepService.LoadHandlers();


            // Configure AppManager

            BeepServices.AppManager.Title = "Beep Data Management Platform";
            BeepServices.AppManager.Theme = "DefaultTheme";
            BeepServices.AppManager.WaitFormType = typeof(BeepWait);
            BeepServices.AppManager.IconUrl = "simpleinfoapps.ico";
            BeepServices.AppManager.LogoUrl = "simpleinfoapps.svg";
            BeepServices.AppManager.HomePageName = "MainFrm";
            BeepServices.AppManager.HomePageDescription = "homePageDescription";
          

            // Start the Application
            BeepAppServices.visManager = BeepServices.AppManager;
            BeepAppServices.beepService = BeepServices.beepService;
            BeepAppServices.StartLoading(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });
            BeepAppServices.RegisterRoutes();
            //Application.Run(new Form1());
            // Show the home page
            BeepServices.ShowHome();
          
            // Dispose services (if needed)
            BeepServices.DisposeServices();
        }

    }
}