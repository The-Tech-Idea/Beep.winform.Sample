
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
        

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // CRITICAL: Set DPI awareness FIRST, before any Windows API calls
            RegisterBeepWinformServices.SetHighDpiMode();


            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
           

            StartApp();
        }
       

        static string pythonRuntimePath= "C:\\Python311"; // Set your Python runtime path here
       
        private static void StartApp()
        {
            // Create HostApplicationBuilder
            var builder = Host.CreateApplicationBuilder();

            // Register Beep Services using the existing method
            BeepServices.RegisterServices(builder);

            // Register Beep Winform Controls and Managers
            RegisterBeepWinformServices.RegisterDialogManager(builder.Services);

           
            // Build the host
            var host = builder.Build();

            // Configure services using the existing method
            BeepServices.ConfigureServices(host);
            BeepThemesManager.InitializeThemes();
            BeepServices.beepService.LoadServices();
            BeepServices.beepService.LoadHandlers();
            // Configure AppManager (exact same configuration)
            BeepServices.AppManager.Title = "Beep Data Management Platform";
            BeepServices.AppManager.Theme = "DefaultTheme";
            BeepServices.AppManager.WaitFormType = typeof(BeepWait);
            BeepServices.AppManager.IconUrl = "simpleinfoapps.ico";
            BeepServices.AppManager.LogoUrl = "simpleinfoapps.svg";
            BeepServices.AppManager.HomePageName = "MainFrm";
            BeepServices.AppManager.HomePageDescription = "homePageDescription";

            // Start the Application (exact same)
            BeepAppServices.visManager = BeepServices.AppManager;
            BeepAppServices.beepService = BeepServices.beepService;
            BeepAppServices.beepService.vis = BeepAppServices.visManager;
            BeepAppServices.StartLoading(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });
            BeepAppServices.RegisterRoutes();


           
            // Show the home page
            BeepServices.ShowHome();

            // Keep the application running
          //  Application.Run();

            // Dispose services when application exits
            BeepServices.DisposeServices();

            // Dispose the host
            host.Dispose();
        }
    }
}