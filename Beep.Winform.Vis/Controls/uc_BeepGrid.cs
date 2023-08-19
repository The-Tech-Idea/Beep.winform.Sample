using System.ComponentModel;
using System.Data;
using BeepEnterprize.Vis.Module;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Logger;
using TheTechIdea.Util;

namespace Beep.Winform.Controls
{
    public partial class uc_BeepGrid : UserControl, IDM_Addin
    {
        public uc_BeepGrid()
        {
            InitializeComponent();
          //  Loadimages();
            InitPanels();
            WireAllControls();
            dataGridView1.Resize += DataGridView_Resize;
            //foreach (Control c in this.Controls)
            //{
            //    c.Click += (sender, e) => { this.InvokeOnClick(this, e); };
            //    c.MouseUp += (sender, e) => { this.OnMouseUp(e); };
            //    c.MouseDown += (sender, e) => { this.OnMouseDown(e); };
            //    c.MouseMove += (sender, e) => { this.OnMouseMove(e); };
            //}
            //   dataGridView1 = new System.Windows.Forms.DataGridView();
            //   // 
            //   // dataGridView1
            //   // 
            //   dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            //   dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //   dataGridView1.Location = new System.Drawing.Point(0, 25);
            //   dataGridView1.Name = "dataGridView1";

            //   dataGridView1.TabIndex = 2;
            //   Controls.Add(this.dataGridView1);
            ////   this.Size = new System.Drawing.Size(250, 200);
            //   dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            //   dataGridView1.BringToFront();
            //    WireAllControls(this);
        }
        public DataGridViewColumnCollection ColumnCollection { get { return dataGridView1.Columns; }  }

        public event EventHandler<BindingSource> CallPrinter;
        public event EventHandler<BindingSource> SendMessage;
        public event EventHandler<BindingSource> ShowSearch;
        public event EventHandler<BindingSource> NewRecordCreated;
        public event EventHandler<BindingSource> SaveCalled;
        public event EventHandler<BindingSource> DeleteCalled;


        private ToolTip searchtooltip;
        private ToolTip addtooltip;
        private ToolTip edittooltip;
        private ToolTip removetooltip;
        private ToolTip nexttooltip;
        private ToolTip previoustooltip;
        private ToolTip canceltooltip;
        private ToolTip savetooltip;
        private ToolTip printtooltip;
        private ToolTip sharetooltip;
        
        private ResourceManager resourceManager = new ResourceManager();
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
     //   public System.Windows.Forms.DataGridView dataGridView1 { get; set; }
        IVisManager visManager;
        BindingSource _bindingSource;
        public BindingSource bindingSource { get { return _bindingSource; } set { dataGridView1.DataSource = value; _bindingSource = value; dataGridView1.Refresh(); } }
        public bool VerifyDelete { get; set; } = true;
        public void Run(IPassedArgs pPassedarg)
        {
            throw new NotImplementedException();
        }

        public void SetConfig(IDMEEditor pbl, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs passarg, IErrorsInfo per)
        {
            DMEEditor = pbl;
            ErrorObject = pbl.ErrorObject;
            Logger = pbl.Logger;

            if (passarg.Objects.Where(c => c.Name == "VISUTIL").Any())
            {
                visManager = (IVisManager)passarg.Objects.Where(c => c.Name == "VISUTIL").FirstOrDefault().obj;
            }
            if (Passedarg.Objects.Where(c => c.Name == "BindingSource").Any())
            {
                bindingSource = (BindingSource)Passedarg.Objects.Where(c => c.Name == "BindingSource").FirstOrDefault().obj;
            }

            if (bindingSource != null)
            {
                this.bindingSource.DataSourceChanged += BindingSource_DataSourceChanged;
                this.bindingSource.ListChanged += BindingSource_ListChanged;
                this.bindingSource.CurrentChanged += BindingSource_CurrentChanged;
              
             //   this..Click -= SavepictureBox_Click;
            
              //  this.EditButton.Click -= EditpictureBox_Click;
              //  this.FindButton.Click -= FindpictureBox_Click;
                this.Printbutton.Click -= PrinterpictureBox_Click;
                this.Sharebutton.Click -= MessegepictureBox_Click;
              
              //  this.RollbackButton.Click += RollbackpictureBox_Click;
               // this.EditButton.Click += EditpictureBox_Click;
             //   this.FindButton.Click += FindpictureBox_Click;
                this.Printbutton.Click += PrinterpictureBox_Click;
                this.Sharebutton.Click += MessegepictureBox_Click;
                datasourcechanged();
            }

        }

        private void WireAllControls()
        {
            foreach (Control c in this.Controls)
            {
                c.Click += (sender, e) => { this.InvokeOnClick(this,e); };
                c.MouseUp += (sender, e) => { this.OnMouseUp(e); };
                c.MouseDown += (sender, e) => { this.OnMouseDown(e); };
                c.MouseMove += (sender, e) => { this.OnMouseMove(e); };
            }
        }
        private void InitPanels()
        {
           
            //foreach (Button item in GetAll(this, typeof(Button)))
            //{
            //    item.FlatAppearance.BorderSize = ButtonBorderSize;
            //    item.FlatAppearance.MouseDownBackColor = SelectedColor;
            //    item.FlatAppearance.MouseOverBackColor = HightlightColor;
            //}

            searchtooltip = new ToolTip();
            addtooltip = new ToolTip();
            edittooltip = new ToolTip();
            removetooltip = new ToolTip();
            nexttooltip = new ToolTip();
            previoustooltip = new ToolTip();
            canceltooltip = new ToolTip();
            savetooltip = new ToolTip();
            printtooltip = new ToolTip();
            sharetooltip = new ToolTip();

            //searchtooltip.SetToolTip(FindButton, "Find");
            //addtooltip.SetToolTip(NewButton, "New Record");
            //edittooltip.SetToolTip(EditButton, "Edit Current Record");
            //removetooltip.SetToolTip(RemoveButton, "Delete Current Record");
            //nexttooltip.SetToolTip(NextButton, "Next");
            //previoustooltip.SetToolTip(PrevoiusButton, "Previous");
            //canceltooltip.SetToolTip(RollbackButton, "Cancel or Rollback changes");
            //savetooltip.SetToolTip(SaveButton, "Save Changes");
            //printtooltip.SetToolTip(PrinterButton, "Print");
            //sharetooltip.SetToolTip(MessageButton, "Share");
           
        }
        private void Loadimages()
        {
            this.Filterbutton.BackgroundImage = resourceManager.GetImage("Beep.Winform.Controls.gfx.", "Filter.png");
            this.Sharebutton.BackgroundImage = resourceManager.GetImage("Beep.Winform.Controls.gfx.", "ShareLink.png");
            this.Printbutton.BackgroundImage = resourceManager.GetImage("Beep.Winform.Controls.gfx.", "Print.png");
         
        }
        #region "BindingSource Events"
        private void RollbackpictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this.Parent, "Are you sure you want to cancel Changes?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    bindingSource.CancelEdit();
                    //      FocusPicture(sender);
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
                SaveCalled?.Invoke(sender, bindingSource);
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
                DeleteCalled?.Invoke(sender, bindingSource);
                if (bindingSource.Current != null)
                {
                    if (bindingSource.Count > 0)
                    {
                        if (VerifyDelete)
                        {
                            if (MessageBox.Show(this.Parent, "Are you sure you want to Delete Record?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                            {
                                bindingSource.RemoveCurrent();
                            };
                        }
                        else
                        {
                            bindingSource.RemoveCurrent();
                        }
                    }
                }


                //FocusPicture(sender);
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }

        }
        private void MessegepictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                //  if (MessageBox.Show(this.Parent, "Are you sure you want to cancel Changes?", "Beep", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                //  {
                // bindingSource.CancelEdit();
                //FocusPicture(sender);
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
                //      FocusPicture(sender);
                CallPrinter?.Invoke(this, bindingSource);
                // };

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

                // FocusPicture(sender);
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
                //  FocusPicture(sender);
            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }

        }
        private void NewButton_Click(object sender, EventArgs e)
        {
            try
            {

                bindingSource.AddNew();
                NewRecordCreated?.Invoke(this, bindingSource);

            }
            catch (Exception ex)
            {
                DMEEditor.AddLogMessage("Binding Navigator", ex.Message, DateTime.Now, bindingSource.Position, ex.Message, Errors.Failed);
            }
        }
        private void BindingSource_CurrentChanged(object sender, EventArgs e)
        {
            datasourcechanged();

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
                    this.MessageLabel.Text = (Convert.ToInt32(bindingSource.Position) + 1).ToString() + " From " + bindingSource.Count.ToString();
                }
                else
                    this.MessageLabel.Text = "-";
            }
        }

        #endregion
        #region "Grid Properties"

           // private DataGridView dataGridView; // Assume this is your internal DataGridView control

            // Expose Properties
            public object DataSource
            {
                get { return dataGridView1.DataSource; }
                set { dataGridView1.DataSource = value; }
            }

            public DataGridViewColumnCollection Columns => dataGridView1.Columns;
            public DataGridViewRowCollection Rows => dataGridView1.Rows;
            public DataGridViewCell this[int col, int invert] => dataGridView1[col, invert];
            public bool AllowUserToAddRows
            {
                get { return dataGridView1.AllowUserToAddRows; }
                set { dataGridView1.AllowUserToAddRows = value; }
            }
            public bool AllowUserToDeleteRows
            {
                get { return dataGridView1.AllowUserToDeleteRows; }
                set { dataGridView1.AllowUserToDeleteRows = value; }
            }
            public bool ReadOnly
            {
                get { return dataGridView1.ReadOnly; }
                set { dataGridView1.ReadOnly = value; }
            }

            // Expose Methods
            public void ClearSelection() => dataGridView1.ClearSelection();
            public void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction) => dataGridView1.Sort(dataGridViewColumn, direction);
            public void AutoResizeColumn(int columnIndex) => dataGridView1.AutoResizeColumn(columnIndex);
            public void AutoResizeColumns() => dataGridView1.AutoResizeColumns();

            // Expose Events
            public event DataGridViewCellEventHandler CellClick
            {
                add { dataGridView1.CellClick += value; }
                remove { dataGridView1.CellClick -= value; }
            }
            public event DataGridViewCellEventHandler CellDoubleClick
            {
                add { dataGridView1.CellDoubleClick += value; }
                remove { dataGridView1.CellDoubleClick -= value; }
            }
            public event DataGridViewCellFormattingEventHandler CellFormatting
            {
                add { dataGridView1.CellFormatting += value; }
                remove { dataGridView1.CellFormatting -= value; }
            }
            public event DataGridViewRowStateChangedEventHandler RowStateChanged
            {
                add { dataGridView1.RowStateChanged += value; }
                remove { dataGridView1.RowStateChanged -= value; }
            }
            public event DataGridViewDataErrorEventHandler DataError
            {
                add { dataGridView1.DataError += value; }
                remove { dataGridView1.DataError -= value; }
            }

        // You can continue to map other properties, methods, and events as needed.


        #endregion
        private void CreateFilterControls()
        {
            filterPanel.Controls.Clear();

            PictureBox filterIcon = new PictureBox
            {
                Image = Image.FromFile(@"C:\path\to\filter_icon.png"), // Path to your filter icon
                Size = new Size(20, 20), // Adjust size as needed
                Location = new Point(0, 0),
            };

            filterPanel.Controls.Add(filterIcon);

            int xOffset = filterIcon.Width; // Start position for text boxes

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                TextBox filterTextBox = new TextBox
                {
                    Width = column.Width,
                    Location = new Point(xOffset, 0)
                };

                filterTextBox.TextChanged += FilterTextBox_TextChanged;

                filterPanel.Controls.Add(filterTextBox);

                xOffset += column.Width; // Move position for next text box
            }
        }

        private void FilterTextBox_TextChanged(object? sender, EventArgs e)
        {
            
        }

        private void DataGridView_Resize(object sender, EventArgs e)
        {
            int xOffset = filterPanel.Controls.OfType<PictureBox>().First().Width; // Start position after the icon

            for (int i = 1; i < filterPanel.Controls.Count; i++) // Start from 1 to skip the PictureBox
            {
                if (i - 1 < dataGridView1.Columns.Count)
                {
                    filterPanel.Controls[i].Width = dataGridView1.Columns[i - 1].Width;
                    filterPanel.Controls[i].Location = new Point(xOffset, 0);
                    xOffset += dataGridView1.Columns[i - 1].Width;
                }
            }
        }

       
    }
}
