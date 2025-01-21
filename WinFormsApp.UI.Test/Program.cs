using Microsoft.Extensions.Hosting;
using TheTechIdea.Beep.Container;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;


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

       

            // Start the Application
            BeepProgram.StartLoading(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });
            BeepProgram.RegisterRoutes();
            BeepProgram.SetMainModule("Form1");
            BeepProgram.ShowMainPage();
          //  Application.Run(new Form1(BeepProgram.beepService));
            // Dispose Services
            BeepProgram.DisposeServices(host.Services);


           
        }
    }
}