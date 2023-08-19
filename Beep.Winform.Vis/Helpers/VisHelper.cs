using Beep.Winform.Vis.Controls.TreeControls;
using BeepEnterprize.Vis.Module;
using BeepEnterprize.Winform.Vis.Helpers;
using DataManagementModels.DriversConfigurations;
using System.Collections;
using System.Reflection;
using TheTechIdea.Beep;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Util;

namespace Beep.Winform.Vis.Helpers
{
    public class VisHelper : IVisHelper
    {
        public VisHelper(IDMEEditor pDMEEditor, IVisManager pVismanager)
        {
            DMEEditor = pDMEEditor;
            Vismanager = pVismanager;
            ImageList = new ImageList();
            //ImageList.ColorDepth = ColorDepth.Depth32Bit;
          //  ImageList.ImageSize = new Size(32,32);
        }
        public IDMEEditor DMEEditor { get; set; }
        public List<ImageConfiguration> ImgAssemblies { get; set; }
        public List<MenuList> Menus { get; set; } = new List<MenuList>();
        public IVisManager Vismanager { get; set; }
        public ImageList ImageList { get; set; }
        public void FillImageList()
        {
            ImageList.Images.Clear(); // Clear existing images if any

            foreach (var file in ImgAssemblies)
            {
                Image image = null;

                if (file.Assembly != null)
                {
                    // Get the resource stream from the assembly
                    using (Stream stream = file.Assembly.GetManifestResourceStream(file.Path))
                    {
                        if (stream != null)
                        {
                            // Copy the resource stream to a memory stream
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                memoryStream.Position = 0;
                                image = Image.FromStream(memoryStream);
                            }
                        }
                    }
                }
                else
                {
                    // Read the file from the path
                    image = Image.FromFile(file.Path);
                }

                if (image != null)
                {
                    ImageList.Images.Add(file.Name, image); // Add image to the ImageList
                }
            }
        }

        public List<ImageConfiguration> GetGraphicFilesLocations(string path = null, bool isEmbeddedResource = false)
        {
            var result = new List<ImageConfiguration>();

            // Add extensions to look for
            string[] extensions = { ".png", ".ico", ".svg" };

            int index = 0; // Explicit index for the files

            if (isEmbeddedResource)
            {
                // Get current, executing, and calling assemblies
                Assembly[] assemblies = {
            Assembly.GetExecutingAssembly(),
            Assembly.GetCallingAssembly(),
            Assembly.GetEntryAssembly()
        };

                foreach (Assembly assembly in assemblies)
                {
                    // Get all embedded resources
                    string[] resources = assembly.GetManifestResourceNames();

                    foreach (string resource in resources)
                    {
                        foreach (string extension in extensions)
                        {
                            if (resource.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                            {
                                result.Add(new ImageConfiguration
                                {
                                    Index = index++,
                                    Name = Path.GetFileName(resource),
                                    Ext = extension,
                                    Path = resource,
                                    Assembly = assembly
                                });
                                break;
                            }
                        }
                    }
                }
            }
            else if (path != null && Directory.Exists(path))
            {
                // Iterate through the files in the folder
                foreach (string filePath in Directory.GetFiles(path))
                {
                    string extension = Path.GetExtension(filePath);

                    // Check if the file has one of the specified extensions
                    if (Array.Exists(extensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Add(new ImageConfiguration
                        {
                            Index = index++,
                            Name = Path.GetFileName(filePath),
                            Ext = extension,
                            Path = filePath
                        });
                    }
                }
            }
            ImgAssemblies = result;
            FillImageList();
            return result;
        }
        private ImageList GetImageList()
        {
           
            return ImageList;
        }
        public int GetImageIndexFromConnectioName(string Connectioname)
        {
            try
            {
                string drname = null;
                string iconname = null;
                ConnectionDriversConfig connectionDrivers;
                if (DMEEditor.ConfigEditor.DataConnections.Where(c => c.ConnectionName == Connectioname).Any())
                {
                    drname = DMEEditor.ConfigEditor.DataConnections.Where(c => c.ConnectionName == Connectioname).FirstOrDefault().DriverName;
                }

                if (drname != null)
                {
                    string drversion = DMEEditor.ConfigEditor.DataConnections.Where(c => c.ConnectionName == Connectioname).FirstOrDefault().DriverVersion;
                    if (DMEEditor.ConfigEditor.DataDriversClasses.Where(c => c.version == drversion && c.DriverClass == drname).Any())
                    {

                        connectionDrivers = DMEEditor.ConfigEditor.DataDriversClasses.Where(c => c.version == drversion && c.DriverClass == drname).FirstOrDefault();
                        if (connectionDrivers != null)
                        {
                            iconname = connectionDrivers.iconname;
                        }
                    }
                    else
                    {
                        connectionDrivers = DMEEditor.ConfigEditor.DataDriversClasses.Where(c => c.DriverClass == drname).FirstOrDefault();
                        if (connectionDrivers != null)
                        {
                            iconname = connectionDrivers.iconname;
                        }
                    }

                    int imgindx = ImageList.Images.IndexOfKey(iconname);
                  
                    return imgindx;
                }
                else
                    return -1;
            }
            catch (Exception)
            {

                return -1;
            }

        }
        public  int GetImageIndex(string name)
        {
            int imgindx = ImageList.Images.IndexOfKey(name);

            return imgindx;

           
        }
        public object GetImage(string imagename)
        {
            try
            {
                int idx = GetImageIndex(imagename);
                if (idx == -1)
                {
                   idx= ImageList.Images.IndexOfKey(imagename);
                }
                object img =ImageList.Images[idx];
                return img;
                // Tree.SelectedImageIndex = GetImageIndex("select.ico");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public object GetImageFromIndex(int index)
        {
            if (index >= 0 && index < ImgAssemblies.Count)
            {
                var file = ImgAssemblies[index];

                if (file.Assembly != null)
                {
                    // Get the resource stream from the assembly
                    using (Stream stream = file.Assembly.GetManifestResourceStream(file.Path))
                    {
                        if (stream != null)
                        {
                            // Create and return the Image from the stream
                            return Image.FromStream(stream);
                        }
                    }
                }
                else
                {
                    // Read the file from the path
                    using (Stream stream = File.OpenRead(file.Path))
                    {
                        // Create and return the Image from the stream
                        return Image.FromStream(stream);
                    }
                }
            }

            return null; // Return null if the index is out of range or the image is not found
        }
        public  object GetImageFromName(string name)
        {
            foreach (var file in ImgAssemblies)
            {
                if (file.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    if (file.Assembly != null)
                    {
                        // Get the resource stream from the assembly
                        using (Stream stream = file.Assembly.GetManifestResourceStream(file.Path))
                        {
                            if (stream != null)
                            {
                                // Create and return the Image from the stream
                                return Image.FromStream(stream);
                            }
                        }
                    }
                    else
                    {
                        // Read the file from the path
                        using (Stream stream = File.OpenRead(file.Path))
                        {
                            // Create and return the Image from the stream
                            return Image.FromStream(stream);
                        }
                    }
                }
            }

            return null; // Return null if the name is not found
        }
        public Image GetImageFromFullName(string fullName)
        {
            Bitmap image = null;
            Stream stream;
            Assembly assembly = Assembly.GetExecutingAssembly();
            // Is this just a single (ie. one-time) image?
            stream = assembly.GetManifestResourceStream(fullName);
            if (stream != null)
            {

                image = new Bitmap(stream);
                stream.Close();
                return image;
            }
            else
            {
                assembly = Assembly.GetCallingAssembly();
                stream = assembly.GetManifestResourceStream(fullName);
                if (stream != null)
                {
                    image = new Bitmap(stream);
                    stream.Close();
                    return image;
                }
            }
            return null;
        }
        public List<string> GetImageList(Assembly assembly)
        {
            System.Globalization.CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            string resourceName = assembly.GetName().Name;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(resourceName, assembly);
            System.Resources.ResourceSet resourceSet = rm.GetResourceSet(culture, true, true);
            List<string> resources = new List<string>();
            foreach (DictionaryEntry resource in resourceSet)
            {
                resources.Add((string)resource.Key);
            }
            rm.ReleaseAllResources();
            return resources;
        }
        private bool IsMethodApplicabletoNode(AssemblyClassDefinition cls, IBranch br)
        {
            if (cls.classProperties == null)
            {
                return true;
            }
            if (cls.classProperties.ObjectType != null)
            {
                if (!cls.classProperties.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }
            return true;


        }
        //public void Nodemenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    ContextMenuStrip menu = (ContextMenuStrip)sender;
        //    ToolStripItem item = e.ClickedItem;
        //    menu.Hide();
        //    AssemblyClassDefinition cls = (AssemblyClassDefinition)item.Tag;

        //    if (cls != null)
        //    {
        //        if (!IsMethodApplicabletoNode(cls, br)) return;
        //        if (cls.componentType == "IFunctionExtension")
        //        {
        //            RunFunction(br, item);

        //        }
        //        else
        //        {

        //            RunMethod(br, item.Text);
        //        };

        //    }
        //}
        //public void Nodemenu_MouseClick(TreeNodeMouseClickEventArgs e)
        //{
           
        
        //    if (br != null)
        //    {
        //        string clicks = "";
        //        if (e.Button == MouseButtons.Right)
        //        {
        //            if (IsMenuCreated(br))
        //            {
        //                MenuList menuList = GetMenuList(br);
        //                menuList.Menu.Show(Cursor.Position);
        //            }
        //        }
        //        else
        //        {
        //            switch (e.Clicks)
        //            {
        //                case 1:
        //                    clicks = "SingleClick";
        //                    break;
        //                case 2:
        //                    clicks = "DoubleClick";
        //                    break;

        //                default:
        //                    break;
        //            }
        //            AssemblyClassDefinition cls = DMEEditor.ConfigEditor.BranchesClasses.Where(x => x.PackageName == br.Name && x.Methods.Where(y => y.DoubleClick == true || y.Click == true).Any()).FirstOrDefault();
        //            if (cls != null)
        //            {
        //                if (!IsMethodApplicabletoNode(cls, br)) return;
        //                RunMethod(br, clicks);

        //            }
        //        }

        //    }

        //}
        public void Nodemenu_ItemClicked(IBranch br, object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip menu = (ContextMenuStrip)sender;
            ToolStripItem item = e.ClickedItem;
            menu.Hide();
            AssemblyClassDefinition cls = (AssemblyClassDefinition)item.Tag;

            if (cls != null)
            {
                if (!IsMethodApplicabletoNode(cls, br)) return;
                if (cls.componentType == "IFunctionExtension")
                {
                    RunFunction(br, item);

                }
                else
                {

                    RunMethod(br, item.Text);
                };

            }
        }
        public void Nodemenu_MouseClick(IBranch br, TreeNodeMouseClickEventArgs e)
        {


            if (br != null)
            {
                string clicks = "";
                if (e.Button == MouseButtons.Right)
                {
                    if (IsMenuCreated(br))
                    {
                        MenuList menuList = GetMenuList(br);
                        menuList.Menu.Show(Cursor.Position);
                    }
                }
                else
                {
                    switch (e.Clicks)
                    {
                        case 1:
                            clicks = "SingleClick";
                            break;
                        case 2:
                            clicks = "DoubleClick";
                            break;

                        default:
                            break;
                    }
                    AssemblyClassDefinition cls = DMEEditor.ConfigEditor.BranchesClasses.Where(x => x.PackageName == br.Name && x.Methods.Where(y => y.DoubleClick == true || y.Click == true).Any()).FirstOrDefault();
                    if (cls != null)
                    {
                        if (!IsMethodApplicabletoNode(cls, br)) return;
                        RunMethod(br, clicks);

                    }
                }

            }

        }
        private bool IsMenuCreated(IBranch br)
        {
            if (br.ObjectType != null)
            {
                return Menus.Where(p => p.ObjectType != null && p.BranchClass.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase)
                && p.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase)
                && p.PointType == br.BranchType).Any();
            }
            return
                false;




        }
        private MenuList GetMenuList(IBranch br)
        {
            return Menus.Where(p => p.ObjectType != null && p.BranchClass.Equals(br.BranchClass, StringComparison.InvariantCultureIgnoreCase)
                && p.ObjectType.Equals(br.ObjectType, StringComparison.InvariantCultureIgnoreCase)
                && p.PointType == br.BranchType).FirstOrDefault();
        }
        public void RunFunction(IBranch br,object sender, EventArgs e)
        {
         
            AssemblyClassDefinition assemblydef = new AssemblyClassDefinition();
            MethodInfo method = null;
            MethodsClass methodsClass;
            string MethodName = "";
            if (sender == null) { return; }
            if (sender.GetType() == typeof(ToolStripButton))
            {
                ToolStripButton item = (ToolStripButton)sender;
                assemblydef = (AssemblyClassDefinition)item.Tag;
                MethodName = item.Text;
            }
            if (sender.GetType() == typeof(ToolStripMenuItem))
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                assemblydef = (AssemblyClassDefinition)item.Tag;
                MethodName = item.Text;
            }
            dynamic fc = DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.dllname, assemblydef.type.ToString(), new object[] { DMEEditor, Vismanager, this });
            //  dynamic fc = DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.type.ToString(), new object[] { DMEEditor, Vismanager, this });
            if (fc == null)
            {
                return;
            }
            Type t = ((IFunctionExtension)fc).GetType();
            AssemblyClassDefinition cls = DMEEditor.ConfigEditor.GlobalFunctions.Where(x => x.className == t.Name).FirstOrDefault();
            methodsClass = cls.Methods.Where(x => x.Caption == MethodName).FirstOrDefault();
            if (br != null)
            {
                DMEEditor.Passedarguments.ObjectName = br.BranchText;
                DMEEditor.Passedarguments.DatasourceName = br.DataSourceName;
                DMEEditor.Passedarguments.Id = br.BranchID;
                DMEEditor.Passedarguments.ParameterInt1 = br.BranchID;
                if (!IsMethodApplicabletoNode(cls, br)) return;
            }
            if (methodsClass != null)
            {
                method = methodsClass.Info;
                if (method.GetParameters().Length > 0)
                {
                    method.Invoke(fc, new object[] { DMEEditor.Passedarguments });
                }
                else
                    method.Invoke(fc, null);
            }
        }
        public void RunFunction(IBranch br, ToolStripItem item)
        {
            if (br != null)
            {
                DMEEditor.Passedarguments.ObjectName = br.BranchText;
                DMEEditor.Passedarguments.DatasourceName = br.DataSourceName;
                DMEEditor.Passedarguments.Id = br.BranchID;
                DMEEditor.Passedarguments.ParameterInt1 = br.BranchID;
                AssemblyClassDefinition assemblydef = (AssemblyClassDefinition)item.Tag;
                dynamic fc = DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.dllname, assemblydef.type.ToString(), new object[] { DMEEditor, Vismanager, this });
                Type t = (( IFunctionExtension)fc).GetType();
                //dynamic fc = Activator.CreateInstance(assemblydef.type, new object[] { DMEEditor, Vismanager, this });
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.GlobalFunctions.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                MethodsClass methodsClass;
                if (!IsMethodApplicabletoNode(cls, br)) return;
                try
                {
                    if (br.BranchType != TheTechIdea.Beep.Vis.EnumPointType.Global)
                    {
                        methodsClass = cls.Methods.Where(x => x.Caption == item.Text).FirstOrDefault();
                    }
                    else
                    {
                        methodsClass = cls.Methods.Where(x => x.Caption == item.Text && x.PointType == br.BranchType).FirstOrDefault();
                    }

                }
                catch (Exception)
                {

                    methodsClass = null;
                }
                if (methodsClass != null)
                {
                    method = methodsClass.Info;
                    if (method.GetParameters().Length > 0)
                    {
                        method.Invoke(fc, new object[] { DMEEditor.Passedarguments });
                    }
                    else
                        method.Invoke(fc, null);
                }
            }


        }
        public IErrorsInfo RunMethod(Object branch, string MethodName)
        {

            try
            {
                Type t = branch.GetType();
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.BranchesClasses.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                MethodsClass methodsClass;
                try
                {
                    methodsClass = cls.Methods.Where(x => x.Caption == MethodName).FirstOrDefault();
                }
                catch (Exception)
                {

                    methodsClass = null;
                }
                if (methodsClass != null)
                {
                    if (!IsMethodApplicabletoNode(cls, (IBranch)branch)) return DMEEditor.ErrorObject;
                    method = methodsClass.Info;
                    if (method.GetParameters().Length > 0)
                    {
                        method.Invoke(branch, new object[] { DMEEditor.Passedarguments.Objects[0].obj });
                    }
                    else
                        method.Invoke(branch, null);


                    //  DMEEditor.AddLogMessage("Success", "Running method", DateTime.Now, 0, null, Errors.Ok);
                }

            }
            catch (Exception ex)
            {
                string mes = "Could not Run Method " + MethodName;
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
    }
}
