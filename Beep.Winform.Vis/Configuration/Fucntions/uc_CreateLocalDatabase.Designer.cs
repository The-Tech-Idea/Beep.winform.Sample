
namespace Beep.Config.Winform.Functions
{
    partial class uc_CreateLocalDatabase
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
            ReaLTaiizor.Controls.PoisonLabel passwordLabel;
            this.InstallFoldercomboBox = new ReaLTaiizor.Controls.PoisonComboBox();
            this.EmbeddedDatabaseTypecomboBox = new ReaLTaiizor.Controls.PoisonComboBox();
            this.label1 = new ReaLTaiizor.Controls.PoisonLabel();
            this.label2 = new ReaLTaiizor.Controls.PoisonLabel();
            this.label3 = new ReaLTaiizor.Controls.PoisonLabel();
            this.CreateDBbutton = new ReaLTaiizor.Controls.PoisonButton();
            this.dataConnectionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.passwordTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.databaseTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.poisonPanel1 = new ReaLTaiizor.Controls.PoisonPanel();
            this.panel2 = new ReaLTaiizor.Controls.PoisonPanel();
            this.poisonLabel1 = new ReaLTaiizor.Controls.PoisonLabel();
            this.poisonStyleManager1 = new ReaLTaiizor.Manager.PoisonStyleManager(this.components);
            passwordLabel = new ReaLTaiizor.Controls.PoisonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).BeginInit();
            this.poisonPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.poisonStyleManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new System.Drawing.Point(98, 178);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(66, 19);
            passwordLabel.TabIndex = 32;
            passwordLabel.Text = "Password:";
            // 
            // InstallFoldercomboBox
            // 
            this.InstallFoldercomboBox.FormattingEnabled = true;
            this.InstallFoldercomboBox.ItemHeight = 23;
            this.InstallFoldercomboBox.Location = new System.Drawing.Point(170, 82);
            this.InstallFoldercomboBox.Name = "InstallFoldercomboBox";
            this.InstallFoldercomboBox.Size = new System.Drawing.Size(286, 29);
            this.InstallFoldercomboBox.TabIndex = 1;
            this.InstallFoldercomboBox.UseSelectable = true;
            // 
            // EmbeddedDatabaseTypecomboBox
            // 
            this.EmbeddedDatabaseTypecomboBox.FormattingEnabled = true;
            this.EmbeddedDatabaseTypecomboBox.ItemHeight = 23;
            this.EmbeddedDatabaseTypecomboBox.Location = new System.Drawing.Point(170, 117);
            this.EmbeddedDatabaseTypecomboBox.Name = "EmbeddedDatabaseTypecomboBox";
            this.EmbeddedDatabaseTypecomboBox.Size = new System.Drawing.Size(286, 29);
            this.EmbeddedDatabaseTypecomboBox.TabIndex = 2;
            this.EmbeddedDatabaseTypecomboBox.UseSelectable = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Installation Folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Class  Database Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "Database Name";
            // 
            // CreateDBbutton
            // 
            this.CreateDBbutton.Location = new System.Drawing.Point(247, 224);
            this.CreateDBbutton.Name = "CreateDBbutton";
            this.CreateDBbutton.Size = new System.Drawing.Size(121, 23);
            this.CreateDBbutton.TabIndex = 4;
            this.CreateDBbutton.Text = "Create";
            this.CreateDBbutton.UseSelectable = true;
            // 
            // dataConnectionsBindingSource
            // 
            this.dataConnectionsBindingSource.DataSource = typeof(TheTechIdea.Util.ConnectionProperties);
            // 
            // passwordTextBox
            // 
            // 
            // 
            // 
            this.passwordTextBox.CustomButton.Image = null;
            this.passwordTextBox.CustomButton.Location = new System.Drawing.Point(268, 2);
            this.passwordTextBox.CustomButton.Name = "";
            this.passwordTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.passwordTextBox.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.passwordTextBox.CustomButton.TabIndex = 1;
            this.passwordTextBox.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.passwordTextBox.CustomButton.UseSelectable = true;
            this.passwordTextBox.CustomButton.Visible = false;
            this.passwordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "Password", true));
            this.passwordTextBox.Lines = new string[0];
            this.passwordTextBox.Location = new System.Drawing.Point(170, 178);
            this.passwordTextBox.MaxLength = 32767;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '\0';
            this.passwordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.passwordTextBox.SelectedText = "";
            this.passwordTextBox.SelectionLength = 0;
            this.passwordTextBox.SelectionStart = 0;
            this.passwordTextBox.ShortcutsEnabled = true;
            this.passwordTextBox.Size = new System.Drawing.Size(286, 20);
            this.passwordTextBox.TabIndex = 3;
            this.passwordTextBox.UseSelectable = true;
            this.passwordTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.passwordTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // databaseTextBox
            // 
            // 
            // 
            // 
            this.databaseTextBox.CustomButton.Image = null;
            this.databaseTextBox.CustomButton.Location = new System.Drawing.Point(268, 2);
            this.databaseTextBox.CustomButton.Name = "";
            this.databaseTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.databaseTextBox.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.databaseTextBox.CustomButton.TabIndex = 1;
            this.databaseTextBox.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.databaseTextBox.CustomButton.UseSelectable = true;
            this.databaseTextBox.CustomButton.Visible = false;
            this.databaseTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "Database", true));
            this.databaseTextBox.Lines = new string[0];
            this.databaseTextBox.Location = new System.Drawing.Point(170, 50);
            this.databaseTextBox.MaxLength = 32767;
            this.databaseTextBox.Name = "databaseTextBox";
            this.databaseTextBox.PasswordChar = '\0';
            this.databaseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.databaseTextBox.SelectedText = "";
            this.databaseTextBox.SelectionLength = 0;
            this.databaseTextBox.SelectionStart = 0;
            this.databaseTextBox.ShortcutsEnabled = true;
            this.databaseTextBox.Size = new System.Drawing.Size(286, 20);
            this.databaseTextBox.TabIndex = 0;
            this.databaseTextBox.UseSelectable = true;
            this.databaseTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.databaseTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // poisonPanel1
            // 
            this.poisonPanel1.Controls.Add(this.panel2);
            this.poisonPanel1.Controls.Add(this.passwordTextBox);
            this.poisonPanel1.Controls.Add(passwordLabel);
            this.poisonPanel1.Controls.Add(this.InstallFoldercomboBox);
            this.poisonPanel1.Controls.Add(this.databaseTextBox);
            this.poisonPanel1.Controls.Add(this.EmbeddedDatabaseTypecomboBox);
            this.poisonPanel1.Controls.Add(this.CreateDBbutton);
            this.poisonPanel1.Controls.Add(this.label1);
            this.poisonPanel1.Controls.Add(this.label3);
            this.poisonPanel1.Controls.Add(this.label2);
            this.poisonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poisonPanel1.HorizontalScrollbarBarColor = true;
            this.poisonPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.poisonPanel1.HorizontalScrollbarSize = 10;
            this.poisonPanel1.Location = new System.Drawing.Point(0, 0);
            this.poisonPanel1.Name = "poisonPanel1";
            this.poisonPanel1.Size = new System.Drawing.Size(581, 274);
            this.poisonPanel1.TabIndex = 33;
            this.poisonPanel1.VerticalScrollbarBarColor = true;
            this.poisonPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.poisonPanel1.VerticalScrollbarSize = 10;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.poisonLabel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.HorizontalScrollbarBarColor = true;
            this.panel2.HorizontalScrollbarHighlightOnWheel = false;
            this.panel2.HorizontalScrollbarSize = 10;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(581, 44);
            this.panel2.TabIndex = 33;
            this.panel2.VerticalScrollbarBarColor = true;
            this.panel2.VerticalScrollbarHighlightOnWheel = false;
            this.panel2.VerticalScrollbarSize = 10;
            // 
            // poisonLabel1
            // 
            this.poisonLabel1.AutoSize = true;
            this.poisonLabel1.FontSize = ReaLTaiizor.Extension.Poison.PoisonLabelSize.Tall;
            this.poisonLabel1.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            this.poisonLabel1.Location = new System.Drawing.Point(27, 6);
            this.poisonLabel1.Name = "poisonLabel1";
            this.poisonLabel1.Size = new System.Drawing.Size(205, 25);
            this.poisonLabel1.TabIndex = 4;
            this.poisonLabel1.Text = "Create Local Database ";
            this.poisonLabel1.UseStyleColors = true;
            // 
            // poisonStyleManager1
            // 
            this.poisonStyleManager1.Owner = this;
            // 
            // uc_CreateLocalDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.poisonPanel1);
            this.Name = "uc_CreateLocalDatabase";
            this.Size = new System.Drawing.Size(581, 274);
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).EndInit();
            this.poisonPanel1.ResumeLayout(false);
            this.poisonPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.poisonStyleManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.PoisonComboBox InstallFoldercomboBox;
        private ReaLTaiizor.Controls.PoisonComboBox EmbeddedDatabaseTypecomboBox;
        private ReaLTaiizor.Controls.PoisonLabel label1;
        private ReaLTaiizor.Controls.PoisonLabel label2;
        private ReaLTaiizor.Controls.PoisonLabel label3;
        private ReaLTaiizor.Controls.PoisonButton CreateDBbutton;
        private System.Windows.Forms.BindingSource dataConnectionsBindingSource;
        private ReaLTaiizor.Controls.PoisonTextBox passwordTextBox;
        private ReaLTaiizor.Controls.PoisonTextBox databaseTextBox;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel1;
        private ReaLTaiizor.Manager.PoisonStyleManager poisonStyleManager1;
        private ReaLTaiizor.Controls.PoisonPanel panel2;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1;
    }
}
