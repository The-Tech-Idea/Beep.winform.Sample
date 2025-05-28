using Autofac;
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
     
        private static void StartApp()
        {
            // Create Autofac ContainerBuilder
            var builder = new ContainerBuilder();
            
            // Register Beep Services with Autofac
            BeepServices.RegisterServices(builder);
            RegisterBeepWinformServices.RegisterControlManager(builder);
            PythonServicesAutofac.RegisterPythonServices(builder, "C:\\Python311");

            // Build the Autofac container
            var container = builder.Build();

            // Resolve and configure services
            BeepServices.ConfigureServices(container);

            BeepAppServices.visManager = BeepServices.AppManager;
            BeepAppServices.beepService = BeepServices.beepService;
            BeepAppServices.beepService.LoadServices();
            BeepAppServices.beepService.LoadHandlers();
            // Configure AppManager

            BeepServices.AppManager.Title = "Beep Data Management Platform";
            BeepServices.AppManager.Theme = EnumBeepThemes.DefaultTheme;
            BeepServices.AppManager.WaitFormType = typeof(BeepWait);
            BeepServices.AppManager.IconUrl = "simpleinfoapps.ico";
            BeepServices.AppManager.LogoUrl = "simpleinfoapps.svg";
            BeepServices.AppManager.HomePageName = "MainFrm";
            BeepServices.AppManager.HomePageDescription = "homePageDescription";
       //     BeepServicesRegisterAutFac.AppManager.Tree = (IBeepUIComponent)container.Resolve<ITree>();
            // Start the Application
            
            BeepAppServices.StartLoading(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });
            BeepAppServices.RegisterRoutes();

            // Show the home page
            BeepServices.ShowHome();

            // Dispose services (if needed)
            BeepServices.DisposeServices();
        }

    }
}