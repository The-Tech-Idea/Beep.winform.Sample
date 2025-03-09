using TheTechIdea.Beep.Vis.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Utilities;

using TheTechIdea.Beep.Addin;
//using TheTechIdea.Beep.Winform.Extensions;
using TheTechIdea.Beep.Desktop.Common;

using Autofac;
using TheTechIdea.Beep.Container;


namespace WinFormsApp.UI.Test
{
    public static class BeepAppServices
    {
        /// <summary>
        /// Visualiztion Manager
        /// </summary>
        public static TheTechIdea.Beep.Vis.Modules.IAppManager visManager { get; set; }
        /// <summary>
        /// Beep Service
        /// </summary>
        public static IBeepService beepService { get; set; }
      
        /// <summary>
        /// Start Loading Data then Config Main Form
        /// </summary>
        /// <param name="namespacestoinclude"></param>
        public static void StartLoading(string[] namespacestoinclude)
        {
            if (namespacestoinclude == null)
            {
                namespacestoinclude = new string[3] { "BeepEnterprize", "TheTechIdea", "Beep" };
            }
            // Load Graphics from Embedded Resources
            ImageListHelper.GetGraphicFilesLocationsFromEmbedded(namespacestoinclude);
            // Load Graphics from Folders
            ImageListHelper.GetGraphicFilesLocations(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath);

            //Setting the Main Form 
            //    AppManager.SetMainDisplay("Form1", "Beep - The Data Plaform", "SimpleODM.ico", "", "", "");
            PassedArgs p = new PassedArgs();
            p.Messege = "Starting ....";
            // Config Wait Form
         //   p.Title = "Beep - The Data Platform";
         //   p.ImagePath = "TheTechIdea.Beep.Winform.Controls.GFX.SVG.simpleinfoapps.svg";
            visManager.ShowWaitForm(p);
            Task.Delay(1000).Wait();
            p.Messege = "Starting ...........";
            // Passing Message to WaitForm
            visManager.PasstoWaitForm(p);
            // Prepare Async Data Notification from Assembly loader to WaitForm
            var progress = new Progress<PassedArgs>(percent =>
            {
                Task.Delay(1000).Wait();
                p.Messege = percent.Messege;
                visManager.PasstoWaitForm(p);
            });
            Task.Delay(1000).Wait();
            // Load Assemblies from folders (DataSources,Drivers, Extensions,...)
            visManager.LoadAssemblies(beepService, progress); //loading DLL using VisManager to show waiting form
                                                              // you can also load DLL using
                                                              // beepService.LoadAssemblies();
                                                              //but this will not show any waiting form
            Task.Delay(1000).Wait();
            visManager.CloseWaitForm();
            Task.Delay(3000).Wait();
            // Config main Page if you want ot use main page use in Beep Platform
            // AppManager.ShowMainPage();
        }
        public static void RegisterRoutes()
        {
            visManager.RoutingManager.RegisterRouteByName("MainForm", "MainForm");
            visManager.RoutingManager.RegisterRouteByName("uc_ConnnectionDrivers", "uc_ConnnectionDrivers");
            visManager.RoutingManager.RegisterRouteByName("uc_FilterForm", "uc_FilterForm");
        }
       
       
    }
}
