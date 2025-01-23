using TheTechIdea.Beep.Vis.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Winform.Controls.KeyManagement;
using TheTechIdea.Beep.Addin;
//using TheTechIdea.Beep.Winform.Extensions;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Winform.Controls;


namespace TheTechIdea.Beep.Container
{
    public static class BeepProgram
    {
        /// <summary>
        /// Visualiztion Manager
        /// </summary>
        public static IAppManager visManager { get; set; }
        /// <summary>
        /// Beep Service
        /// </summary>
        public static IBeepService beepService { get; set; }
        private static bool IsReady = false;

        /// <summary>
        /// Register Global Key Handler
        /// </summary>
       
        /// <summary>
        ///  Register Services
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterServices(HostApplicationBuilder builder)
        {
            // Register beep services
            builder.Services.RegisterBeep(AppContext.BaseDirectory, null, BeepConfigType.Application, true);
            builder.Services.RegisterRouter();
            builder.Services.RegisterKeyHandler();
            builder.Services.RegisterViewModels();
            builder.Services.RegisterViews();
            builder.Services.RegisterAppManager();
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
            visManager = host.Services.GetService<IAppManager>()!;

            // Assuming these method calls setup and configure the services as necessary
            //Connect Winform Visula Manager to My Beep Service
            //if Web or other UI use the appropriate VisManager

           // SetupVisManager();
        }
       
        /// <summary>
        /// Start Loading Data then Config Main Form
        /// </summary>
        /// <param name="namespacestoinclude"></param>
        public static void StartLoading(string[] namespacestoinclude)
        {
            LoadGraphics(namespacestoinclude);
            //Setting the Main Form 
            //    visManager.SetMainDisplay("Form1", "Beep - The Data Plaform", "SimpleODM.ico", "", "", "");
            PassedArgs p = new PassedArgs();
            p.Messege = "Starting ....";
            // Config Wait Form
         //   p.Title = "Beep - The Data Platform";
         //   p.ImagePath = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.simpleinfoapps.svg";
            visManager.ShowWaitForm(p);
            Task.Delay(2000).Wait();
            p.Messege = "Starting ...........";
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
           
            visManager.CloseWaitForm();
            // Config main Page if you want ot use main page use in Beep Platform
            // visManager.ShowMainPage();
        }
        public static void RegisterRoutes()
        {
            visManager.RoutingManager.RegisterRouteByName("Form1", "Form1");
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
            // Load Graphics from Embedded Resources
            ImageListHelper.GetGraphicFilesLocationsFromEmbedded(namespacestoinclude);
            // Load Graphics from Folders
            ImageListHelper.GetGraphicFilesLocations(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath);

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
