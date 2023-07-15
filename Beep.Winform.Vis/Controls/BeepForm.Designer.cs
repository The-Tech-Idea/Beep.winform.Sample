namespace Beep.Winform.Controls
{
    partial class BeepForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Insidepanel = new ReaLTaiizor.Controls.MetroPanel();
            this.SuspendLayout();
            // 
            // Insidepanel
            // 
            this.Insidepanel.BackgroundColor = System.Drawing.Color.White;
            this.Insidepanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.Insidepanel.BorderThickness = 1;
            this.Insidepanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Insidepanel.IsDerivedStyle = true;
            this.Insidepanel.Location = new System.Drawing.Point(0, 0);
            this.Insidepanel.Name = "Insidepanel";
            this.Insidepanel.Size = new System.Drawing.Size(872, 608);
            this.Insidepanel.Style = ReaLTaiizor.Enum.Metro.Style.Light;
            this.Insidepanel.StyleManager = null;
            this.Insidepanel.TabIndex = 2;
            this.Insidepanel.ThemeAuthor = "Taiizor";
            this.Insidepanel.ThemeName = "MetroLight";
            // 
            // BeepForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 608);
            this.Controls.Add(this.Insidepanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(3440, 1392);
            this.MinimumSize = new System.Drawing.Size(190, 40);
            this.Name = "BeepForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "dungeonForm1";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MetroPanel Insidepanel;
    }
}