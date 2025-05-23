﻿using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Addin;
using Autofac;
using TheTechIdea.Beep.Winform.Controls.Helpers;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Vis.Modules;
using System.IO;
using System.Linq;

namespace WinFormsApp.UI.Test
{
    public static class BeepAppServices
    {
        /// <summary>
        /// Visualiztion Manager
        /// </summary>
        public static IAppManager visManager { get; set; }
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

            FontListHelper.GetFontFilesLocations(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath);
            FontListHelper.GetFontResourcesFromEmbedded(namespacestoinclude);
            // Add to the file extensions in GetFontFilesLocations
            string[] fontFiles = Directory.GetFiles(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath, "*.ttf")
                .Concat(Directory.GetFiles(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath, "*.otf"))
                .Concat(Directory.GetFiles(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath, "*.woff"))
                .Concat(Directory.GetFiles(beepService.DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath, "*.woff2")).ToArray();

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
            beepService.LoadAssemblies(progress);
            beepService.Config_editor.LoadedAssemblies = beepService.LLoader.Assemblies.Select(c => c.DllLib).ToList();

            // Load Assemblies from folders (DataSources,Drivers, Extensions,...)
          //  visManager.LoadAssemblies(beepService, progress); //loading DLL using VisManager to show waiting form
                                                              // you can also load DLL using
                                                              // beepService.LoadAssemblies();
                                                              //but this will not show any waiting form
            Task.Delay(1000).Wait();
            visManager.CloseWaitForm();
            Task.Delay(3000).Wait();
            // Load Configurations
            //  beepService.LoadConfigurations("Beep");

            // Load Environments
            //  beepService.LoadEnvironments();

            // Config main Page if you want ot use main page use in Beep Platform
            // AppManager.ShowMainPage();
        }
        public static void RegisterRoutes()
        {
            visManager.RoutingManager.RegisterRouteByName("MainFrm", "MainFrm");
            visManager.RoutingManager.RegisterRouteByName("uc_ConnnectionDrivers", "uc_ConnnectionDrivers");
            visManager.RoutingManager.RegisterRouteByName("uc_FilterForm", "uc_FilterForm");
            visManager.RoutingManager.RegisterRouteByName("uc_RDBMSConnections", "uc_RDBMSConnections");
            visManager.RoutingManager.RegisterRouteByName("uc_FileConnections", "uc_FileConnections");
            visManager.RoutingManager.RegisterRouteByName("uc_EntityEditor", "uc_EntityEditor");
            visManager.RoutingManager.RegisterRouteByName("uc_CreateLocalDB", "uc_CreateLocalDB");
            visManager.RoutingManager.RegisterRouteByName("uc_diagraming", "uc_diagraming");
            visManager.RoutingManager.RegisterRouteByName("uc_FunctiontoFunctionMapping", "uc_FunctiontoFunctionMapping");
            visManager.RoutingManager.RegisterRouteByName("uc_DataEdit", "uc_DataEdit");
            visManager.RoutingManager.RegisterRouteByName("uc_CopyEntities", "uc_CopyEntities");
            visManager.RoutingManager.RegisterRouteByName("uc_DataConnections", "uc_DataConnections");
            //uc_DataConnections
            //uc_CreateLocalDB uc_FileConnections
            //uc_CopyEntities
            //uc_LocalDBConnections
            //uc_DataEdit
            //uc_FunctiontoFunctionMapping

        }


    }
}
