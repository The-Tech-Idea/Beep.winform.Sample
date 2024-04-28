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
using TheTechIdea.Beep.Winform.Controls.FunctionsandExtensions;
using TheTechIdea.Beep.Winform.Controls.KeyManagement;

namespace TheTechIdea.Beep.Container
{
    public static class BeepProgram
    {
        /// <summary>
        /// Visualiztion Manager
        /// </summary>
        public static IVisManager visManager { get; set; }
        /// <summary>
        /// Beep Service
        /// </summary>
        public static IBeepService beepService { get; set; }
        private static IPythonRunTimeManager PythonRunTimeManager;
        private static bool IsReady = false;
        private static string Pythonruntimepath;
        private static IPackageManagerViewModel PackageManager;
        private static IPythonMLManager pythonMLManager;
        private static string PythonDataPath;
        /// <summary>
        /// Register Global Key Handler
        /// </summary>
        public static void RegisterGlobalKeyHandler()
        {
            // Registering global key handler
            KeyManager.RegisterGlobalKeyHandler(beepService.DMEEditor,visManager);
        }
        /// <summary>
        ///  Register Services
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterServices(HostApplicationBuilder builder)
        {
            // Register beep services
            builder.Services.RegisterBeep(AppContext.BaseDirectory, null, TheTechIdea.Util.BeepConfigType.Application, true);

            builder.Services.RegisterVisManager();

            // Python service registration
            Pythonruntimepath = DeterminePythonHomePath();
            builder.Services.RegisterPythonService(Pythonruntimepath);
            builder.Services.RegisterPythonPackageManagerService();
            builder.Services.RegisterPythonMLService();
            // Add additional service registrations here
        }
        /// <summary>
        /// Determine Python Home Path
        /// </summary>
        /// <returns>Python Home </returns>
        public static string DeterminePythonHomePath()
        {
            if (Directory.Exists(@"\\mvcsepimprod\DHUB\py\x64"))
            {
                return @"\\mvcsepimprod\DHUB\py\x64";
            }
            if (Directory.Exists(@"W:\\Cpython\\3.9\\x64"))
            {
                return @"W:\\Cpython\\3.9\\x64";
            }
            return string.Empty; // Consider a default or throw an exception if necessary
        }
       
        /// <summary>
        /// Initialize and Configure Services
        /// </summary>
        /// <param name="host"></param>
        public static void InitializeAndConfigureServices(IHost host)
        {
            ServiceHelper.Initialize(host.Services);
            // Extracted service retrieval and initial configuration into a separate method
            beepService = host.Services.GetService<IBeepService>()!;
            visManager = host.Services.GetService<IVisManager>()!;

            // Assuming these method calls setup and configure the services as necessary
            SetupVisManager();
            SetupPythonRuntimeManager(host.Services);
            SetupPackageManagerViewModel(host.Services);
            SetupPythonMLManagerViewModel(host.Services);
        }
        /// <summary>
        /// Setup Visulization Manager
        /// </summary>
        public static void SetupVisManager()
        {    // have to fo this , to work as crossplaform and Different UI
         //   visManager = new VisManager(beepService.DMEEditor);
          //  visManager.SetBeepReference(beepService);
            // Original logic for setting up visManager


        }
        /// <summary>
        /// Setup Python Runtime Manager
        /// </summary>
        /// <param name="services"></param>
        public static void SetupPythonRuntimeManager(IServiceProvider services)
        {
            PythonRunTimeManager = services.GetService<IPythonRunTimeManager>()!;
            IsReady = PythonRunTimeManager.Initialize(DeterminePythonHomePath(), BinType32or64.p395x64, @"lib\site-packages");
            PythonServices.PythonRunTimeManager = PythonRunTimeManager;
            PythonServices.Pythonruntimepath = Pythonruntimepath;
           
            // Setting up Python runtime manager with the necessary configurations
            // pythonRunTimeManager.DMEditor = beepService.DMEEditor;
            // Additional setup as necessary
        }
        /// <summary>
        /// Setup Package Manager View Model
        /// </summary>
        /// <param name="services"></param>
        public static void SetupPackageManagerViewModel(IServiceProvider services)
        {
            PackageManager = services.GetService<IPackageManagerViewModel>()!;
            // Configuring package manager view model
            PackageManager.Init();
            PythonServices.PackageManager = PackageManager;
            PythonServices.PackageManager= PackageManager;
            // Add additional setup as required
        }
        public static void SetupPythonMLManagerViewModel(IServiceProvider services)
        {
            PythonServices.PythonMLManager = services.GetService<IPythonMLManager>()!;
            // Add additional setup as required
        }
        /// <summary>
        /// Start Loading Data then Show Main Form
        /// </summary>
        /// <param name="namespacestoinclude"></param>
        public static void StartLoadingDataThenShowMainForm(string[] namespacestoinclude)
        {
            //Setting the Main Form 
            visManager.SetMainDisplay("Frm_Main", "Beep - The Data Plaform", "SimpleODM.ico", "", "", "");



            PassedArgs p = new PassedArgs();

            p.Messege = "Loading DLL's";
            // Show Wait Form
            visManager.ShowWaitForm(p);

            // Passing Message to WaitForm
            visManager.PasstoWaitForm(p);

            // Prepare Async Data Notification from Assembly loader to WaitForm

            var progress = new Progress<PassedArgs>(percent =>
            {

                p.Messege = percent.Messege;
                visManager.PasstoWaitForm(p);
            });

            // Load Assemblies from folders (DataSources,Drivers, Extensions,...)
            visManager.LoadAssemblies(beepService, progress); //loading DLL using VisManager to show waiting form
                                                              // you can also load DLL using
                                                              // beepService.LoadAssemblies();
                                                              //but this will not show any waiting form

            LoadGraphics(namespacestoinclude);

            visManager.CloseWaitForm();
            // Show main Page
            visManager.ShowMainPage();
        }
        /// <summary>
        /// Load Graphics
        /// </summary>
        /// <param name="namespacestoinclude"></param>
        public static void LoadGraphics(string[] namespacestoinclude)
        {
            if (namespacestoinclude == null)
            {
                namespacestoinclude = new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" };
            }

            visManager.visHelper.GetGraphicFilesLocationsFromEmbedded(namespacestoinclude);
            visManager.visHelper.GetGraphicFilesLocations(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath);

        }
        /// <summary>
        /// Dispose Services
        /// </summary>
        /// <param name="services"></param>
        public static void DisposeServices(IServiceProvider services)
        {

            var pythonRunTimeManager = services.GetService<IPythonRunTimeManager>();
            // Dispose logic for services
            var packageManagerViewModel = services.GetService<IPackageManagerViewModel>()!;
            if (packageManagerViewModel != null)
            {
                packageManagerViewModel.Dispose();
            }
            if (pythonRunTimeManager != null)
            {
                pythonRunTimeManager?.Dispose();
            }
            KeyManager.UnregisterGlobalKeyHandler();
            visManager.Dispose();
            beepService.DMEEditor.Dispose();
            beepService?.Dispose();
            // Add additional dispose logic as necessary
        }
    }
}
