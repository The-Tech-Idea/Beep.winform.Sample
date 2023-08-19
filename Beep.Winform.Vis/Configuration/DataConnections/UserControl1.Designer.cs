namespace Beep.Winform.Vis.Configuration.DataConnections
{
    partial class UserControl1
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.uc_BeepGrid1 = new Beep.Winform.Controls.uc_BeepGrid();
            this.dataConnectionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // uc_BeepGrid1
            // 
            this.uc_BeepGrid1.AddinName = null;
            this.uc_BeepGrid1.AllowDrop = true;
            this.uc_BeepGrid1.AllowUserToAddRows = true;
            this.uc_BeepGrid1.AllowUserToDeleteRows = true;
            this.uc_BeepGrid1.bindingSource = this.dataConnectionsBindingSource;
            this.uc_BeepGrid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uc_BeepGrid1.DataSource = this.dataConnectionsBindingSource;
            this.uc_BeepGrid1.DefaultCreate = false;
            this.uc_BeepGrid1.Description = null;
            this.uc_BeepGrid1.DllName = null;
            this.uc_BeepGrid1.DllPath = null;
            this.uc_BeepGrid1.DMEEditor = null;
            this.uc_BeepGrid1.EntityName = null;
            this.uc_BeepGrid1.EntityStructure = null;
            this.uc_BeepGrid1.ErrorObject = null;
            this.uc_BeepGrid1.Location = new System.Drawing.Point(18, 78);
            this.uc_BeepGrid1.Logger = null;
            this.uc_BeepGrid1.Name = "uc_BeepGrid1";
            this.uc_BeepGrid1.NameSpace = null;
            this.uc_BeepGrid1.ObjectName = null;
            this.uc_BeepGrid1.ObjectType = null;
            this.uc_BeepGrid1.ParentName = null;
            this.uc_BeepGrid1.Passedarg = null;
            this.uc_BeepGrid1.ReadOnly = false;
            this.uc_BeepGrid1.Size = new System.Drawing.Size(704, 500);
            this.uc_BeepGrid1.TabIndex = 0;
            this.uc_BeepGrid1.VerifyDelete = true;
            // 
            // dataConnectionsBindingSource
            // 
            this.dataConnectionsBindingSource.DataSource = typeof(TheTechIdea.Util.ConnectionProperties);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uc_BeepGrid1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(781, 644);
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Winform.Controls.uc_BeepGrid uc_BeepGrid1;
        private BindingSource dataConnectionsBindingSource;
    }
}
