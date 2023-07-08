
using Beep.Winform.Controls;

namespace Beep.Config.Winform.DataConnections
{
    partial class uc_webapiHeaders
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.headersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.headersDataGridView = new ReaLTaiizor.Controls.PoisonDataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uc_bindingNavigator1 = new Beep.Winform.Controls.uc_bindingNavigator();
            this.poisonPanel2 = new ReaLTaiizor.Controls.PoisonPanel();
            this.poisonLabel1 = new ReaLTaiizor.Controls.PoisonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.headersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headersDataGridView)).BeginInit();
            this.poisonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // headersBindingSource
            // 
            this.headersBindingSource.DataSource = typeof(TheTechIdea.Util.WebApiHeader);
            // 
            // headersDataGridView
            // 
            this.headersDataGridView.AllowUserToResizeRows = false;
            this.headersDataGridView.AutoGenerateColumns = false;
            this.headersDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.headersDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.headersDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.headersDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.headersDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.headersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.headersDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.headersDataGridView.DataSource = this.headersBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.headersDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.headersDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headersDataGridView.EnableHeadersVisualStyles = false;
            this.headersDataGridView.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.headersDataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.headersDataGridView.Location = new System.Drawing.Point(0, 58);
            this.headersDataGridView.Name = "headersDataGridView";
            this.headersDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.headersDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.headersDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.headersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.headersDataGridView.Size = new System.Drawing.Size(582, 511);
            this.headersDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "headername";
            this.dataGridViewTextBoxColumn1.HeaderText = "headername";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "headervalue";
            this.dataGridViewTextBoxColumn2.HeaderText = "headervalue";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // uc_bindingNavigator1
            // 
            this.uc_bindingNavigator1.AddinName = null;
            this.uc_bindingNavigator1.BackColor = System.Drawing.Color.White;
            this.uc_bindingNavigator1.bindingSource = null;
            this.uc_bindingNavigator1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uc_bindingNavigator1.ButtonBorderSize = 0;
            this.uc_bindingNavigator1.CausesValidation = false;
            this.uc_bindingNavigator1.ControlHeight = 30;
            this.uc_bindingNavigator1.DefaultCreate = false;
            this.uc_bindingNavigator1.Description = null;
            this.uc_bindingNavigator1.DllName = null;
            this.uc_bindingNavigator1.DllPath = null;
            this.uc_bindingNavigator1.DMEEditor = null;
            this.uc_bindingNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uc_bindingNavigator1.EntityName = null;
            this.uc_bindingNavigator1.EntityStructure = null;
            this.uc_bindingNavigator1.ErrorObject = null;
            this.uc_bindingNavigator1.HightlightColor = System.Drawing.Color.Red;
            this.uc_bindingNavigator1.Location = new System.Drawing.Point(0, 569);
            this.uc_bindingNavigator1.Logger = null;
            this.uc_bindingNavigator1.Name = "uc_bindingNavigator1";
            this.uc_bindingNavigator1.NameSpace = null;
            this.uc_bindingNavigator1.ObjectName = null;
            this.uc_bindingNavigator1.ObjectType = null;
            this.uc_bindingNavigator1.ParentName = null;
            this.uc_bindingNavigator1.Passedarg = null;
            this.uc_bindingNavigator1.SelectedColor = System.Drawing.Color.Green;
            this.uc_bindingNavigator1.Size = new System.Drawing.Size(582, 31);
            this.uc_bindingNavigator1.TabIndex = 2;
            this.uc_bindingNavigator1.VerifyDelete = true;
            // 
            // poisonPanel2
            // 
            this.poisonPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.poisonPanel2.Controls.Add(this.poisonLabel1);
            this.poisonPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.poisonPanel2.HorizontalScrollbarBarColor = true;
            this.poisonPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.poisonPanel2.HorizontalScrollbarSize = 10;
            this.poisonPanel2.Location = new System.Drawing.Point(0, 0);
            this.poisonPanel2.Name = "poisonPanel2";
            this.poisonPanel2.Size = new System.Drawing.Size(582, 58);
            this.poisonPanel2.TabIndex = 5;
            this.poisonPanel2.UseStyleColors = true;
            this.poisonPanel2.VerticalScrollbarBarColor = true;
            this.poisonPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.poisonPanel2.VerticalScrollbarSize = 10;
            // 
            // poisonLabel1
            // 
            this.poisonLabel1.AutoSize = true;
            this.poisonLabel1.FontSize = ReaLTaiizor.Extension.Poison.PoisonLabelSize.Tall;
            this.poisonLabel1.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            this.poisonLabel1.Location = new System.Drawing.Point(130, 13);
            this.poisonLabel1.Name = "poisonLabel1";
            this.poisonLabel1.Size = new System.Drawing.Size(284, 25);
            this.poisonLabel1.TabIndex = 3;
            this.poisonLabel1.Text = "Web API Header Configurations";
            this.poisonLabel1.UseStyleColors = true;
            // 
            // uc_webapiHeaders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.headersDataGridView);
            this.Controls.Add(this.poisonPanel2);
            this.Controls.Add(this.uc_bindingNavigator1);
            this.Name = "uc_webapiHeaders";
            this.Size = new System.Drawing.Size(582, 600);
            ((System.ComponentModel.ISupportInitialize)(this.headersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headersDataGridView)).EndInit();
            this.poisonPanel2.ResumeLayout(false);
            this.poisonPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource headersBindingSource;
        private ReaLTaiizor.Controls.PoisonDataGridView headersDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private uc_bindingNavigator uc_bindingNavigator1;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel2;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1;
    }
}
