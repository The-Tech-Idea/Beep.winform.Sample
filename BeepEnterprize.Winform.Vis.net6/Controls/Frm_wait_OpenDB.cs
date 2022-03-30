using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeepEnterprize.Vis.Module;
using TheTechIdea;
using TheTechIdea.Beep;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Logger;
using TheTechIdea.Util;

namespace BeepEnterprize.Winform.Vis.Controls
{
    [AddinAttribute(Caption = "Wait for Connection", Name = "Frm_wait_OpenDB", misc = "None", menu = "", addinType = AddinType.Form, displayType = DisplayType.Popup)]
    public partial class Frm_wait_OpenDB : Form,IDM_Addin
    {
        public Frm_wait_OpenDB()
        {
            InitializeComponent();
        }

        public string ParentName { get ; set ; }
        public string ObjectName { get ; set ; }
        public string AddinName { get; set; } = "Wait for Connection";
        public string ObjectType { get; set; } = "Form";
        public string Description { get; set; } = "Wait Form for a Task";
        public bool DefaultCreate { get; set; } = true;
        public string DllPath { get ; set ; }
        public string DllName { get ; set ; }
        public string NameSpace { get ; set ; }
        public IErrorsInfo ErrorObject { get ; set ; }
        public IDMLogger Logger { get ; set ; }
        public IDMEEditor DMEEditor { get ; set ; }
        public EntityStructure EntityStructure { get ; set ; }
        public string EntityName { get ; set ; }
        public IPassedArgs Passedarg { get ; set ; }
        public IVisManager Visutil { get; set; }
     
        public void Run(IPassedArgs pPassedarg)
        {
            if( pPassedarg != null)
            {
                if(pPassedarg.DatasourceName != null)
                {
                    if (DMEEditor != null)
                    {
                        
                        this.Title.Text =  "DataSource : " + pPassedarg.DatasourceName;
                        this.messege.Text = "Connecting ..."+ Environment.NewLine;

                    }
                }
            }
            
        }
        public void CloseForm()
        {

              System.Threading.Thread.Sleep(1000);
            if (this.IsHandleCreated)
            {
                Close();
            }
            
        }
       
        public void SetConfig(IDMEEditor pbl, IDMLogger plogger, IUtil putil, string[] args, IPassedArgs e, IErrorsInfo per)
        {
            Passedarg = e;
            Logger = plogger;
            ErrorObject = per;
            DMEEditor = pbl;
            
        //    Visutil = (IVisManager)e.Objects.Where(c => c.Name == "VISUTIL").FirstOrDefault().obj;
        }
    }
}
