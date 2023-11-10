
using BeepEnterprize.Vis.Module;
using BeepEnterprize.Winform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheTechIdea.Beep.Container;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Winform.Extensions;
using TheTechIdea.Util;

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

            builder.Services.RegisterBeep(AppContext.BaseDirectory, null, TheTechIdea.Util.BeepConfigType.Application);
            builder.Services.RegisterVisManager();
            // Register Other Services here

            using IHost host = builder.Build();

            // Retreiving Services
            ServiceHelper.Initialize(host.Services);
            IBeepService beepService = host.Services.GetService<IBeepService>()!;
            IVisManager visManager = host.Services.GetService<IVisManager>()!;

            //Setting the Main Form 
            visManager.SetMainDisplay("Frm_Main", "Beep - The Data Plaform", "SimpleODM.ico", "","","");

            // Adding Required Configurations
            beepService.AddAllDataSourceMappings();
            beepService.AddAllConnectionConfigurations();
            beepService.AddAllDataSourceQueryConfigurations();


            // Load Assemblies from folders (DataSources,Drivers, Extensions,...)
            visManager.LoadAssemblies(beepService); //loading DLL using VisManager to show waiting form
                                                    // you can also load DLL using
                                                    // beepService.LoadAssemblies();
                                                    //but this will not show any waiting form

            // have to fo this , to work as crossplaform and Different UI
            visManager.SetBeepReference(beepService);


            // Load extra grahics files and icons
            visManager.visHelper.GetGraphicFilesLocations(null, true);
            visManager.visHelper.GetGraphicFilesLocations(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath, false);

            // Here you can
            // Initialize Your Services and Database Connections
            //IDhubMainConfig dhubMainConfig = host.Services.GetService<IDhubMainConfig>()!;
            //dhubMainConfig.InitService(false, false);


            // Show main Page
            visManager.ShowMainPage();

            // Dispose all objects after closing application
            beepService.vis.Dispose();
            beepService.DMEEditor.Dispose();
            beepService = null;

        }
    }
}
