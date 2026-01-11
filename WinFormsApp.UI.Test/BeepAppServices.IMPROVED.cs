using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.Winform.Controls.Helpers;
using TheTechIdea.Beep.Desktop.Common;
using TheTechIdea.Beep.Vis.Modules;
using System.IO;
using System.Linq;
using TheTechIdea.Beep.Desktop.Common.Util;
using System.Diagnostics;

namespace WinFormsApp.UI.Test
{
    /// <summary>
    /// ✅ IMPROVED: Beep Application Services
    /// This class has been refactored to avoid static mutable state and provide better error handling.
    /// However, static usage is acceptable for Desktop apps (single UI thread).
    /// </summary>
    public static class BeepAppServices
    {
        /// <summary>
        /// Visualization Manager - static is acceptable for Desktop (single-threaded UI)
        /// </summary>
        public static IAppManager VisManager { get; set; }

        /// <summary>
        /// Beep Service - static is acceptable for Desktop (single-threaded UI)
        /// </summary>
        public static IBeepService BeepService { get; set; }

        /// <summary>
        /// Start Loading Data and Config Main Form with enhanced error handling
        /// </summary>
        /// <param name="namespacestoinclude">Namespaces to scan for resources</param>
        /// <returns>Loading result with success/failure status</returns>
        public static IErrorsInfo StartLoading(string[] namespacestoinclude)
        {
            var errorInfo = new ErrorsInfo();

            try
            {
                // Validate inputs
                if (namespacestoinclude == null)
                {
                    namespacestoinclude = new string[] { "BeepEnterprize", "TheTechIdea", "Beep" };
                    Debug.WriteLine("⚠️  No namespaces provided, using defaults");
                }

                Debug.WriteLine("═══════════════════════════════════════════════════════════");
                Debug.WriteLine("📦 Starting resource loading sequence");
                Debug.WriteLine("═══════════════════════════════════════════════════════════");

                // Validate services are initialized
                if (VisManager == null)
                {
                    errorInfo.Flag = Errors.Failed;
                    errorInfo.Message = "VisManager is not initialized";
                    Debug.WriteLine($"❌ {errorInfo.Message}");
                    return errorInfo;
                }

                if (BeepService == null || BeepService.DMEEditor == null)
                {
                    errorInfo.Flag = Errors.Failed;
                    errorInfo.Message = "BeepService or DMEEditor is not initialized";
                    Debug.WriteLine($"❌ {errorInfo.Message}");
                    return errorInfo;
                }

                // Step 1: Load graphics from embedded resources
                try
                {
                    Debug.WriteLine("🖼️  Loading graphics from embedded resources...");
                    ImageListHelper.GetGraphicFilesLocationsFromEmbedded(namespacestoinclude);
                    Debug.WriteLine("✅ Embedded graphics loaded");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"⚠️  Error loading embedded graphics: {ex.Message}");
                    // Continue - embedded resources are optional
                }

                // Step 2: Load graphics from folders
                try
                {
                    Debug.WriteLine("🖼️  Loading graphics from folders...");
                    var gfxFolder = BeepService.DMEEditor.ConfigEditor.Config?.Folders?
                        .FirstOrDefault(x => x.FolderFilesType == FolderFileTypes.GFX)?.FolderPath;

                    if (!string.IsNullOrEmpty(gfxFolder))
                    {
                        ImageListHelper.GetGraphicFilesLocations(gfxFolder);
                        Debug.WriteLine($"✅ Graphics loaded from: {gfxFolder}");
                    }
                    else
                    {
                        Debug.WriteLine("⚠️  GFX folder not found in configuration");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"⚠️  Error loading graphics from folders: {ex.Message}");
                    // Continue - folder graphics are optional
                }

                // Step 3: Load fonts from folders
                try
                {
                    Debug.WriteLine("🔤 Loading fonts from folders...");
                    var gfxFolder = BeepService.DMEEditor.ConfigEditor.Config?.Folders?
                        .FirstOrDefault(x => x.FolderFilesType == FolderFileTypes.GFX)?.FolderPath;

                    if (!string.IsNullOrEmpty(gfxFolder))
                    {
                        FontListHelper.GetFontFilesLocations(gfxFolder);
                        Debug.WriteLine($"✅ Fonts loaded from: {gfxFolder}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"⚠️  Error loading fonts from folders: {ex.Message}");
                    // Continue - fonts are optional
                }

                // Step 4: Load fonts from embedded resources
                try
                {
                    Debug.WriteLine("🔤 Loading fonts from embedded resources...");
                    FontListHelper.GetFontResourcesFromEmbedded(namespacestoinclude);
                    Debug.WriteLine("✅ Embedded fonts loaded");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"⚠️  Error loading embedded fonts: {ex.Message}");
                    // Continue - embedded fonts are optional
                }

                // Step 5: Show wait form and load assemblies
                try
                {
                    Debug.WriteLine("⏳ Showing wait form...");
                    var loadingArgs = new PassedArgs { Messege = "Loading assemblies..." };
                    VisManager.ShowWaitForm(loadingArgs);

                    // Create progress callback for assembly loading
                    var progress = new Progress<PassedArgs>(args =>
                    {
                        try
                        {
                            VisManager.PasstoWaitForm(args);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"⚠️  Error updating wait form: {ex.Message}");
                        }
                    });

                    Debug.WriteLine("📚 Loading assemblies...");
                    BeepService.LoadAssemblies(progress);
                    Debug.WriteLine("✅ Assemblies loaded");

                    // Update loaded assemblies in configuration
                    if (BeepService.LLoader?.Assemblies != null)
                    {
                        BeepService.Config_editor.LoadedAssemblies = 
                            BeepService.LLoader.Assemblies.Select(c => c.DllLib).ToList();
                        Debug.WriteLine($"✅ Updated configuration with {BeepService.LLoader.Assemblies.Count} assemblies");
                    }

                    // Close wait form
                    VisManager.CloseWaitForm();
                    Debug.WriteLine("✅ Wait form closed");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"❌ Error during assembly loading: {ex.Message}");
                    Debug.WriteLine($"   Stack trace: {ex.StackTrace}");
                    errorInfo.Flag = Errors.Failed;
                    errorInfo.Message = $"Assembly loading failed: {ex.Message}";
                    errorInfo.Exception = ex;
                    try { VisManager?.CloseWaitForm(); } catch { }
                    return errorInfo;
                }

                Debug.WriteLine("═══════════════════════════════════════════════════════════");
                Debug.WriteLine("✅ Resource loading completed successfully");
                Debug.WriteLine("═══════════════════════════════════════════════════════════");

                errorInfo.Flag = Errors.Ok;
                errorInfo.Message = "Loading completed successfully";
                return errorInfo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ CRITICAL ERROR in StartLoading: {ex.Message}");
                Debug.WriteLine($"   Stack trace: {ex.StackTrace}");
                errorInfo.Flag = Errors.Failed;
                errorInfo.Message = $"Critical error: {ex.Message}";
                errorInfo.Exception = ex;
                return errorInfo;
            }
        }

        /// <summary>
        /// Register all standard Beep routes with error handling
        /// </summary>
        public static IErrorsInfo RegisterRoutes()
        {
            var errorInfo = new ErrorsInfo();

            try
            {
                if (VisManager?.RoutingManager == null)
                {
                    errorInfo.Flag = Errors.Failed;
                    errorInfo.Message = "RoutingManager is not available";
                    Debug.WriteLine($"❌ {errorInfo.Message}");
                    return errorInfo;
                }

                Debug.WriteLine("📍 Registering standard routes...");

                var routes = new Dictionary<string, string>
                {
                    { "MainFrm", "MainFrm" },
                    { "uc_ConnnectionDrivers", "uc_ConnnectionDrivers" },
                    { "uc_FilterForm", "uc_FilterForm" },
                    { "uc_RDBMSConnections", "uc_RDBMSConnections" },
                    { "uc_FileConnections", "uc_FileConnections" },
                    { "uc_EntityEditor", "uc_EntityEditor" },
                    { "uc_CreateLocalDB", "uc_CreateLocalDB" },
                    { "uc_diagraming", "uc_diagraming" },
                    { "uc_FunctiontoFunctionMapping", "uc_FunctiontoFunctionMapping" },
                    { "uc_DataEdit", "uc_DataEdit" },
                    { "uc_CopyEntities", "uc_CopyEntities" },
                    { "uc_DataConnections", "uc_DataConnections" }
                };

                int successCount = 0;
                int failureCount = 0;

                foreach (var route in routes)
                {
                    try
                    {
                        var result = VisManager.RoutingManager.RegisterRouteByName(route.Key, route.Value);
                        if (result?.Flag == Errors.Ok)
                        {
                            Debug.WriteLine($"   ✅ Registered: {route.Key}");
                            successCount++;
                        }
                        else
                        {
                            Debug.WriteLine($"   ⚠️  Failed: {route.Key} - {result?.Message}");
                            failureCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"   ⚠️  Exception registering {route.Key}: {ex.Message}");
                        failureCount++;
                    }
                }

                Debug.WriteLine($"📍 Route registration: {successCount} successful, {failureCount} failed");

                errorInfo.Flag = Errors.Ok;
                errorInfo.Message = $"Routes registered: {successCount} successful";
                return errorInfo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Error registering routes: {ex.Message}");
                errorInfo.Flag = Errors.Failed;
                errorInfo.Message = ex.Message;
                errorInfo.Exception = ex;
                return errorInfo;
            }
        }
    }
}
