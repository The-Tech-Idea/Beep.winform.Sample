namespace Beep.Winform.Vis
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
            this.beepGrid1 = new TheTechIdea.Beep.Winform.Controls.Grid.BeepGrid();
            this.dataConnectionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // beepGrid1
            // 
            this.beepGrid1.AddinName = null;
            this.beepGrid1.AllowDrop = true;
            this.beepGrid1.AllowUserToAddRows = true;
            this.beepGrid1.AllowUserToDeleteRows = true;
            this.beepGrid1.bindingSource = null;
            this.beepGrid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.beepGrid1.DataSource = null;
            this.beepGrid1.DefaultCreate = false;
            this.beepGrid1.Description = null;
            this.beepGrid1.DllName = null;
            this.beepGrid1.DllPath = null;
            this.beepGrid1.DMEEditor = null;
            this.beepGrid1.EntityName = null;
            this.beepGrid1.EntityStructure = null;
            this.beepGrid1.ErrorObject = null;
            this.beepGrid1.Location = new System.Drawing.Point(0, 120);
            this.beepGrid1.Logger = null;
            this.beepGrid1.Name = "beepGrid1";
            this.beepGrid1.NameSpace = null;
            this.beepGrid1.ObjectName = null;
            this.beepGrid1.ObjectType = null;
            this.beepGrid1.ParentName = null;
            this.beepGrid1.Passedarg = null;
            this.beepGrid1.ReadOnly = false;
            this.beepGrid1.Size = new System.Drawing.Size(735, 348);
            this.beepGrid1.TabIndex = 0;
            this.beepGrid1.VerifyDelete = true;
            // 
            // dataConnectionsBindingSource
            // 
            this.dataConnectionsBindingSource.DataSource = typeof(TheTechIdea.Util.ConnectionProperties);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.beepGrid1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(755, 596);
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TheTechIdea.Beep.Winform.Controls.Grid.BeepGrid beepGrid1;
        private BindingSource dataConnectionsBindingSource;
    }
}
