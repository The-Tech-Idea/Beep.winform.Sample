using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeepEnterprize.Winform.Vis.Controls;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Workflow.Mapping;
using TheTechIdea.Logger;
using TheTechIdea.Util;

namespace BeepEnterprize.Winform.Vis.ETL.ImportData
{
    [AddinAttribute(Caption = "Import Data", Name = "Frm_ImportData", misc = "ImportDataManager", addinType = AddinType.Form)]
    public partial class Frm_ImportData : System.Windows.Forms.Form,IDM_Addin
    {
        public Frm_ImportData()
        {
            InitializeComponent();
        }

        public string ParentName { get ; set ; }
        public string ObjectName { get; set; } = "Frm_ImportData";
        public string ObjectType { get; set; } = "Form";
        public string AddinName { get; set; } = "Import Data";
        public string Description { get; set; } = "Import Data";
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
        IDataSource ds;
        Type CurrentType;
        ImportDataManager importDataManager;

        uc_FilterData uc_Filter;
        uc_MapFields uc_Map;
        uc_RunImport uc_Run;
        uc_SelectField uc_Select;
        uc_addeditImportData uc_addedit;

        public BindingList<EntityDataMap_DTL> MappedEntities { get; set; } = new BindingList<EntityDataMap_DTL>();

        int pageindex = 0;
        int totpages = 4;

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
            Passedarg = e;
            uc_Filter = new uc_FilterData();
            uc_Map = new uc_MapFields();
            uc_Run = new uc_RunImport();
            uc_Select = new uc_SelectField();
            uc_addedit = new uc_addeditImportData();
           
         
            this.MainPanel.Controls.Add(uc_Filter);
            uc_Filter.Dock = DockStyle.Fill;
            this.MainPanel.Controls.Add(uc_Map);
            uc_Map.Dock = DockStyle.Fill;
            this.MainPanel.Controls.Add(uc_Run);
            uc_Run.Dock = DockStyle.Fill;
            this.MainPanel.Controls.Add(uc_Select);
            uc_Select.Dock = DockStyle.Fill;
            this.MainPanel.Controls.Add(uc_addedit);
            uc_addedit.Dock = DockStyle.Fill;
            pageindex = 0;
            Showpage();
            dataConnectionsBindingSource = importDataManager.dataConnectionsBindingSource;
          
            entityDataMapBindingSource = importDataManager.entityDataMapBindingSource;
           
            mappedEntitiesBindingSource = entityDataMapBindingSource;
            dataGridView1.DataSource = mappedEntitiesBindingSource;
            dataGridView1.Refresh();

            uc_Filter.SetConfig(DMEEditor, plogger, putil, args, e, per);
            uc_Map.SetConfig(DMEEditor, plogger, putil, args, e, per);
            uc_Run.SetConfig(DMEEditor, plogger, putil, args, e, per);
            uc_Select.SetConfig(DMEEditor, plogger, putil, args, e, per);
            uc_addedit.SetConfig(DMEEditor, plogger, putil, args, e, per);
            uc_Filter.importDataManager = importDataManager;
            uc_Map.importDataManager = importDataManager;
            uc_Run.importDataManager = importDataManager;
            uc_Select.importDataManager = importDataManager;


            if (importDataManager.Mapping.MappedEntities.Count > 0)
            {
               if (string.IsNullOrEmpty(importDataManager.Mapping.MappedEntities[0].EntityName))
                {
                    mappedEntitiesBindingSource.RemoveCurrent();
                }
            }
            //-------------- Main Buttons
            NewImportButton.Click += NewImportButton_Click;
            NextButton.Click += NextButton_Click;
            PreviousButton.Click += PreviousButton_Click;
            HomeNavButton.Click += HomeNavButton_Click;
            FinishButton.Click += FinishButton_Click;
           
            //----------------------------
            //--------------Left Bar Buttons
            SelectFieldButton.Click += SelectFieldButton_Click;
            FilterDataButton.Click += FilterDataButton_Click;
            MapFieldsButton.Click += MapFieldsButton_Click;
            ImportButton.Click += ImportButton_Click;
            FinishandSaveButton.Click += FinishandSaveButton_Click;
            //--------------
           
            dataGridView1.DataError += DataGridView1_DataError;
            EntityLabel.Text = importDataManager.EntityStructure.EntityName;
            DataSourceLabel.Text = importDataManager.EntityStructure.DataSourceID;
            //----------------------------
            mappedEntitiesBindingSource.AddingNew += MappedEntitiesBindingSource_AddingNew;
            mappedEntitiesBindingSource.BindingComplete += MappedEntitiesBindingSource_BindingComplete;
            SourceDatasourcecomboBox.SelectedValueChanged += SourceDatasourcecomboBox_SelectedValueChanged;


            EntitysourcecomboBox.Validating += EntitysourcecomboBox_Validating;
            mappedEntitiesBindingSource.AddingNew += MappedEntitiesBindingSource_AddingNew;
            this.SaveNewbutton.Click += SaveNewbutton_Click;
            this.Text = "Import Data Manager";

            uc_bindingNavigator1.bindingSource = mappedEntitiesBindingSource;
            uc_bindingNavigator1.CallPrinter += Uc_bindingNavigator1_CallPrinter;
            uc_bindingNavigator1.SendMessage += Uc_bindingNavigator1_SendMessage;
            uc_bindingNavigator1.ShowSearch += Uc_bindingNavigator1_ShowSearch;
            uc_bindingNavigator1.SetConfig(DMEEditor, plogger, putil, args, e, per);
            
        }

        private void MappedEntitiesBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate
                 && e.Exception == null)
                e.Binding.BindingManagerBase.EndCurrentEdit();
        }

        private void Uc_bindingNavigator1_ShowSearch(object sender, BindingSource e)
        {
           
        }

        private void Uc_bindingNavigator1_SendMessage(object sender, BindingSource e)
        {
            throw new NotImplementedException();
        }

        private void Uc_bindingNavigator1_CallPrinter(object sender, BindingSource e)
        {
            throw new NotImplementedException();
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        //----- Get New Entities for Data Source
        private void SaveNewbutton_Click(object sender, EventArgs e)
        {
            try
            {
                importDataManager.Save();
                MessageBox.Show("Mapping Save Successfully", "Beep");
                dataGridView1.Refresh();
                showHome();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error: Saving Mapping", "Beep", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DMEEditor.AddLogMessage("Import Mapping", "Error: Saving Mapping : " + ex.Message, DateTime.Now, 0, ex.Message, Errors.Failed);

            }
        }
        private void MappedEntitiesBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            e.NewObject = importDataManager.AddEntitytoMappedEntities(SourceDatasourcecomboBox.Text, EntitysourcecomboBox.Text, importDataManager.EntityStructure);
            showImportedit();
        }
        private void EntitysourcecomboBox_Validating(object sender, CancelEventArgs e)
        {
            mappedEntitiesBindingSource.ResetBindings(false);
        }
        private void SourceDatasourcecomboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SourceDatasourcecomboBox.Text))
            {
                IDataSource ds = DMEEditor.GetDataSource(SourceDatasourcecomboBox.Text);
                if (ds != null)
                {
                    ds.Openconnection();
                    if (ds.ConnectionStatus == ConnectionState.Open)
                    {
                        ds.GetEntitesList();
                        EntitysourcecomboBox.DataSource = ds.EntitiesNames;
                    }
                    else
                    {
                        EntitysourcecomboBox.DataSource = null;
                        MessageBox.Show(this.ParentForm, "Beep", $"Could Not Open Data Source ({SourceDatasourcecomboBox.Text})", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DMEEditor.AddLogMessage("Import Mapping", $"Could Not Open Data Source ({SourceDatasourcecomboBox.Text})", DateTime.Now, 0, $"Could Not Open Data Source ({SourceDatasourcecomboBox.Text})", Errors.Failed);
                    }
                }
                else
                {
                    EntitysourcecomboBox.DataSource = null;
                    MessageBox.Show(this.ParentForm, "Beep", $"Could Not Get Data Source ({SourceDatasourcecomboBox.Text})", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DMEEditor.AddLogMessage("Import Mapping", $"Could Not Get Data Source ({SourceDatasourcecomboBox.Text})", DateTime.Now, 0, $"Could Not Open Data Source ({SourceDatasourcecomboBox.Text})", Errors.Failed);

                }

            }
        }
        //-------------------------------------
        #region "Left Menu Buttons Click"
        private void ImportButton_Click(object sender, EventArgs e)
        {
            showRun();
        }

        private void MapFieldsButton_Click(object sender, EventArgs e)
        {
            showMapfield();
        }

        private void FilterDataButton_Click(object sender, EventArgs e)
        {
            showFilterdata();
        }

        private void SelectFieldButton_Click(object sender, EventArgs e)
        {
            showSelectField();
        }
        private void FinishandSaveButton_Click(object sender, EventArgs e)
        {
            importDataManager.Save();
            this.Close();
        }
        #endregion
        #region "Bottom Bar buttons"
        private void FinishButton_Click(object sender, EventArgs e)
        {
            importDataManager.Save();
            this.Close();

        }
        private void HomeNavButton_Click(object sender, EventArgs e)
        {
            showHome();
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (pageindex > 0)
            {
                pageindex -= 1;
            }
            Showpage();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (pageindex < 5)
            {
                pageindex += 1;
            }
            Showpage();
        }

        private void NewImportButton_Click(object sender, EventArgs e)
        {
            mappedEntitiesBindingSource.AddNew();

        }
        #endregion
        #region "Wizard Controls Functions"
        private void showHome()
        {
            uc_Filter.Hide();
            uc_Map.Hide();
            uc_addedit.Hide();
            uc_Run.Hide();
            uc_Select.Hide();
            addeditpanelpanel.Hide();
        }
        private void showSelectField()
        {
            uc_Filter.Hide();
            uc_Map.Hide();
            uc_Run.Hide();
            uc_Select.Show();
            uc_addedit.Hide();
            uc_Select.BringToFront();
            uc_Select.Run(Passedarg);
        }
        private void showMapfield()
        {
            uc_Filter.Hide();
            uc_Map.Show();
            uc_Map.BringToFront();
            uc_Map.Run(Passedarg);
            uc_Run.Hide();
            uc_addedit.Hide();
            uc_Select.Hide();
        }
        private void showFilterdata()
        {
            uc_Filter.Show();
            uc_Filter.BringToFront();
            uc_Filter.Run(Passedarg);
            uc_Map.Hide();
            uc_addedit.Hide();
            uc_Run.Hide();
            uc_Select.Hide();
        }
        private void showRun()
        {
            uc_Filter.Hide();
            uc_Map.Hide();
            uc_addedit.Hide();
            uc_Select.Hide();
            uc_Run.Show();
            uc_Run.BringToFront();
            uc_Run.Run(Passedarg);

        }
        private void showImportedit()
        {
            uc_Filter.Hide();
            uc_Map.Hide();
            uc_Run.Hide();
            uc_Select.Hide();

            addeditpanelpanel.Show();
            addeditpanelpanel.BringToFront();
            //  uc_addedit.Run(Passedarg);

        }
        private void Showpage()
        {
            switch (pageindex)
            {
                case 0:
                    showHome();
                    break;

                case 1:
                    showSelectField();
                    break;
                case 2:
                    showMapfield();
                    break;
                case 3:
                    showFilterdata();
                    break;
                case 4:
                    showRun();
                    break;
                case 99:
                    showImportedit();
                    break;

                default:
                    break;
            }
        }
        #endregion



    }
}
