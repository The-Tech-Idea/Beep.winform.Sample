﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using BeepEnterprize.Vis.Module;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Logger;
using TheTechIdea.Util;
using static TheTechIdea.Beep.Util;

namespace BeepEnterprize.Winform.Vis.Controls
{
    [AddinAttribute(Caption = "Beep", Name = "TreeControl", misc = "Control")]
    public class TreeControl : IDM_Addin,ITree
    {
        public TreeControl(IDMEEditor pDMEEditor,VisManager pVismanager)
        {
            DMEEditor = pDMEEditor;
            Vismanager = pVismanager;
            VisManager = pVismanager;
          
            
        }
        #region "Addin Properties"
        public string ParentName { get; set; }
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public string AddinName { get; set; }
        public string Description { get; set; }
        public bool DefaultCreate { get; set; }
        public string DllPath { get; set; }
        public string DllName { get; set; }
        public string NameSpace { get; set; }
        public IErrorsInfo ErrorObject { get; set; }
        public IDMLogger Logger { get; set; }
        public IDMEEditor DMEEditor { get; set; }
        public EntityStructure EntityStructure { get; set; }
        public string EntityName { get; set; }
        public IPassedArgs Passedarg { get; set; }
        #endregion

        public VisManager Vismanager { get; set; }
        public IVisManager VisManager { get; set; }
        public System.Windows.Forms.TreeView TreeV { get; set; }
        public string CategoryIcon { get; set; } = "Category.ico";
        public string SelectIcon { get; set; } = "select.ico";
        public IBranch CurrentBranch { get ; set ; }
        public List<ContextMenu> menus { get; set; }=new List<ContextMenu>();
        public PassedArgs args { get; set; }
        static int pSeqID = 0;
        public int SeqID {
            get {
                pSeqID += 1;
                return pSeqID;
            }
        }
        public List<IBranch> Branches { get; set; } = new List<IBranch>();
        public int SelectedBranchID { get ; set ; }
        public string TreeEvent { get; set; }
        public  string TreeOP { get; set; }
        public TreeNode LastSelectedNode { get; set; }
        public TreeNode SelectedNode { get; set; }
        public int StartselectBranchID { get; set; }
        public Color SelectBackColor { get; set; }
        public List<int> SelectedBranchs { get; set; } = new List<int>();
        public TreeNodeDragandDropHandler treeNodeDragandDropHandler { get ; set ; }
        public ITreeBranchHandler treeBranchHandler { get ; set ; }
        public IErrorsInfo CreateFunctionExtensions(MethodsClass item)
        {
            ContextMenuStrip nodemenu = new ContextMenuStrip();
            try
            {
                nodemenu.ImageList = Vismanager.Images;
                ToolStripItem st = nodemenu.Items.Add(item.Caption);
                foreach  (IBranch br in Branches)
                {
                    if(br.BranchType== item.PointType)
                    {
                        nodemenu.Name = br.ToString();
                        if (item.iconimage != null)
                        {
                            st.ImageIndex = Vismanager.visHelper.GetImageIndex(item.iconimage);
                        }
                        nodemenu.ItemClicked += Nodemenu_ItemClicked;
                        nodemenu.Tag = br;
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = $"Could not add method from Extension {item.Name} to menu " ;
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;

        }
        public IErrorsInfo CreateGlobalMenu(IBranch br, TreeNode n)
        {
            try
            {
                ContextMenuStrip nodemenu = n.ContextMenuStrip;
                if (nodemenu == null)
                {
                    nodemenu = new ContextMenuStrip();
                    nodemenu.ItemClicked += Nodemenu_ItemClicked;
                }
                List<AssemblyClassDefinition> extentions = DMEEditor.ConfigEditor.GlobalFunctions.OrderBy(p => p.Order).ToList();
                foreach (AssemblyClassDefinition cls in extentions)
                {
                    foreach (var item in cls.Methods)
                    {
                        if (item.PointType == br.BranchType)
                        {
                            ToolStripItem st = nodemenu.Items.Add(item.Caption);
                            nodemenu.Name = br.ToString();
                            if (item.iconimage != null)
                            {
                                st.ImageIndex = Vismanager.visHelper.GetImageIndex(item.iconimage);
                            }
                            st.Tag = cls;
                        }
                    }
                }

                return DMEEditor.ErrorObject;
            }
            catch (Exception ex)
            {
                return DMEEditor.ErrorObject;
            }
        }
        public IErrorsInfo CreateRootTree()
        {
            SetupTreeView();
            treeNodeDragandDropHandler = new TreeNodeDragandDropHandler(DMEEditor, this);
            treeBranchHandler = new TreeBranchHandler(DMEEditor, this);
            Branches = new List<IBranch>();
            try
            {
                foreach (AssemblyClassDefinition cls in DMEEditor.ConfigEditor.BranchesClasses.OrderBy(x => x.Order))
                {
                    Type adc = DMEEditor.assemblyHandler.GetType(cls.PackageName);
                    ConstructorInfo ctor = adc.GetConstructors().Where(o => o.GetParameters().Length == 0).FirstOrDefault(); ;
                    if (ctor != null)
                    {
                        ObjectActivator<IBranch> createdActivator = GetActivator<IBranch>(ctor);
                        try
                        {
                            IBranch br = createdActivator();
                            if (br.BranchType == EnumPointType.Root)
                            {

                                int id = SeqID;
                                br.Name = cls.PackageName;
                                TreeNode n = TreeV.Nodes.Add(br.BranchText);
                                n.Tag = id;

                                br.TreeEditor = this;
                                br.Visutil = VisManager;
                                br.BranchID = id;
                                br.ID = id;
                                if (Vismanager.visHelper.GetImageIndex(br.IconImageName) == -1)
                                {
                                    n.ImageIndex = Vismanager.visHelper.GetImageIndexFromConnectioName(br.BranchText);
                                    n.SelectedImageIndex = Vismanager.visHelper.GetImageIndexFromConnectioName(br.BranchText);
                                }
                                else
                                {
                                    n.ImageKey = br.IconImageName;
                                    n.SelectedImageKey = br.IconImageName;
                                }

                                //   ITreeView treeView = (ITreeView)br;
                                //  treeView.Visutil = Visutil;
                                n.ContextMenuStrip = CreateMenuMethods(br);
                                CreateGlobalMenu(br,n);
                                br.DMEEditor = DMEEditor;
                                if (!DMEEditor.ConfigEditor.objectTypes.Any(i => i.ObjectType == br.BranchClass && i.ObjectName == br.BranchType.ToString() + "_" + br.BranchClass))
                                {
                                    DMEEditor.ConfigEditor.objectTypes.Add(new TheTechIdea.Beep.Workflow.ObjectTypes { ObjectType = br.BranchClass, ObjectName = br.BranchType.ToString() + "_" + br.BranchClass });
                                }
                                Branches.Add(br);
                                br.CreateChildNodes();
                            }
                        }
                        catch (Exception ex)
                        {
                            DMEEditor.AddLogMessage("Error", $"Creating Tree Root Node {cls.PackageName} {ex.Message} ", DateTime.Now, 0, null, Errors.Failed);

                        }

                    }
               

                }
                // DMEEditor.AddLogMessage("Success", "Create Tree", DateTime.Now, 0, null, Errors.Ok);
            }
            catch (Exception ex)
            {
                string mes = "Could not Create Tree";
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return DMEEditor.ErrorObject;
        }
        public ContextMenuStrip CreateMenuMethods(IBranch branch)
        {

            ContextMenuStrip nodemenu = new ContextMenuStrip();
            try
            {
                nodemenu.ImageList = Vismanager.Images;
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.BranchesClasses.Where(x => x.PackageName == branch.ToString()).FirstOrDefault();
                foreach (var item in cls.Methods.Where(y => y.Hidden == false))
                {
                    ToolStripItem st = nodemenu.Items.Add(item.Caption);
                    nodemenu.Name = branch.ToString();
                    if (item.iconimage != null)
                    {
                        st.ImageIndex = Vismanager.visHelper.GetImageIndex(item.iconimage);
                    }
                    st.Tag = cls;

                }
                nodemenu.ItemClicked += Nodemenu_ItemClicked;
              
                

            }
            catch (Exception ex)
            {
                string mes = "Could not add method to menu " + branch.BranchText;
                DMEEditor.AddLogMessage(ex.Message, mes, DateTime.Now, -1, mes, Errors.Failed);
            };
            return nodemenu;
        }
        public void Run(IPassedArgs pPassedarg)
        {
            throw new NotImplementedException();
        }
        public void RunFunction(object sender, EventArgs e)
        {
            IBranch br = null;
            ToolStripItem item = (ToolStripItem)sender;
            if (TreeV.SelectedNode != null)
            {
                TreeNode n = TreeV.SelectedNode;
                br =treeBranchHandler.GetBranch(Convert.ToInt32(n.Tag));
            }
            if (br != null)
            {
                DMEEditor.Passedarguments = new PassedArgs();
                DMEEditor.Passedarguments.ObjectName = br.BranchText;
                DMEEditor.Passedarguments.DatasourceName = br.DataSourceName;
                DMEEditor.Passedarguments.Id = br.BranchID;
                DMEEditor.Passedarguments.ParameterInt1 = br.BranchID;
                AssemblyClassDefinition assemblydef = (AssemblyClassDefinition)item.Tag;
                dynamic fc = DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.type.ToString(), new object[] { DMEEditor, Vismanager, this });
                Type t = ((IFunctionExtension)fc).GetType();
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.GlobalFunctions.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                MethodsClass methodsClass;
                try
                {
                    if (br.BranchType != EnumPointType.Global)
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
        public void RunFunction(IBranch br, ToolStripItem item)
        {
          
         
            if (TreeV.SelectedNode != null)
            {
                TreeNode n = TreeV.SelectedNode;
                br = treeBranchHandler.GetBranch(Convert.ToInt32(n.Tag));
            }
            if (br != null)
            {
                DMEEditor.Passedarguments = new PassedArgs();
                DMEEditor.Passedarguments.ObjectName = br.BranchText;
                DMEEditor.Passedarguments.DatasourceName = br.DataSourceName;
                DMEEditor.Passedarguments.Id = br.BranchID;
                DMEEditor.Passedarguments.ParameterInt1 = br.BranchID;
                AssemblyClassDefinition assemblydef = (AssemblyClassDefinition)item.Tag;
                dynamic fc = DMEEditor.assemblyHandler.CreateInstanceFromString(assemblydef.dllname,assemblydef.type.ToString(), new object[] { DMEEditor, Vismanager, this });
                Type t = ((IFunctionExtension)fc).GetType();
                //dynamic fc = Activator.CreateInstance(assemblydef.type, new object[] { DMEEditor, Vismanager, this });
                AssemblyClassDefinition cls = DMEEditor.ConfigEditor.GlobalFunctions.Where(x => x.className == t.Name).FirstOrDefault();
                MethodInfo method = null;
                MethodsClass methodsClass;
                try
                {
                    if (br.BranchType != EnumPointType.Global)
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
        public void SetConfig(IDMEEditor pbl, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs e, IErrorsInfo per)
        {
            throw new NotImplementedException();
        }
        private void SetupTreeView()
        {

            Vismanager.Images = new ImageList();
            Vismanager.Images.ImageSize = new Size(24, 24);
            Vismanager.Images.ColorDepth = ColorDepth.Depth32Bit;
            foreach (string filename_w_path in Directory.GetFiles(DMEEditor.ConfigEditor.Config.Folders.Where(x => x.FolderFilesType == FolderFileTypes.GFX).FirstOrDefault().FolderPath, "*.ico", SearchOption.AllDirectories))
            {
                try
                {
                    string filename = Path.GetFileName(filename_w_path);

                    Vismanager.Images.Images.Add(filename, System.Drawing.Image.FromFile(filename_w_path));


                }
                catch (FileLoadException ex)
                {
                    DMEEditor.ErrorObject.Flag = Errors.Failed;
                    DMEEditor.ErrorObject.Ex = ex;
                    DMEEditor.Logger.WriteLog($"Error Loading icons ({ex.Message})");
                }

            }
            TreeV.CheckBoxes = false;
            TreeV.ImageList = Vismanager.Images;
            TreeV.ItemHeight = 24;
            TreeV.SelectedImageKey = SelectIcon;
            //TreeV.Dock = DockStyle.Fill;
            //TreeV.SendToBack();
           
        }
        public IErrorsInfo TurnonOffCheckBox(IPassedArgs Passedarguments)
        {
            DMEEditor.ErrorObject.Flag = Errors.Ok;
            try
            {

                //TreeView trv = (TreeView)TreeEditor.TreeStrucure;
                TreeV.CheckBoxes = !TreeV.CheckBoxes;
                SelectedBranchs.Clear();

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Fail", $"Could not select entities {ex.Message}", DateTime.Now, 0, Passedarguments.DatasourceName, Errors.Failed);
            }
            return DMEEditor.ErrorObject;

        }
        #region "TreeNode Handling"
        public TreeNode GetTreeNodeByTag(string tag, TreeNodeCollection p_Nodes)
        {
            try
            {
                foreach (TreeNode node in p_Nodes)
                {
                    if (node.Tag.ToString() == tag)
                    {
                        return node;
                    }

                    if (node.Nodes.Count > 0)
                    {
                        var result = GetTreeNodeByTag(tag, node.Nodes);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;


            }


            return null;// TreeV.Nodes.Cast<TreeNode>().Where(n => n.Tag.ToString() == tag).FirstOrDefault();
        }
        public TreeNode GetTreeNodeByCaption(string Caption, TreeNodeCollection p_Nodes)
        {
            foreach (TreeNode node in p_Nodes)
            {
                if (node.Text == Caption)
                {
                    return node;
                }

                if (node.Nodes.Count > 0)
                {
                    var result = GetTreeNodeByCaption(Caption, node.Nodes);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
        #endregion
        #region "Method Calling"
        public void Nodemenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip n = (ContextMenuStrip)sender;
            ToolStripItem item = e.ClickedItem;
            //IBranch br  = (IBranch)n.Tag;
            n.Hide();
            IBranch br = treeBranchHandler.GetBranch(SelectedBranchID);
            AssemblyClassDefinition cls = (AssemblyClassDefinition)item.Tag;
           
            if (cls != null)
            {
                if (cls.RootName != "IFunctionExtension")
                {
                    RunMethod(br, item.Text);
                }else
                {
                    RunFunction(br, item);
                };

            }
        }
        public void Nodemenu_MouseClick(TreeNodeMouseClickEventArgs e)
        {
            // ContextMenuStrip n = (ContextMenuStrip)e.;
            IBranch br =treeBranchHandler.GetBranch(SelectedBranchID);
            string clicks = "";
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
                RunMethod(br, clicks);
            }
        }

        #endregion
    }
}
