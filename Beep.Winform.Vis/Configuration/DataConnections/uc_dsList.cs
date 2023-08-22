
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Util;
using System.Data;
using Beep.Winform.Vis.Controls;
using BeepEnterprize.Vis.Module;
using TheTechIdea.Beep.Editor;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Logger;
using Beep.Winform.Vis;
using TheTechIdea.Beep.DataBase;

using TheTechIdea.Beep.Report;
using DataManagementModels.Editor;

namespace Beep.Config.Winform.DataConnections
{
    [AddinAttribute(Caption = "DataConnections", Name = "uc_dsList", misc = "Config", menu = "Configuration", displayType = DisplayType.Popup, addinType = AddinType.Control)]
    [AddinVisSchema(BranchID = 55, RootNodeName = "Configuration", Order = 55, ID = 55, BranchText = "Connection Manager", BranchType = EnumPointType.Function, IconImageName = "connections.ico", BranchClass = "ADDIN", BranchDescription = "Connection Drivers Setup Screen")]
    public partial class uc_dsList : UserControl, IDM_Addin, IAddinVisSchema
    {
        public uc_dsList()
        {
            InitializeComponent();
        }
        public string AddinName { get; set; } = "Data Connection Manager";
        public string Description { get; set; } = "Data Connection Manager";
        public string ObjectName { get; set; }
        public string ObjectType { get; set; } = "UserControl";
        public string DllName { get; set; }
        public string DllPath { get; set; }
        public string NameSpace { get; set; }
        public string ParentName { get; set; }
        public Boolean DefaultCreate { get; set; } = true;
        public IRDBSource DestConnection { get; set; }
        public DataSet Dset { get; set; }
        public IErrorsInfo ErrorObject { get; set; }
        public IDMLogger Logger { get; set; }
        public IRDBSource SourceConnection { get; set; }
        public EntityStructure EntityStructure { get; set; }
        public string EntityName { get; set; }
        public IPassedArgs Passedarg { get; set; }
        public IUtil util { get; set; }
        public VisManager Visutil { get; set; }
        public IDMEEditor DMEEditor { get; set; }
        #region "IAddinVisSchema"
        public string RootNodeName { get; set; } = "Configuration";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 1;
        public int ID { get; set; } = 1;
        public string BranchText { get; set; } = "Connection Manager";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Entity;
        public int BranchID { get; set; } = 1;
        public string IconImageName { get; set; } = "connections.ico";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "";
        public string BranchClass { get; set; } = "ADDIN";
        #endregion "IAddinVisSchema"
        UnitofWork<ConnectionProperties> DBWork { get; set; }
        TreeControl tree;
        IBranch branch;
        string selectedCategory;
        int selectedCategoryValue;
        DatasourceCategory selectedCategoryItem;
        public ConnectionProperties cn { get; set; }
        public int id { get; set; }
        public string GuidID { get; set; }
        public void Run(IPassedArgs pPassedarg)
        {

        }

        public void SetConfig(IDMEEditor pDMEEditor, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs obj, IErrorsInfo per)
        {
            Passedarg = obj;
            Visutil = (VisManager)obj.Objects.Where(c => c.Name == "VISUTIL").FirstOrDefault().obj;
            Logger = plogger;
            DMEEditor = pDMEEditor;
            //     DataSourceCategoryType = args[0];
            ErrorObject = per;
            tree = (TreeControl)Visutil.Tree;
            if (tree != null)
            {
                branch = tree.Branches[tree.Branches.FindIndex(x => x.BranchClass == "RDBMS" && x.BranchType == EnumPointType.Root)];
            }
            else
                branch = null;

            //foreach (var item in Enum.GetValues(typeof(DatasourceCategory)))
            //{
            //    DatasourceCategorycomboBox.Items.Add(item);

            //}
          
            DatasourceCategorycomboBox.DataSource = Enum.GetValues(typeof(DatasourceCategory));
            DBWork = new UnitofWork<ConnectionProperties>(DMEEditor,true, new ObservableBindingList<ConnectionProperties>(DMEEditor.ConfigEditor.DataConnections),"GuidID");
            DBWork.PrimaryKey = "GuidID";
           // beepGrid1.DataSource = DBWork.Units;
           
            // Create a new DataGridViewImageColumn
            DataGridViewImageColumn editButtonColumn = new DataGridViewImageColumn();
            editButtonColumn.Name = "EditButton";
            editButtonColumn.HeaderText = "Edit";
             editButtonColumn.Image =  Beep.Winform.Vis.Properties.Resources._221_database_6; // Add your image from resources here
            editButtonColumn.ImageLayout = DataGridViewImageCellLayout.Normal;

            int columnIndex = 0;  // Choose the index where you want the button column to be.
            if (poisonDataGridView1.Columns["EditButton"] == null)  // Ensure this column is not already added.
            {
                poisonDataGridView1.Columns.Insert(columnIndex, editButtonColumn);
            }
            //List<ConnectionProperties> retval = (List<ConnectionProperties>)DMEEditor.ConfigEditor.DataConnections;//.Where(p => p.Category.ToString().Equals(selectedCategory, StringComparison.InvariantCultureIgnoreCase)).ToList();
            selectedCategoryValue = 0;
            setfilterdData();
            DatasourceCategorycomboBox.SelectedValueChanged += DatasourceCategorycomboBox_SelectedValueChanged;
            poisonDataGridView1.CellContentClick += PoisonDataGridView1_CellContentClick;
            CreatepoisonButton.Click += CreatepoisonButton_Click;
            SaveChangespoisonButton.Click += SaveChangespoisonButton_Click;
        }

        private void SaveChangespoisonButton_Click(object? sender, EventArgs e)
        {
            DMEEditor.ConfigEditor.DataConnections= DBWork.Units.ToList();
            DMEEditor.ConfigEditor.SaveDataconnectionsValues();
        }

        private void PoisonDataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // If the clicked column is our button column
            if (e.ColumnIndex == poisonDataGridView1.Columns["EditButton"].Index && e.RowIndex >= 0)
            {
                // Get the clicked row
                DataGridViewRow currentRow = poisonDataGridView1.Rows[e.RowIndex];
                // Now you have the clicked row, and you can get its cells values.
                // Get the value of the hidden "GuidID" column
                // Get the DataBoundItem as YourClass
                var item = currentRow.DataBoundItem as ConnectionProperties;

                // If the cast was successful and item is not null
                if (item != null)
                {
                    GuidID = item.GuidID;
                    cn = item;

                    CreateEditConnection(false);
                }
             
            }
        }

        private void CreatepoisonButton_Click(object? sender, EventArgs e)
        {
            CreateEditConnection(true);

        }
        private void CreateEditConnection(bool IsNew)
        {
            Form fbfrm = new Form();
            switch (selectedCategoryItem)
            {
                case DatasourceCategory.FILE:
                    uc_File filectl = new uc_File();
                    filectl.SetConfig(DMEEditor, DMEEditor.Logger, DMEEditor.Utilfunction, new string[] { }, DMEEditor.Passedarguments, DMEEditor.ErrorObject);
                    filectl.IsNew = IsNew;
                    fbfrm.Size = new Size() { Height = filectl.Height + 5, Width = filectl.Width + 5 };
                    filectl.Dock = DockStyle.Fill;
                    fbfrm.Controls.Add(filectl);
                    filectl.DBWork = DBWork;
                    if (!IsNew)
                    {
                        filectl.id = id;
                        filectl.GuidID = GuidID;
                        filectl.cn = cn;
                    }

                    filectl.Run(DMEEditor.Passedarguments);
                    fbfrm.ShowDialog(this);
                    break;
               
                case DatasourceCategory.RDBMS:
                    uc_Database dbctl = new uc_Database();
                    dbctl.SetConfig(DMEEditor, DMEEditor.Logger, DMEEditor.Utilfunction, new string[] { }, DMEEditor.Passedarguments, DMEEditor.ErrorObject);
                    dbctl.IsNew = IsNew;
                    fbfrm.Size = new Size() { Height = dbctl.Height + 5, Width = dbctl.Width + 5 };
                    dbctl.Dock = DockStyle.Fill;
                    fbfrm.Controls.Add(dbctl);
                    dbctl.DBWork = DBWork;
                    if (!IsNew)
                    {
                        dbctl.id = id;
                        dbctl.GuidID = GuidID;
                        dbctl.cn = cn;
                    }

                    dbctl.Run(DMEEditor.Passedarguments);
                    fbfrm.ShowDialog(this);
                    break;
                default:
                    uc_WebApi webapictl = new uc_WebApi();
                    webapictl.SetConfig(DMEEditor, DMEEditor.Logger, DMEEditor.Utilfunction, new string[] { }, DMEEditor.Passedarguments, DMEEditor.ErrorObject);
                    webapictl.IsNew = IsNew;
                    fbfrm.Size = new Size() { Height = webapictl.Height + 5, Width = webapictl.Width + 5 };
                    webapictl.Dock = DockStyle.Fill;
                    fbfrm.Controls.Add(webapictl);
                    webapictl.DBWork = DBWork;
                    webapictl.DatasourceCategory = (DatasourceCategory)selectedCategoryValue;
                    if (!IsNew)
                    {
                        webapictl.id = id;
                        webapictl.GuidID = GuidID;
                        webapictl.cn = cn;
                    }

                    webapictl.Run(DMEEditor.Passedarguments);
                    fbfrm.ShowDialog(this);
                    break;
                   
            }
            setfilterdData();
            poisonDataGridView1.Refresh();
        }
        private void DatasourceCategorycomboBox_SelectedValueChanged(object? sender, EventArgs e)
        {
            selectedCategory = DatasourceCategorycomboBox.Text;
            selectedCategoryItem = (DatasourceCategory)DatasourceCategorycomboBox.SelectedItem;
            selectedCategoryValue = (int)selectedCategoryItem;

            // Perform the filtering on the original data
            setfilterdData();
            poisonDataGridView1.Refresh();
        }
       private void setfilterdData()
        {
            List<AppFilter> filters = new List<AppFilter>();
            filters.Add(new AppFilter() { FieldName = "Category", FilterValue = selectedCategoryValue.ToString(),Operator="=" });
            var retval= DBWork.Get(filters);
            retval.Wait();
            poisonDataGridView1.DataSource = retval.Result;
            poisonDataGridView1.Refresh();
        }

    }
}
