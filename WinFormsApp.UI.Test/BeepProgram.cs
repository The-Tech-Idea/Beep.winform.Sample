using TheTechIdea.Beep.Vis.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Winform.Controls.KeyManagement;
using TheTechIdea.Beep.Addin;


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
        private static bool IsReady = false;

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
            builder.Services.RegisterBeep(AppContext.BaseDirectory, null, BeepConfigType.Application, true);
            builder.Services.RegisterVisManager();
            // Add additional service registrations here
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
            //Connect Winform Visula Manager to My Beep Service
            //if Web or other UI use the appropriate VisManager

            SetupVisManager();
        }
        /// <summary>
        /// Setup Visulization Manager
        /// </summary>
        public static void SetupVisManager()
        {    // have to fo this , to work as crossplaform and Different UI
         //   visManager = new VisManager(beepService.DMEEditor);
           
            // Original logic for setting up visManager
        }
        /// <summary>
      
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
            // Show main Page if you want ot use main page use in Beep Platform
            // visManager.ShowMainPage();
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
        /// Load Classes That Implement IBeepViewModel
        /// </summary>
        /// <param name="namespacestoinclude"></param>
        public static void LoadViewModels()
        {
           
           
        }
        /// <summary>
        /// Dispose Services
        /// </summary>
        /// <param name="services"></param>
        public static void DisposeServices(IServiceProvider services)
        {
            KeyManager.UnregisterGlobalKeyHandler();
            visManager.Dispose();
            beepService.DMEEditor.Dispose();
            beepService?.Dispose();
            // Add additional dispose logic as necessary
        }
    }
}
