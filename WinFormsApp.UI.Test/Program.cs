using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.Helpers;



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

        //    StartAppUsingMicroSoft();
            StartAppUsingAutoFac();
        }
        private static void StartAppUsingMicroSoft()
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            // Register Beep Services
            BeepServicesRegister.RegisterServices(builder);
            // Register Other Services here

            using IHost host = builder.Build();

            // Retreiving Services and Configuring them
            BeepServicesRegister.ConfigureServices(host);


            host.Services.ConfigureAppManager(appManager =>
            {

                appManager.Title = "Beep Data Managment Platform";
                appManager.Theme = EnumBeepThemes.CandyTheme;
                //appManager.WaitFormType = typeof(BeepWait);
                appManager.IconUrl = "simpleinfoapps.ico";
                appManager.LogoUrl = "simpleinfoapps.svg";
                appManager.HomePageName = "Form1";

                appManager.HomePageDescription = "homePageDescription";
            });
            // Start the Application
            BeepAppServices.visManager = BeepAppServices.visManager ?? host.Services.GetService<TheTechIdea.Beep.Vis.Modules.IAppManager>();
            BeepAppServices.beepService = BeepAppServices.beepService ?? host.Services.GetService<IBeepService>();
            BeepAppServices.StartLoading(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });
            BeepAppServices.RegisterRoutes();


            BeepServicesRegister.ShowHome();
            //  Application.Run(new Form1(BeepProgram.beepService));
            // Dispose Services
            BeepServicesRegister.DisposeServices(host.Services);
        }
        private static void StartAppUsingAutoFac()
        {
            // Create Autofac ContainerBuilder
            var builder = new ContainerBuilder();
            
            // Register Beep Services with Autofac
            BeepServicesRegisterAutFac.RegisterServices(builder);
            RegisterBeepWinformServices.RegisterControlManager(builder);
          //  RegisterBeepWinformServices.RegisterTreeControl(builder);
            //// Register all types in the executing assembly that implement IFunctionExtension.
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //       .Where(t => typeof(IFunctionExtension).IsAssignableFrom(t))
            //       .As<IFunctionExtension>();
            // Register Other Services here (if any)

            // Build the Autofac container
            var container = builder.Build();

            // Resolve and configure services
            BeepServicesRegisterAutFac.ConfigureServices(container);

            BeepAppServices.visManager = BeepServicesRegisterAutFac.AppManager;
            BeepAppServices.beepService = BeepServicesRegisterAutFac.beepService;

            // Configure AppManager
        
            BeepServicesRegisterAutFac.AppManager.Title = "Beep Data Management Platform";
            BeepServicesRegisterAutFac.AppManager.Theme = EnumBeepThemes.DefaultTheme;
            BeepServicesRegisterAutFac.AppManager.WaitFormType = typeof(BeepWait);
            BeepServicesRegisterAutFac.AppManager.IconUrl = "simpleinfoapps.ico";
            BeepServicesRegisterAutFac.AppManager.LogoUrl = "simpleinfoapps.svg";
            BeepServicesRegisterAutFac.AppManager.HomePageName = "MainFrm";
            BeepServicesRegisterAutFac.AppManager.HomePageDescription = "homePageDescription";
       //     BeepServicesRegisterAutFac.AppManager.Tree = (IBeepUIComponent)container.Resolve<ITree>();
            // Start the Application
            
            BeepAppServices.StartLoading(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });
            BeepAppServices.RegisterRoutes();
         
            // Show the home page
            BeepServicesRegisterAutFac.ShowHome();

            // Dispose services (if needed)
            BeepServicesRegisterAutFac.DisposeServices();
        }

    }
}