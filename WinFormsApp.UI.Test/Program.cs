using Autofac;
using Beep.Python.Model;
using Beep.Python.RuntimeEngine.Helpers;
using Beep.Python.RuntimeEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.Helpers;
using TheTechIdea.Beep.Winform.Controls.Integrated;


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
            //EnumBeepThemes theme = EnumBeepThemes.ZenTheme;
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

        
            StartApp();
        }
        static string pythonRuntimePath= "C:\\Python311"; // Set your Python runtime path here
        private static void StartApp()
        {
            // Create Autofac ContainerBuilder
            var builder = new ContainerBuilder();
            
            // Register Beep Services with Autofac
            BeepServices.RegisterServices(builder);
            RegisterBeepWinformServices.RegisterControlManager(builder);
             List<PythonRunTime> runtimes=  PythonEnvironmentDiagnostics.GetPythonRuntimesInstallations();
            if (runtimes.Count > 0)
            {
                PythonServicesAutofac.RegisterPythonServices(builder, runtimes[0].RuntimePath);
            }
           // Build the Autofac container
            var container = builder.Build();
            if (runtimes.Count > 0)
            {
                PythonServicesAutofac.ConfigureContainer(container);

                // Verify the path using diagnostics
                var diagnostics = PythonEnvironmentDiagnostics.RunFullDiagnostics(pythonRuntimePath);
                if (diagnostics.PythonFound)
                {
                    // Initialize the runtime manager
                    var manager = PythonServicesAutofac.GetPythonRunTimeManager();
                    // You may need to pass additional config objects as required by Initialize
                    manager.Initialize(pythonRuntimePath);
                }
                else
                {
                    // Handle error: Python not found at the path
                }
            }
            
            // Resolve and configure services
            BeepServices.ConfigureServices(container);
            BeepThemesManager.InitializeThemes();
   //         BeepThemesManager.AddPredefinedThemes();
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
            Console.WriteLine(BeepThemesManager._themes.Count());
            // Show the home page
            BeepServices.ShowHome();

            // Dispose services (if needed)
            BeepServices.DisposeServices();
        }

    }
}