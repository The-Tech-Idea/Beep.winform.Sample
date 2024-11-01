using Beep.Vis.Module;
namespace TheTechIdea.Beep.Winform.Controls.UIEditors
{
    partial class ImageSelectorImporterForm
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
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel3 = new Panel();
            LocalResoucesradioButton = new RadioButton();
            ClearLocalResourcesbutton = new Button();
            ImportLocalResourcesbutton = new Button();
            panel4 = new Panel();
            ProjectResourceImportbutton = new Button();
            ImagelistBox = new ListBox();
            ResourcesPathcomboBox = new ComboBox();
            ProjectResoucesradioButton = new RadioButton();
            panel2 = new Panel();
            SelectImagebutton = new Button();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(244, 450);
            panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(SelectImagebutton, 0, 2);
            tableLayoutPanel1.Controls.Add(panel3, 0, 0);
            tableLayoutPanel1.Controls.Add(panel4, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20.6018524F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 79.39815F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel1.Size = new Size(244, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(LocalResoucesradioButton);
            panel3.Controls.Add(ClearLocalResourcesbutton);
            panel3.Controls.Add(ImportLocalResourcesbutton);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(238, 80);
            panel3.TabIndex = 0;
            // 
            // LocalResoucesradioButton
            // 
            LocalResoucesradioButton.AutoSize = true;
            LocalResoucesradioButton.Location = new Point(18, 16);
            LocalResoucesradioButton.Name = "LocalResoucesradioButton";
            LocalResoucesradioButton.Size = new Size(109, 19);
            LocalResoucesradioButton.TabIndex = 2;
            LocalResoucesradioButton.TabStop = true;
            LocalResoucesradioButton.Text = "Local Resources";
            LocalResoucesradioButton.UseVisualStyleBackColor = true;
            // 
            // ClearLocalResourcesbutton
            // 
            ClearLocalResourcesbutton.Location = new Point(134, 41);
            ClearLocalResourcesbutton.Name = "ClearLocalResourcesbutton";
            ClearLocalResourcesbutton.Size = new Size(75, 23);
            ClearLocalResourcesbutton.TabIndex = 1;
            ClearLocalResourcesbutton.Text = "Clear";
            ClearLocalResourcesbutton.UseVisualStyleBackColor = true;
            // 
            // ImportLocalResourcesbutton
            // 
            ImportLocalResourcesbutton.Location = new Point(18, 41);
            ImportLocalResourcesbutton.Name = "ImportLocalResourcesbutton";
            ImportLocalResourcesbutton.Size = new Size(75, 23);
            ImportLocalResourcesbutton.TabIndex = 0;
            ImportLocalResourcesbutton.Text = "Import";
            ImportLocalResourcesbutton.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.Controls.Add(ProjectResourceImportbutton);
            panel4.Controls.Add(ImagelistBox);
            panel4.Controls.Add(ResourcesPathcomboBox);
            panel4.Controls.Add(ProjectResoucesradioButton);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 89);
            panel4.Name = "panel4";
            panel4.Size = new Size(238, 328);
            panel4.TabIndex = 1;
            // 
            // ProjectResourceImportbutton
            // 
            ProjectResourceImportbutton.Location = new Point(75, 277);
            ProjectResourceImportbutton.Name = "ProjectResourceImportbutton";
            ProjectResourceImportbutton.Size = new Size(75, 23);
            ProjectResourceImportbutton.TabIndex = 3;
            ProjectResourceImportbutton.Text = "Import";
            ProjectResourceImportbutton.UseVisualStyleBackColor = true;
            // 
            // ImagelistBox
            // 
            ImagelistBox.FormattingEnabled = true;
            ImagelistBox.ItemHeight = 15;
            ImagelistBox.Location = new Point(18, 57);
            ImagelistBox.Name = "ImagelistBox";
            ImagelistBox.Size = new Size(191, 214);
            ImagelistBox.TabIndex = 5;
            // 
            // ResourcesPathcomboBox
            // 
            ResourcesPathcomboBox.FormattingEnabled = true;
            ResourcesPathcomboBox.Location = new Point(18, 28);
            ResourcesPathcomboBox.Name = "ResourcesPathcomboBox";
            ResourcesPathcomboBox.Size = new Size(191, 23);
            ResourcesPathcomboBox.TabIndex = 4;
            // 
            // ProjectResoucesradioButton
            // 
            ProjectResoucesradioButton.AutoSize = true;
            ProjectResoucesradioButton.Location = new Point(18, 3);
            ProjectResoucesradioButton.Name = "ProjectResoucesradioButton";
            ProjectResoucesradioButton.Size = new Size(118, 19);
            ProjectResoucesradioButton.TabIndex = 3;
            ProjectResoucesradioButton.TabStop = true;
            ProjectResoucesradioButton.Text = "Project Resources";
            ProjectResoucesradioButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            //panel2.Controls.Add(ImagePreview_beepImage);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(244, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(556, 450);
            panel2.TabIndex = 1;
            //// 
            //// ImagePreview_beepImage
            //// 
            //ImagePreview_beepImage.ApplyThemeOnImage = false;
            //ImagePreview_beepImage.BackColor = Color.FromArgb(255, 255, 255);
            //ImagePreview_beepImage.Dock = DockStyle.Fill;
            //ImagePreview_beepImage.ForeColor = Color.FromArgb(255, 255, 255);
            //ImagePreview_beepImage.Image = null;
            //ImagePreview_beepImage.ImagePath = null;
            //ImagePreview_beepImage.IsStillImage = true;
            //ImagePreview_beepImage.Location = new Point(0, 0);
            //ImagePreview_beepImage.Name = "ImagePreview_beepImage";
            //ImagePreview_beepImage.ScaleMode = ImageScaleMode.KeepAspectRatio;
            //ImagePreview_beepImage.Size = new Size(556, 450);
            //ImagePreview_beepImage.TabIndex = 0;
            //ImagePreview_beepImage.Text = "beepImage1";
            // 
            // SelectImagebutton
            // 
            SelectImagebutton.Anchor = AnchorStyles.None;
            SelectImagebutton.Location = new Point(84, 423);
            SelectImagebutton.Name = "SelectImagebutton";
            SelectImagebutton.Size = new Size(75, 23);
            SelectImagebutton.TabIndex = 6;
            SelectImagebutton.Text = "Select";
            SelectImagebutton.UseVisualStyleBackColor = true;
            // 
            // ImageDesignerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ImageDesignerForm";
            Text = "Beep Image Loader";
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        //private BeepImage ImagePreview_beepImage;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel3;
        private Button ClearLocalResourcesbutton;
        private Button ImportLocalResourcesbutton;
        private Panel panel4;
        private RadioButton LocalResoucesradioButton;
        private Button ProjectResourceImportbutton;
        private ListBox ImagelistBox;
        private ComboBox ResourcesPathcomboBox;
        private RadioButton ProjectResoucesradioButton;
        private Button SelectImagebutton;
    }
}