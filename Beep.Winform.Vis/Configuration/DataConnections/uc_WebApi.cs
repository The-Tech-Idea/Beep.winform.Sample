
using System.Data;
using TheTechIdea.Beep.Vis;

using TheTechIdea.Util;

using Beep.Winform.Vis.Controls;
using BeepEnterprize.Vis.Module;
using TheTechIdea.Beep.Editor;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Logger;
using Beep.Winform.Vis;
using TheTechIdea.Beep.DataBase;


namespace Beep.Config.Winform.DataConnections
{
    public partial class uc_WebApi : UserControl, IDM_Addin
    {
        public uc_WebApi()
        {
            InitializeComponent();
        }
        public string AddinName { get; set; } = "Web API Connection Manager";
        public string Description { get; set; } = "Web API Connection Manager";
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
        public UnitofWork<ConnectionProperties> DBWork { get; set; }
        public ConnectionProperties cn { get; set; }
        public DatasourceCategory DatasourceCategory { get; set; }
        public string GuidID { get; set; }
        public int id { get; set; }
        public bool IsReady { get; set; }
        public bool IsRunning { get; set; }
        public bool IsNew { get; set; }

        TreeControl tree;
        IBranch branch;
        public void Run(IPassedArgs pPassedarg)
        {
            if (DBWork != null)
            {
                IsReady = true;
                if (IsNew)
                {
                    cn = new ConnectionProperties();
                    cn.Category = DatasourceCategory;
                    GuidID = cn.GuidID;
                    id = cn.ID;
                    DBWork.Create(cn);
                }
                else
                {
                    if (cn != null)
                    {
                        id = DBWork.Getindex(cn);
                    }
                    if (id > -1)
                    {
                        cn = DBWork.Units[id];
                    }
                    else
                    {
                        IsReady = false;
                        MessageBox.Show("No Connection Record Passed", "Beep");
                        return;
                    }
                }
                dataConnectionsBindingSource.DataSource = cn;
            }
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


            //foreach (var item in Enum.GetValues(typeof(DataSourceType)))
            //{
            //    databaseTypeComboBox.Items.Add(item);
            //}
            //foreach (var item in Enum.GetValues(typeof(DatasourceCategory)))
            //{
            //    DatasourceCategorycomboBox.Items.Add(item);
            //    var it = DatasourceCategorycomboBox.Items.Add(item);

            //}
            foreach (var item in DMEEditor.ConfigEditor.DataDriversClasses)
            {
                try
                {
                    if (!string.IsNullOrEmpty(item.PackageName))
                    {
                        driverNameComboBox.Items.Add(item.PackageName);
                    }


                }
                catch (Exception ex)
                {

                    Logger.WriteLog($"Error for Database connection  :{ex.Message}");
                }

            }
            try
            {
                foreach (var item in DMEEditor.ConfigEditor.DataDriversClasses)
                {
                    if (!string.IsNullOrEmpty(item.PackageName))
                    {
                        driverVersionComboBox.Items.Add(item.version);
                    }
                }
            }
            catch (Exception ex)
            {


            }

            //ds = DMEEditor.ConfigEditor.DataConnections.Where(x => x.Category == DatasourceCategory.RDBMS).ToList();
            //ConnectionProperties file = new ConnectionProperties();
            //file.FileName = "DataConnections.json";
            //file.FilePath = "./config/";
            //file.DriverName = "JSONSource";
            //file.DriverVersion = "1";
            //file.ConnectionName = "DataConnections";
            //DMEEditor.ConfigEditor.AddDataConnection(file);
            //JsonDataSource ds= (JsonDataSource)DMEEditor.GetDataSource("DataConnections");
            //ds.Openconnection();
            //DBWork = new UnitofWork<ConnectionProperties>(true, DMEEditor.Utilfunction.ConvertToObservableCollection<ConnectionProperties>( DMEEditor.ConfigEditor.DataConnections));

            dataConnectionsBindingSource.AllowNew = true;

            driverNameComboBox.SelectedValueChanged += DriverNameComboBox_SelectedValueChanged;
            // DatasourceCategorycomboBox.SelectedValueChanged += DatasourceCategorycomboBox_SelectedValueChanged;
            SaveButton.Click += SaveButton_Click;
            ExitCancelpoisonButton.Click += ExitCancelpoisonButton_Click;

        }

        private void ExitCancelpoisonButton_Click(object? sender, EventArgs e)
        {
            Form f = (Form)this.Parent;
            if (f != null)
            {
                f.Close();
            }

        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            DBWork.Update(GuidID, cn);
            MessageBox.Show("Changes Saved", "Beep");
            Form f = (Form)this.Parent;
            if (f != null)
            {
                f.Close();
            }
        }

        private void DriverNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string pkname = driverNameComboBox.Text;
            driverVersionComboBox.Items.Clear();
            foreach (var item in DMEEditor.ConfigEditor.DataDriversClasses.Where(c => c.PackageName == pkname))
            {
                driverVersionComboBox.Items.Add(item.version);
            }
        }
    }
}
