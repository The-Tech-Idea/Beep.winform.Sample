using Microsoft.Extensions.Hosting;
using TheTechIdea.Beep.Container;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.Managers;


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
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            // Register Beep Services
            BeepProgram.RegisterServices(builder);
            // Register Other Services here

            using IHost host = builder.Build();

            // Retreiving Services and Configuring them
            BeepProgram.InitializeAndConfigureServices(host);


            host.Services.ConfigureAppManager(appManager =>
            {

                appManager.Title = "Beep Data Managment Platform";
                appManager.Theme = EnumBeepThemes.CandyTheme;
                appManager.WaitFormType = typeof(BeepWait);
                appManager.DMEEditor.ConfigEditor.Config.SystemEntryFormName = "Form1";
                appManager.IconUrl = "simpleinfoapps.ico";
                appManager.LogoUrl = "simpleinfoapps.svg";
                appManager.HomePageName = "Form1";
    
                appManager.HomePageDescription = "homePageDescription";
            });
            // Start the Application
            BeepProgram.StartLoading(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });
            BeepProgram.RegisterRoutes();
            
           
            host.Services.ShowHome();
            //  Application.Run(new Form1(BeepProgram.beepService));
            // Dispose Services
            BeepProgram.DisposeServices(host.Services);


           
        }
    }
}