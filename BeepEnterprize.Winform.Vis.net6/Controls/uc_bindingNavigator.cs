using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BeepEnterprize.Winform.Vis.ETL.ImportData;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Logger;
using TheTechIdea.Util;

namespace BeepEnterprize.Winform.Vis.Controls
{
    public partial class uc_bindingNavigator : UserControl,IDM_Addin
    {
        public uc_bindingNavigator()
        {
            InitializeComponent();
            Loadimages();
        }

        public event EventHandler<BindingSource> CallPrinter;
        public event EventHandler<BindingSource> SendMessage;
        public event EventHandler<BindingSource> ShowSearch;
        public string ParentName { get ; set ; }
        public string ObjectName { get ; set ; }
        public string ObjectType { get ; set ; }
        public string AddinName { get ; set ; }
        public string Description { get ; set ; }
        public bool DefaultCreate { get ; set ; }
        public string DllPath { get ; set ; }
        public string DllName { get ; set ; }
        public string NameSpace { get ; set ; }
        public IErrorsInfo ErrorObject { get ; set ; }
        public IDMLogger Logger { get ; set ; }
        public IDMEEditor DMEEditor { get ; set ; }
        public EntityStructure EntityStructure { get ; set ; }
        public string EntityName { get ; set ; }
        public IPassedArgs Passedarg { get ; set ; }
        VisManager visManager;
        ImportDataManager importDataManager;
        public  BindingSource bindingSource { get; set; }
        public int ControlHeight { get; set; } = 30;
        public Color HightlightColor { get { return Highlightpanel.BackColor; } set { Highlightpanel.BackColor = value; } } 
        public Color SelectedColor { get { return Focuspanel.BackColor; } set { Focuspanel.BackColor = value; } } 
        public void Run(IPassedArgs pPassedarg)
        {
           
        }
        public void SetConfig(IDMEEditor pbl, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs e, IErrorsInfo per)
        {
            DMEEditor = pbl;
            ErrorObject = pbl.ErrorObject;
            Logger = pbl.Logger;

            if (e.Objects.Where(c => c.Name == "VISUTIL").Any())
            {
                visManager = (VisManager)e.Objects.Where(c => c.Name == "VISUTIL").FirstOrDefault().obj;
            }
            if (e.Objects.Where(c => c.Name == "ImportDataManager").Any())
            {
                importDataManager = (ImportDataManager)e.Objects.Where(c => c.Name == "ImportDataManager").FirstOrDefault().obj;
            }
            if (e.Objects.Where(c => c.Name == "BindingSource").Any())
            {
                bindingSource = (BindingSource)e.Objects.Where(c => c.Name == "BindingSource").FirstOrDefault().obj;
            }

            Loadimages();
            InitPanels();

            Highlightpanel.BackColor = Color.SteelBlue;
            Focuspanel.BackColor = Color.DarkOrange;

            this.Height = ControlHeight;
            this.SizeChanged += Uc_bindingNavigator_SizeChanged;
            this.Resize += Uc_bindingNavigator_Resize;

            if (bindingSource != null)
            {
                this.bindingSource.DataSourceChanged += BindingSource_DataSourceChanged;
                this.bindingSource.ListChanged += BindingSource_ListChanged;
                this.bindingSource.CurrentChanged += BindingSource_CurrentChanged;
                this.NewpictureBox1.Click += NewpictureBox1_Click;
                this.PreviouspictureBox.Click += PreviouspictureBox_Click;
                this.NextpictureBox.Click += NextpictureBox_Click;
                this.RemovepictureBox.Click += RemovepictureBox_Click;
                this.SavepictureBox.Click += SavepictureBox_Click;
                this.RollbackpictureBox.Click += RollbackpictureBox_Click;
                this.EditpictureBox.Click += EditpictureBox_Click;
                this.FindpictureBox.Click += FindpictureBox_Click;
                this.PrinterpictureBox.Click += PrinterpictureBox_Click;
                this.MessegepictureBox.Click += MessegepictureBox_Click;

                this.NewpictureBox1.MouseHover += pictureBox_MouseHover;
                this.PreviouspictureBox.MouseHover += pictureBox_MouseHover;
                this.NextpictureBox.MouseHover += pictureBox_MouseHover;
                this.RemovepictureBox.MouseHover += pictureBox_MouseHover;
                this.SavepictureBox.MouseHover += pictureBox_MouseHover;
                this.RollbackpictureBox.MouseHover += pictureBox_MouseHover;
                this.EditpictureBox.MouseHover += pictureBox_MouseHover;
                this.FindpictureBox.MouseHover += pictureBox_MouseHover;
                this.MessegepictureBox.MouseHover += pictureBox_MouseHover;
                this.PrinterpictureBox.MouseHover += pictureBox_MouseHover;
                this.Recordnumberinglabel1.MouseMove += Recordnumberinglabel1_MouseMove;

                datasourcechanged();
            }
        }
        private void Recordnumberinglabel1_MouseMove(object sender, MouseEventArgs e)
        {
            InitPanels();
        }
        private void pictureBox_MouseHover(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            if (box != null)
            {
                Highlightpanel.Left = box.Left-2 ;
                Highlightpanel.Top = box.Top-2;
                Highlightpanel.Height = box.Height+4;
                Highlightpanel.Width = box.Width+4;
                Highlightpanel.SendToBack();
            }
        }
        private void InitPanels()
        {
            Highlightpanel.Left = Recordnumberinglabel1.Left - 2;
            Highlightpanel.Top = Recordnumberinglabel1.Top-2;
            Highlightpanel.Height = Recordnumberinglabel1.Height+4;
            Highlightpanel.Width = Recordnumberinglabel1.Width + 4;
            Focuspanel.Visible = false;
        }
        #region "Click Methods for all Buttons"
        private void FocusPicture(object sender)
        {
            PictureBox box = (PictureBox)sender;
            if (box != null)
            {
                Focuspanel.Visible = true;
                Focuspanel.Left = box.Left - 2;
                Focuspanel.Top = box.Top - 2;
                Focuspanel.Height = box.Height + 4;
                Focuspanel.Width = box.Width + 4;
                
                
            }
        }
        private void MessegepictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                //  if (MessageBox.Show(this.Parent, "Are you sure you want to cancel Changes?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                //  {
                // bindingSource.CancelEdit();
                FocusPicture(sender);
                SendMessage?.Invoke(this, bindingSource);
                // };

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }
        }
        private void PrinterpictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                //  if (MessageBox.Show(this.Parent, "Are you sure you want to cancel Changes?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                //  {
                // bindingSource.CancelEdit();
                FocusPicture(sender);
                CallPrinter?.Invoke(this, bindingSource);
                // };

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }
        }
        private void FindpictureBox_Click(object sender, EventArgs e)
        {
            try
            {
              //  if (MessageBox.Show(this.Parent, "Are you sure you want to cancel Changes?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
              //  {
                   // bindingSource.CancelEdit();
                    FocusPicture(sender);
                ShowSearch?.Invoke(this, bindingSource);
                // };

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }
        }
        private void EditpictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                //if (MessageBox.Show(this.Parent, "Are you sure you want to cancel Changes?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                //{
                  //  bindingSource.CancelEdit();
                    FocusPicture(sender);
               // };

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }
        }
        private void RollbackpictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this.Parent, "Are you sure you want to cancel Changes?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                  bindingSource.CancelEdit();
                    FocusPicture(sender);
                };

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }
        }
        private void SavepictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource.EndEdit();
                FocusPicture(sender);
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }

        }
        private void RemovepictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this.Parent, "Are you sure you want to Delete Record?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    bindingSource.RemoveCurrent();
                };
                FocusPicture(sender);
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }

        }
        private void NextpictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingSource.Count > 0)
                    if (bindingSource[bindingSource.Count - 1] != null)
                        if (bindingSource.Position + 1 < bindingSource.Count)
                            bindingSource.MoveNext();
                ;
                FocusPicture(sender);
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }

        }
        private void PreviouspictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (bindingSource.Count > 0)
                    if (bindingSource[bindingSource.Count - 1] != null)
                        if (bindingSource.Position > 0)
                            bindingSource.MovePrevious();
                ;
                FocusPicture(sender);
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }

        }
        private void NewpictureBox1_Click(object sender, EventArgs e)
        {
            try
            {

                bindingSource.AddNew();
                FocusPicture(sender);

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }

        }
        #endregion
        #region "BindingSource Events"
        private void BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            datasourcechanged();
        }
        private void Uc_bindingNavigator_Resize(object sender, EventArgs e)
        {
            this.Height = ControlHeight;
        }
        private void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            datasourcechanged();
        }
        private void BindingSource_DataSourceChanged(object sender, EventArgs e)
        {
            datasourcechanged();


        }
        private void datasourcechanged()
        {
            if (this.bindingSource != null)
            {
                if (this.bindingSource.List != null)
                {
                    this.Recordnumberinglabel1.Text = (Convert.ToInt32(bindingSource.Position) + 1).ToString() + " From " + bindingSource.Count.ToString();
                }
                else
                    this.Recordnumberinglabel1.Text = "-";
            }
        }

        #endregion
        private void Uc_bindingNavigator_SizeChanged(object sender, EventArgs e)
        {
            this.Height = ControlHeight;
        }

        #region "Resource Loaders"
        private void Loadimages()
        {
            this.NewpictureBox1.Image = GetImage("add.png");
            //this.Newbutton.Image = visManager.GetImage("add.png");
            this.SavepictureBox.Image = GetImage("save.png");
            this.NextpictureBox.Image = GetImage("right.png");
            this.PreviouspictureBox.Image = GetImage("left.png");
            this.RemovepictureBox.Image = GetImage("remove.png");
            this.RollbackpictureBox.Image = GetImage("back.png");
            this.EditpictureBox.Image = GetImage("edit.png");
            this.FindpictureBox.Image = GetImage("search.png");
           this.MessegepictureBox.Image = GetImage("messages.png");
            this.PrinterpictureBox.Image = GetImage("printer.png");
        }
        public Image GetImage(string fullName)
        {
            Bitmap image = null;
            Stream stream;
            Assembly assembly = Assembly.GetExecutingAssembly();
            // Is this just a single (ie. one-time) image?
            stream = assembly.GetManifestResourceStream("BeepEnterprize.Winform.Vis.gfx." + fullName);
            if (stream != null)
            {
                image = new Bitmap(stream);
                stream.Close();
                return image;
            }
            else
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
        #endregion
    }
}
