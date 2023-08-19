namespace Beep.Winform.Controls
{
    partial class uc_BeepGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Toppanel = new System.Windows.Forms.Panel();
            this.Titlelabel = new System.Windows.Forms.Label();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.Sharebutton = new System.Windows.Forms.Button();
            this.Printbutton = new System.Windows.Forms.Button();
            this.Filterbutton = new System.Windows.Forms.Button();
            this.Bottompanel = new System.Windows.Forms.Panel();
            this.uc_bindingNavigator1 = new Beep.Winform.Controls.uc_bindingNavigator();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Toppanel.SuspendLayout();
            this.Bottompanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.filterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Toppanel
            // 
            this.Toppanel.BackColor = System.Drawing.Color.White;
            this.Toppanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Toppanel.Controls.Add(this.Titlelabel);
            this.Toppanel.Controls.Add(this.MessageLabel);
            this.Toppanel.Controls.Add(this.Sharebutton);
            this.Toppanel.Controls.Add(this.Printbutton);
            this.Toppanel.Controls.Add(this.Filterbutton);
            this.Toppanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Toppanel.Location = new System.Drawing.Point(0, 0);
            this.Toppanel.Name = "Toppanel";
            this.Toppanel.Size = new System.Drawing.Size(735, 22);
            this.Toppanel.TabIndex = 0;
            // 
            // Titlelabel
            // 
            this.Titlelabel.Location = new System.Drawing.Point(3, 1);
            this.Titlelabel.Name = "Titlelabel";
            this.Titlelabel.Size = new System.Drawing.Size(262, 20);
            this.Titlelabel.TabIndex = 9;
            this.Titlelabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MessageLabel
            // 
            this.MessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageLabel.Location = new System.Drawing.Point(271, 1);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(382, 20);
            this.MessageLabel.TabIndex = 8;
            this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Sharebutton
            // 
            this.Sharebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sharebutton.BackgroundImage = global::Beep.Winform.Vis.Properties.Resources.messages;
            this.Sharebutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Sharebutton.Location = new System.Drawing.Point(661, 1);
            this.Sharebutton.Name = "Sharebutton";
            this.Sharebutton.Size = new System.Drawing.Size(20, 20);
            this.Sharebutton.TabIndex = 2;
            this.Sharebutton.UseVisualStyleBackColor = true;
            // 
            // Printbutton
            // 
            this.Printbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Printbutton.BackgroundImage = global::Beep.Winform.Vis.Properties.Resources._015_printer;
            this.Printbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Printbutton.Location = new System.Drawing.Point(686, 1);
            this.Printbutton.Name = "Printbutton";
            this.Printbutton.Size = new System.Drawing.Size(20, 20);
            this.Printbutton.TabIndex = 1;
            this.Printbutton.UseVisualStyleBackColor = true;
            // 
            // Filterbutton
            // 
            this.Filterbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Filterbutton.BackgroundImage = global::Beep.Winform.Vis.Properties.Resources._052_filter;
            this.Filterbutton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Filterbutton.Location = new System.Drawing.Point(710, 1);
            this.Filterbutton.Name = "Filterbutton";
            this.Filterbutton.Size = new System.Drawing.Size(20, 20);
            this.Filterbutton.TabIndex = 0;
            this.Filterbutton.UseVisualStyleBackColor = true;
            // 
            // Bottompanel
            // 
            this.Bottompanel.BackColor = System.Drawing.Color.White;
            this.Bottompanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Bottompanel.Controls.Add(this.uc_bindingNavigator1);
            this.Bottompanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Bottompanel.Location = new System.Drawing.Point(0, 324);
            this.Bottompanel.Name = "Bottompanel";
            this.Bottompanel.Size = new System.Drawing.Size(735, 24);
            this.Bottompanel.TabIndex = 1;
            // 
            // uc_bindingNavigator1
            // 
            this.uc_bindingNavigator1.AddinName = null;
            this.uc_bindingNavigator1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uc_bindingNavigator1.BackColor = System.Drawing.Color.White;
            this.uc_bindingNavigator1.bindingSource = null;
            this.uc_bindingNavigator1.ButtonBorderSize = 0;
            this.uc_bindingNavigator1.CausesValidation = false;
            this.uc_bindingNavigator1.ControlHeight = 30;
            this.uc_bindingNavigator1.DefaultCreate = false;
            this.uc_bindingNavigator1.Description = null;
            this.uc_bindingNavigator1.DllName = null;
            this.uc_bindingNavigator1.DllPath = null;
            this.uc_bindingNavigator1.DMEEditor = null;
            this.uc_bindingNavigator1.EntityName = null;
            this.uc_bindingNavigator1.EntityStructure = null;
            this.uc_bindingNavigator1.ErrorObject = null;
            this.uc_bindingNavigator1.HightlightColor = System.Drawing.Color.Empty;
            this.uc_bindingNavigator1.Location = new System.Drawing.Point(0, 3);
            this.uc_bindingNavigator1.Logger = null;
            this.uc_bindingNavigator1.Name = "uc_bindingNavigator1";
            this.uc_bindingNavigator1.NameSpace = null;
            this.uc_bindingNavigator1.ObjectName = null;
            this.uc_bindingNavigator1.ObjectType = null;
            this.uc_bindingNavigator1.ParentName = null;
            this.uc_bindingNavigator1.Passedarg = null;
            this.uc_bindingNavigator1.SelectedColor = System.Drawing.Color.Empty;
            this.uc_bindingNavigator1.Size = new System.Drawing.Size(729, 16);
            this.uc_bindingNavigator1.TabIndex = 9;
            this.uc_bindingNavigator1.VerifyDelete = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.White;
            this.dataGridView1.Location = new System.Drawing.Point(0, 46);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.CornflowerBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(735, 278);
            this.dataGridView1.TabIndex = 2;
            // 
            // filterPanel
            // 
            this.filterPanel.BackColor = System.Drawing.Color.White;
            this.filterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filterPanel.Controls.Add(this.pictureBox1);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(0, 22);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(735, 24);
            this.filterPanel.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Beep.Winform.Vis.Properties.Resources._052_filter;
            this.pictureBox1.Location = new System.Drawing.Point(6, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // uc_BeepGrid
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.Bottompanel);
            this.Controls.Add(this.Toppanel);
            this.Name = "uc_BeepGrid";
            this.Size = new System.Drawing.Size(735, 348);
            this.Toppanel.ResumeLayout(false);
            this.Bottompanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.filterPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel Toppanel;
        public System.Windows.Forms.Panel Bottompanel;
       
        public System.Windows.Forms.Button Filterbutton;
        public System.Windows.Forms.Button Printbutton;
        public System.Windows.Forms.Button Sharebutton;
        public System.Windows.Forms.Label MessageLabel;
        public System.Windows.Forms.Label Titlelabel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Panel filterPanel;
        private uc_bindingNavigator uc_bindingNavigator1;
        private PictureBox pictureBox1;
    }
}
