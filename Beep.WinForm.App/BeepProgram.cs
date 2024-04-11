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

namespace TheTechIdea.Beep.Container
{
    public static class BeepProgram
    {
        static Dictionary<KeyCombination, string> keyMapToFunction = new Dictionary<KeyCombination, string>
    {
        { new KeyCombination(Keys.C, true, false, false), "CopyFunction" },
        { new KeyCombination(Keys.F4, false, true, false), "CloseFunction" },
        { new KeyCombination(Keys.A, false, false, true), "SelectAllFunction" },
        // Add more mappings as needed
    };
        public static IVisManager visManager { get; set; }
        public static IBeepService beepService { get; set; }
        public static void RegisterGlobalKeyHandler( )
        {
            // Registering global key handler
            var globalKeyHandler = new GlobalKeyHandler();
            Application.AddMessageFilter(globalKeyHandler);
            globalKeyHandler.KeyDown += GlobalKeyDown;
        }
        private static void GlobalKeyDown(object sender, KeyEventArgs e)
        {
            var combination = new KeyCombination(e.KeyCode, e.Control, e.Alt, e.Shift);

            if (keyMapToFunction.TryGetValue(combination, out var functionName))
            {
                Console.WriteLine($"{functionName} triggered");
                // Implement your function call logic based on functionName
                e.Handled = true;
            }
        }
        public static void RegisterServices(HostApplicationBuilder builder)
        {
            // Register beep services
            builder.Services.RegisterBeep(AppContext.BaseDirectory, null, TheTechIdea.Util.BeepConfigType.Application, true);
            builder.Services.RegisterVisManager();

            // Python service registration
            string pythonHome = DeterminePythonHomePath();
            builder.Services.RegisterPythonService(pythonHome);
            builder.Services.RegisterPythonPackageManagerService(pythonHome);
            // Add additional service registrations here
        }
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

        public static void InitializeAndConfigureServices(IHost host)
        {
            ServiceHelper.Initialize(host.Services);
            // Extracted service retrieval and initial configuration into a separate method
             beepService = host.Services.GetService<IBeepService>()!;
             visManager = host.Services.GetService<IVisManager>()!;

            // Assuming these method calls setup and configure the services as necessary
            SetupVisManager( );
            SetupPythonRuntimeManager(host.Services);
            SetupPackageManagerViewModel(host.Services);
        }
        public static void SetupVisManager()
        {    // have to fo this , to work as crossplaform and Different UI
            visManager = new VisManager(beepService.DMEEditor);
            visManager.SetBeepReference(beepService);
            // Original logic for setting up visManager
           
           
        }

        public static void SetupPythonRuntimeManager(IServiceProvider services)
        {
            var pythonRunTimeManager = services.GetService<IPythonRunTimeManager>()!;
            // Setting up Python runtime manager with the necessary configurations
            pythonRunTimeManager.DMEditor = beepService.DMEEditor;
            // Additional setup as necessary
        }

        public static void SetupPackageManagerViewModel(IServiceProvider services)
        {
            var packageManagerViewModel = services.GetService<IPackageManagerViewModel>()!;
            // Configuring package manager view model
            packageManagerViewModel.Editor = beepService.DMEEditor;
            packageManagerViewModel.Init();
            // Add additional setup as required
        }




        public static void StartLoadingDataThenShowMainForm( string[] namespacestoinclude)
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

            var progress = new Progress<PassedArgs>(percent => {

                p.Messege = percent.Messege;
                visManager.PasstoWaitForm(p);
            });

            // Load Assemblies from folders (DataSources,Drivers, Extensions,...)
            visManager.LoadAssemblies(beepService, progress); //loading DLL using VisManager to show waiting form
                                                              // you can also load DLL using
                                                              // beepService.LoadAssemblies();
                                                              //but this will not show any waiting form

            LoadGraphics( namespacestoinclude);

            visManager.CloseWaitForm();
            // Show main Page
            visManager.ShowMainPage();
        }

        public static void LoadGraphics( string[] namespacestoinclude)
        {
            if (namespacestoinclude==null)
            {
                namespacestoinclude = new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" };
            }
                
            visManager.visHelper.GetGraphicFilesLocationsFromEmbedded(namespacestoinclude);
            visManager.visHelper.GetGraphicFilesLocations(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath);

        }
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
            
            visManager.Dispose();
            beepService.DMEEditor.Dispose();
            beepService?.Dispose();
            // Add additional dispose logic as necessary
        }
    }
}
