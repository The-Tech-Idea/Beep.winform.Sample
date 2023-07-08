
using Beep.Winform.Controls;
using DataManagementModels.ConfigUtil;

namespace Beep.Winform.Vis
{
    partial class uc_events
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
            this.eventsDataGridView = new ReaLTaiizor.Controls.PoisonDataGridView();
            this.uc_bindingNavigator1 = new Beep.Winform.Controls.uc_bindingNavigator();
            this.poisonStyleManager1 = new ReaLTaiizor.Manager.PoisonStyleManager(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.poisonPanel2 = new ReaLTaiizor.Controls.PoisonPanel();
            this.poisonLabel1 = new ReaLTaiizor.Controls.PoisonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.eventsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.poisonStyleManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsBindingSource)).BeginInit();
            this.poisonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // eventsDataGridView
            // 
            this.eventsDataGridView.AllowUserToResizeRows = false;
            this.eventsDataGridView.AutoGenerateColumns = false;
            this.eventsDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.eventsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.eventsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.eventsDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.eventsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eventsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.eventsDataGridView.DataSource = this.eventsBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.eventsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.eventsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsDataGridView.EnableHeadersVisualStyles = false;
            this.eventsDataGridView.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.eventsDataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.eventsDataGridView.Location = new System.Drawing.Point(0, 58);
            this.eventsDataGridView.Name = "eventsDataGridView";
            this.eventsDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventsDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.eventsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.eventsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.eventsDataGridView.Size = new System.Drawing.Size(557, 410);
            this.eventsDataGridView.TabIndex = 1;
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
            this.uc_bindingNavigator1.Location = new System.Drawing.Point(0, 468);
            this.uc_bindingNavigator1.Logger = null;
            this.uc_bindingNavigator1.Name = "uc_bindingNavigator1";
            this.uc_bindingNavigator1.NameSpace = null;
            this.uc_bindingNavigator1.ObjectName = null;
            this.uc_bindingNavigator1.ObjectType = null;
            this.uc_bindingNavigator1.ParentName = null;
            this.uc_bindingNavigator1.Passedarg = null;
            this.uc_bindingNavigator1.SelectedColor = System.Drawing.Color.Green;
            this.uc_bindingNavigator1.Size = new System.Drawing.Size(557, 31);
            this.uc_bindingNavigator1.TabIndex = 2;
            this.uc_bindingNavigator1.VerifyDelete = true;
            // 
            // poisonStyleManager1
            // 
            this.poisonStyleManager1.Owner = this;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "EventName";
            this.dataGridViewTextBoxColumn1.HeaderText = "EventName";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // eventsBindingSource
            // 
            this.eventsBindingSource.DataSource = typeof(DataManagementModels.ConfigUtil.Event);
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
            this.poisonPanel2.Size = new System.Drawing.Size(557, 58);
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
            this.poisonLabel1.Location = new System.Drawing.Point(27, 14);
            this.poisonLabel1.Name = "poisonLabel1";
            this.poisonLabel1.Size = new System.Drawing.Size(265, 25);
            this.poisonLabel1.TabIndex = 3;
            this.poisonLabel1.Text = "System Events Configurations";
            this.poisonLabel1.UseStyleColors = true;
            // 
            // uc_events
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventsDataGridView);
            this.Controls.Add(this.poisonPanel2);
            this.Controls.Add(this.uc_bindingNavigator1);
            this.Name = "uc_events";
            this.Size = new System.Drawing.Size(557, 499);
            ((System.ComponentModel.ISupportInitialize)(this.eventsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.poisonStyleManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsBindingSource)).EndInit();
            this.poisonPanel2.ResumeLayout(false);
            this.poisonPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource eventsBindingSource;
        private ReaLTaiizor.Controls.PoisonDataGridView eventsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private uc_bindingNavigator uc_bindingNavigator1;
        private ReaLTaiizor.Manager.PoisonStyleManager poisonStyleManager1;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel2;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1;
    }
}
