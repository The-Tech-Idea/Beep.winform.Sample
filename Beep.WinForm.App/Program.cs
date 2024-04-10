using Beep.Vis.Module;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheTechIdea.Beep.Container;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Util;
using Beep.Python.RuntimeEngine;
using TheTechIdea.Beep.Winform.Extensions;
using TheTechIdea;
using Beep.Python.Model;
using TheTechIdea.Beep.Winform.Controls.Managers;


namespace Beep.Winform.App
{
    static class Program
    {
       
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

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
            BeepProgram.StartLoadingDataThenShowMainForm(new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" });

            // Dispose Services
            BeepProgram.DisposeServices(host.Services);

        }
       
    }
}
