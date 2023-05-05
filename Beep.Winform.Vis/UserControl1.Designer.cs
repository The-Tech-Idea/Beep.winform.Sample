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
            this.beepTabControl1 = new BeepEnterprize.Winform.Vis.Controls.BeepTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.beepTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // beepTabControl1
            // 
            this.beepTabControl1.Controls.Add(this.tabPage1);
            this.beepTabControl1.Controls.Add(this.tabPage2);
            this.beepTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.beepTabControl1.Location = new System.Drawing.Point(59, 176);
            this.beepTabControl1.Name = "beepTabControl1";
            this.beepTabControl1.Padding = new System.Drawing.Point(14, 4);
            this.beepTabControl1.SelectedIndex = 0;
            this.beepTabControl1.Size = new System.Drawing.Size(200, 100);
            this.beepTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 72);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 72);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.beepTabControl1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(719, 601);
            this.beepTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BeepEnterprize.Winform.Vis.Controls.BeepTabControl beepTabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
    }
}
