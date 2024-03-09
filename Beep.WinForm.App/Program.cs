using BeepEnterprize.Vis.Module;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheTechIdea.Beep.Container;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Util;
using Beep.Python.RuntimeEngine;
using TheTechIdea.Beep.Winform.Extensions;
using TheTechIdea;
using Beep.Python.Model;

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

            string pythonhome = string.Empty;
            if (Directory.Exists(@"\\mvcsepimprod\DHUB\py\x64"))
            {
                pythonhome = @"\\mvcsepimprod\DHUB\py\x64";
            }
            if (Directory.Exists(@"W:\\Cpython\\3.9\\x64"))
            {
                pythonhome = @"W:\\Cpython\\3.9\\x64";
            }


            builder.Services.RegisterPythonService(pythonhome);//C:\Users\f_ald\source\repos\The-Tech-Idea\Beep.Python\PythonRuntime\3.9\x64"
                                                               // Register Other Services here

            using IHost host = builder.Build();

            // Retreiving Services
            ServiceHelper.Initialize(host.Services);
            IBeepService beepService = host.Services.GetService<IBeepService>()!;
            IVisManager visManager = host.Services.GetService<IVisManager>()!;


            // SetUp Python runtime engine
            IPythonRunTimeManager PythonRunTimeManager = host.Services.GetService<IPythonRunTimeManager>()!;
            PythonRunTimeManager.DMEditor= beepService.DMEEditor;

      


            //Setting the Main Form 
            visManager.SetMainDisplay("Frm_Main", "Beep - The Data Plaform", "SimpleODM.ico", "","","");

            // Adding Required Configurations
            beepService.DMEEditor.ConfigEditor.DataDriversClasses.Clear();
            beepService.AddAllDataSourceMappings();
            beepService.AddAllConnectionConfigurations();
            beepService.AddAllDataSourceQueryConfigurations();
            beepService.DMEEditor.ConfigEditor.LoadDataConnectionsValues();

            PassedArgs p = new PassedArgs();

            p.Messege = "Loading DLL's";
            // Show Wait Form
            visManager.ShowWaitForm(p);

            // Passing Message to WaitForm
            visManager.PasstoWaitForm(p);

            // Prepare Async Data Notification from Assembly loader to WaitForm

            var progress = new Progress<PassedArgs>(percent => {

                p.Messege = percent.Messege;
                visManager.PasstoWaitForm(p);
            });

            // Load Assemblies from folders (DataSources,Drivers, Extensions,...)
            visManager.LoadAssemblies(beepService, progress); //loading DLL using VisManager to show waiting form
                                                              // you can also load DLL using
                                                              // beepService.LoadAssemblies();
                                                              //but this will not show any waiting form

            // have to fo this , to work as crossplaform and Different UI
            visManager.SetBeepReference(beepService);


            // Load extra grahics files and icons
            string[] namespacestoinclude = { "BeepEnterprize", "TheTechIdea", "Beep" };
            visManager.visHelper.GetGraphicFilesLocationsFromEmbedded(namespacestoinclude);
            visManager.visHelper.GetGraphicFilesLocations(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath);


 
            visManager.CloseWaitForm();
            // Show main Page
            visManager.ShowMainPage();

            // Dispose all objects after closing application
            beepService.vis.Dispose();
            beepService.DMEEditor.Dispose();
            PythonRunTimeManager.Dispose();
            //packageManagerViewModel.Dispose();
            beepService = null;

        }
    }
}
