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

            BeepProgram.RegisterGlobalKeyHandler();

            // Start the Application
            BeepProgram.StartLoading(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });
            BeepProgram.SetMainModule("Form1");
            BeepProgram.ShowMainPage();
            Application.Run(new Form1(BeepProgram.beepService));
            // Dispose Services
            BeepProgram.DisposeServices(host.Services);


            //var x = new Form1();
            //x.Theme = theme;
            //BeepSplashScreen beepSplashScreen = new BeepSplashScreen();
            //beepSplashScreen.Title = "Beep Splash Screen";
            //beepSplashScreen.Theme = theme;
            
            ////  beepSplashScreen.Message = "Loading...";
            //// set the logo image from resourses slack.svg
            ////      ImageTools.GetGraphicFilesLocationsFromEmbedded(new string[] { "System" });
            //beepSplashScreen.LogoPath = "WinFormsApp.UI.Test.gfx.slack.svg";
            //beepSplashScreen.ShowWithFadeIn();
            //Application.DoEvents();
            //Thread.Sleep(5000);
            //beepSplashScreen.HideWithFadeOut();
            //BeepWaitScreen beepWaitScreen = new BeepWaitScreen();
            //beepWaitScreen.Theme = theme;
            //beepWaitScreen.ShowAndRunAsync(async () =>
            //{
            //    await Task.Delay(5000);
            //});
            //Application.Run(x);

        }
    }
}