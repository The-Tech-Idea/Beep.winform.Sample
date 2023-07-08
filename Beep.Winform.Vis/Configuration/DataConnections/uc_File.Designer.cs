namespace Beep.Config.Winform.DataConnections
{
    partial class uc_File
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
            ReaLTaiizor.Controls.PoisonLabel driverVersionLabel;
            ReaLTaiizor.Controls.PoisonLabel connectionNameLabel;
            ReaLTaiizor.Controls.PoisonLabel driverNameLabel;
            ReaLTaiizor.Controls.PoisonLabel passwordLabel;
            ReaLTaiizor.Controls.PoisonLabel userIDLabel;
            ReaLTaiizor.Controls.PoisonLabel poisonLabel2;
            ReaLTaiizor.Controls.PoisonLabel poisonLabel3;
            this.poisonStyleManager1 = new ReaLTaiizor.Manager.PoisonStyleManager(this.components);
            this.dataConnectionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.poisonPanel3 = new ReaLTaiizor.Controls.PoisonPanel();
            this.ExitCancelpoisonButton = new ReaLTaiizor.Controls.PoisonButton();
            this.SaveButton = new ReaLTaiizor.Controls.PoisonButton();
            this.poisonPanel1 = new ReaLTaiizor.Controls.PoisonPanel();
            this.poisonPanel2 = new ReaLTaiizor.Controls.PoisonPanel();
            this.poisonLabel1 = new ReaLTaiizor.Controls.PoisonLabel();
            this.connectionNameTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.driverVersionComboBox = new ReaLTaiizor.Controls.PoisonComboBox();
            this.driverNameComboBox = new ReaLTaiizor.Controls.PoisonComboBox();
            this.userIDTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.passwordTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.FileNamepoisonTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.FilePathpoisonTextBox1 = new ReaLTaiizor.Controls.PoisonTextBox();
            driverVersionLabel = new ReaLTaiizor.Controls.PoisonLabel();
            connectionNameLabel = new ReaLTaiizor.Controls.PoisonLabel();
            driverNameLabel = new ReaLTaiizor.Controls.PoisonLabel();
            passwordLabel = new ReaLTaiizor.Controls.PoisonLabel();
            userIDLabel = new ReaLTaiizor.Controls.PoisonLabel();
            poisonLabel2 = new ReaLTaiizor.Controls.PoisonLabel();
            poisonLabel3 = new ReaLTaiizor.Controls.PoisonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.poisonStyleManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).BeginInit();
            this.poisonPanel3.SuspendLayout();
            this.poisonPanel1.SuspendLayout();
            this.poisonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // poisonStyleManager1
            // 
            this.poisonStyleManager1.Owner = null;
            // 
            // dataConnectionsBindingSource
            // 
            this.dataConnectionsBindingSource.DataSource = typeof(TheTechIdea.Util.ConnectionProperties);
            // 
            // poisonPanel3
            // 
            this.poisonPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.poisonPanel3.Controls.Add(this.ExitCancelpoisonButton);
            this.poisonPanel3.Controls.Add(this.SaveButton);
            this.poisonPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.poisonPanel3.HorizontalScrollbarBarColor = true;
            this.poisonPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.poisonPanel3.HorizontalScrollbarSize = 10;
            this.poisonPanel3.Location = new System.Drawing.Point(0, 290);
            this.poisonPanel3.Name = "poisonPanel3";
            this.poisonPanel3.Size = new System.Drawing.Size(677, 55);
            this.poisonPanel3.TabIndex = 65;
            this.poisonPanel3.UseStyleColors = true;
            this.poisonPanel3.VerticalScrollbarBarColor = true;
            this.poisonPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.poisonPanel3.VerticalScrollbarSize = 10;
            // 
            // ExitCancelpoisonButton
            // 
            this.ExitCancelpoisonButton.Location = new System.Drawing.Point(18, 20);
            this.ExitCancelpoisonButton.Name = "ExitCancelpoisonButton";
            this.ExitCancelpoisonButton.Size = new System.Drawing.Size(75, 23);
            this.ExitCancelpoisonButton.TabIndex = 3;
            this.ExitCancelpoisonButton.Text = "Exit/Cancel";
            this.ExitCancelpoisonButton.UseSelectable = true;
            this.ExitCancelpoisonButton.UseStyleColors = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(580, 20);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseSelectable = true;
            this.SaveButton.UseStyleColors = true;
            // 
            // poisonPanel1
            // 
            this.poisonPanel1.Controls.Add(poisonLabel3);
            this.poisonPanel1.Controls.Add(poisonLabel2);
            this.poisonPanel1.Controls.Add(this.FilePathpoisonTextBox1);
            this.poisonPanel1.Controls.Add(this.FileNamepoisonTextBox);
            this.poisonPanel1.Controls.Add(this.poisonPanel2);
            this.poisonPanel1.Controls.Add(this.poisonPanel3);
            this.poisonPanel1.Controls.Add(driverVersionLabel);
            this.poisonPanel1.Controls.Add(this.connectionNameTextBox);
            this.poisonPanel1.Controls.Add(this.driverVersionComboBox);
            this.poisonPanel1.Controls.Add(connectionNameLabel);
            this.poisonPanel1.Controls.Add(driverNameLabel);
            this.poisonPanel1.Controls.Add(this.driverNameComboBox);
            this.poisonPanel1.Controls.Add(passwordLabel);
            this.poisonPanel1.Controls.Add(this.userIDTextBox);
            this.poisonPanel1.Controls.Add(this.passwordTextBox);
            this.poisonPanel1.Controls.Add(userIDLabel);
            this.poisonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poisonPanel1.HorizontalScrollbarBarColor = true;
            this.poisonPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.poisonPanel1.HorizontalScrollbarSize = 10;
            this.poisonPanel1.Location = new System.Drawing.Point(0, 0);
            this.poisonPanel1.Name = "poisonPanel1";
            this.poisonPanel1.Size = new System.Drawing.Size(677, 345);
            this.poisonPanel1.TabIndex = 68;
            this.poisonPanel1.VerticalScrollbarBarColor = true;
            this.poisonPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.poisonPanel1.VerticalScrollbarSize = 10;
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
            this.poisonPanel2.Size = new System.Drawing.Size(677, 58);
            this.poisonPanel2.TabIndex = 4;
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
            this.poisonLabel1.Location = new System.Drawing.Point(207, 13);
            this.poisonLabel1.Name = "poisonLabel1";
            this.poisonLabel1.Size = new System.Drawing.Size(143, 25);
            this.poisonLabel1.TabIndex = 3;
            this.poisonLabel1.Text = "File Connection";
            this.poisonLabel1.UseStyleColors = true;
            // 
            // driverVersionLabel
            // 
            driverVersionLabel.AutoSize = true;
            driverVersionLabel.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            driverVersionLabel.Location = new System.Drawing.Point(103, 238);
            driverVersionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            driverVersionLabel.Name = "driverVersionLabel";
            driverVersionLabel.Size = new System.Drawing.Size(98, 19);
            driverVersionLabel.TabIndex = 64;
            driverVersionLabel.Text = "Driver Version:";
            driverVersionLabel.UseStyleColors = true;
            // 
            // connectionNameTextBox
            // 
            // 
            // 
            // 
            this.connectionNameTextBox.CustomButton.Image = null;
            this.connectionNameTextBox.CustomButton.Location = new System.Drawing.Point(361, 1);
            this.connectionNameTextBox.CustomButton.Name = "";
            this.connectionNameTextBox.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.connectionNameTextBox.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.connectionNameTextBox.CustomButton.TabIndex = 1;
            this.connectionNameTextBox.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.connectionNameTextBox.CustomButton.UseSelectable = true;
            this.connectionNameTextBox.CustomButton.Visible = false;
            this.connectionNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "ConnectionName", true));
            this.connectionNameTextBox.FontSize = ReaLTaiizor.Extension.Poison.PoisonTextBoxSize.Medium;
            this.connectionNameTextBox.FontWeight = ReaLTaiizor.Extension.Poison.PoisonTextBoxWeight.Bold;
          //  this.connectionNameTextBox.Icon = global::Beep.Config.Winform.Properties.Resources._221_database_6;
            this.connectionNameTextBox.IconRight = true;
            this.connectionNameTextBox.Lines = new string[0];
            this.connectionNameTextBox.Location = new System.Drawing.Point(209, 72);
            this.connectionNameTextBox.MaxLength = 32767;
            this.connectionNameTextBox.Name = "connectionNameTextBox";
            this.connectionNameTextBox.PasswordChar = '\0';
            this.connectionNameTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.connectionNameTextBox.SelectedText = "";
            this.connectionNameTextBox.SelectionLength = 0;
            this.connectionNameTextBox.SelectionStart = 0;
            this.connectionNameTextBox.ShortcutsEnabled = true;
            this.connectionNameTextBox.Size = new System.Drawing.Size(385, 25);
            this.connectionNameTextBox.TabIndex = 40;
            this.connectionNameTextBox.UseSelectable = true;
            this.connectionNameTextBox.UseStyleColors = true;
            this.connectionNameTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.connectionNameTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // driverVersionComboBox
            // 
            this.driverVersionComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "DriverVersion", true));
            this.driverVersionComboBox.FormattingEnabled = true;
            this.driverVersionComboBox.ItemHeight = 23;
            this.driverVersionComboBox.Location = new System.Drawing.Point(209, 238);
            this.driverVersionComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.driverVersionComboBox.Name = "driverVersionComboBox";
            this.driverVersionComboBox.Size = new System.Drawing.Size(82, 29);
            this.driverVersionComboBox.TabIndex = 62;
            this.driverVersionComboBox.UseSelectable = true;
            this.driverVersionComboBox.UseStyleColors = true;
            // 
            // connectionNameLabel
            // 
            connectionNameLabel.AutoSize = true;
            connectionNameLabel.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            connectionNameLabel.Location = new System.Drawing.Point(79, 72);
            connectionNameLabel.Name = "connectionNameLabel";
            connectionNameLabel.Size = new System.Drawing.Size(122, 19);
            connectionNameLabel.TabIndex = 41;
            connectionNameLabel.Text = "Connection Name:";
            connectionNameLabel.UseStyleColors = true;
            // 
            // driverNameLabel
            // 
            driverNameLabel.AutoSize = true;
            driverNameLabel.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            driverNameLabel.Location = new System.Drawing.Point(112, 205);
            driverNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            driverNameLabel.Name = "driverNameLabel";
            driverNameLabel.Size = new System.Drawing.Size(89, 19);
            driverNameLabel.TabIndex = 63;
            driverNameLabel.Text = "Driver Name:";
            driverNameLabel.UseStyleColors = true;
            // 
            // driverNameComboBox
            // 
            this.driverNameComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "DriverName", true));
            this.driverNameComboBox.FormattingEnabled = true;
            this.driverNameComboBox.ItemHeight = 23;
            this.driverNameComboBox.Location = new System.Drawing.Point(209, 203);
            this.driverNameComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.driverNameComboBox.Name = "driverNameComboBox";
            this.driverNameComboBox.Size = new System.Drawing.Size(218, 29);
            this.driverNameComboBox.TabIndex = 61;
            this.driverNameComboBox.UseSelectable = true;
            this.driverNameComboBox.UseStyleColors = true;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            passwordLabel.Location = new System.Drawing.Point(131, 127);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new System.Drawing.Size(70, 19);
            passwordLabel.TabIndex = 56;
            passwordLabel.Text = "Password:";
            passwordLabel.UseStyleColors = true;
            // 
            // userIDTextBox
            // 
            // 
            // 
            // 
            this.userIDTextBox.CustomButton.Image = null;
            this.userIDTextBox.CustomButton.Location = new System.Drawing.Point(103, 2);
            this.userIDTextBox.CustomButton.Name = "";
            this.userIDTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.userIDTextBox.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.userIDTextBox.CustomButton.TabIndex = 1;
            this.userIDTextBox.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.userIDTextBox.CustomButton.UseSelectable = true;
            this.userIDTextBox.CustomButton.Visible = false;
            this.userIDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "UserID", true));
            this.userIDTextBox.Lines = new string[0];
            this.userIDTextBox.Location = new System.Drawing.Point(209, 103);
            this.userIDTextBox.MaxLength = 32767;
            this.userIDTextBox.Name = "userIDTextBox";
            this.userIDTextBox.PasswordChar = '\0';
            this.userIDTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.userIDTextBox.SelectedText = "";
            this.userIDTextBox.SelectionLength = 0;
            this.userIDTextBox.SelectionStart = 0;
            this.userIDTextBox.ShortcutsEnabled = true;
            this.userIDTextBox.Size = new System.Drawing.Size(121, 20);
            this.userIDTextBox.TabIndex = 51;
            this.userIDTextBox.UseSelectable = true;
            this.userIDTextBox.UseStyleColors = true;
            this.userIDTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.userIDTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // passwordTextBox
            // 
            // 
            // 
            // 
            this.passwordTextBox.CustomButton.Image = null;
            this.passwordTextBox.CustomButton.Location = new System.Drawing.Point(103, 2);
            this.passwordTextBox.CustomButton.Name = "";
            this.passwordTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.passwordTextBox.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.passwordTextBox.CustomButton.TabIndex = 1;
            this.passwordTextBox.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.passwordTextBox.CustomButton.UseSelectable = true;
            this.passwordTextBox.CustomButton.Visible = false;
            this.passwordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "Password", true));
            this.passwordTextBox.Lines = new string[0];
            this.passwordTextBox.Location = new System.Drawing.Point(209, 126);
            this.passwordTextBox.MaxLength = 32767;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.passwordTextBox.SelectedText = "";
            this.passwordTextBox.SelectionLength = 0;
            this.passwordTextBox.SelectionStart = 0;
            this.passwordTextBox.ShortcutsEnabled = true;
            this.passwordTextBox.Size = new System.Drawing.Size(121, 20);
            this.passwordTextBox.TabIndex = 52;
            this.passwordTextBox.UseSelectable = true;
            this.passwordTextBox.UseStyleColors = true;
            this.passwordTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.passwordTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // userIDLabel
            // 
            userIDLabel.AutoSize = true;
            userIDLabel.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            userIDLabel.Location = new System.Drawing.Point(143, 104);
            userIDLabel.Name = "userIDLabel";
            userIDLabel.Size = new System.Drawing.Size(58, 19);
            userIDLabel.TabIndex = 55;
            userIDLabel.Text = "User ID:";
            userIDLabel.UseStyleColors = true;
            // 
            // FileNamepoisonTextBox
            // 
            // 
            // 
            // 
            this.FileNamepoisonTextBox.CustomButton.Image = null;
            this.FileNamepoisonTextBox.CustomButton.Location = new System.Drawing.Point(366, 2);
            this.FileNamepoisonTextBox.CustomButton.Name = "";
            this.FileNamepoisonTextBox.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.FileNamepoisonTextBox.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.FileNamepoisonTextBox.CustomButton.TabIndex = 1;
            this.FileNamepoisonTextBox.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.FileNamepoisonTextBox.CustomButton.UseSelectable = true;
            this.FileNamepoisonTextBox.CustomButton.Visible = false;
            this.FileNamepoisonTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "FileName", true));
            this.FileNamepoisonTextBox.Lines = new string[0];
            this.FileNamepoisonTextBox.Location = new System.Drawing.Point(209, 152);
            this.FileNamepoisonTextBox.MaxLength = 32767;
            this.FileNamepoisonTextBox.Name = "FileNamepoisonTextBox";
            this.FileNamepoisonTextBox.PasswordChar = '*';
            this.FileNamepoisonTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.FileNamepoisonTextBox.SelectedText = "";
            this.FileNamepoisonTextBox.SelectionLength = 0;
            this.FileNamepoisonTextBox.SelectionStart = 0;
            this.FileNamepoisonTextBox.ShortcutsEnabled = true;
            this.FileNamepoisonTextBox.Size = new System.Drawing.Size(384, 20);
            this.FileNamepoisonTextBox.TabIndex = 66;
            this.FileNamepoisonTextBox.UseSelectable = true;
            this.FileNamepoisonTextBox.UseStyleColors = true;
            this.FileNamepoisonTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.FileNamepoisonTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // FilePathpoisonTextBox1
            // 
            // 
            // 
            // 
            this.FilePathpoisonTextBox1.CustomButton.Image = null;
            this.FilePathpoisonTextBox1.CustomButton.Location = new System.Drawing.Point(366, 2);
            this.FilePathpoisonTextBox1.CustomButton.Name = "";
            this.FilePathpoisonTextBox1.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.FilePathpoisonTextBox1.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.FilePathpoisonTextBox1.CustomButton.TabIndex = 1;
            this.FilePathpoisonTextBox1.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.FilePathpoisonTextBox1.CustomButton.UseSelectable = true;
            this.FilePathpoisonTextBox1.CustomButton.Visible = false;
            this.FilePathpoisonTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "FilePath", true));
            this.FilePathpoisonTextBox1.Lines = new string[0];
            this.FilePathpoisonTextBox1.Location = new System.Drawing.Point(209, 178);
            this.FilePathpoisonTextBox1.MaxLength = 32767;
            this.FilePathpoisonTextBox1.Name = "FilePathpoisonTextBox1";
            this.FilePathpoisonTextBox1.PasswordChar = '*';
            this.FilePathpoisonTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.FilePathpoisonTextBox1.SelectedText = "";
            this.FilePathpoisonTextBox1.SelectionLength = 0;
            this.FilePathpoisonTextBox1.SelectionStart = 0;
            this.FilePathpoisonTextBox1.ShortcutsEnabled = true;
            this.FilePathpoisonTextBox1.Size = new System.Drawing.Size(384, 20);
            this.FilePathpoisonTextBox1.TabIndex = 67;
            this.FilePathpoisonTextBox1.UseSelectable = true;
            this.FilePathpoisonTextBox1.UseStyleColors = true;
            this.FilePathpoisonTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.FilePathpoisonTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // poisonLabel2
            // 
            poisonLabel2.AutoSize = true;
            poisonLabel2.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            poisonLabel2.Location = new System.Drawing.Point(133, 152);
            poisonLabel2.Name = "poisonLabel2";
            poisonLabel2.Size = new System.Drawing.Size(68, 19);
            poisonLabel2.TabIndex = 68;
            poisonLabel2.Text = "FileName:";
            poisonLabel2.UseStyleColors = true;
            // 
            // poisonLabel3
            // 
            poisonLabel3.AutoSize = true;
            poisonLabel3.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            poisonLabel3.Location = new System.Drawing.Point(141, 179);
            poisonLabel3.Name = "poisonLabel3";
            poisonLabel3.Size = new System.Drawing.Size(60, 19);
            poisonLabel3.TabIndex = 69;
            poisonLabel3.Text = "FilePath:";
            poisonLabel3.UseStyleColors = true;
            // 
            // uc_File
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.poisonPanel1);
            this.Name = "uc_File";
            this.Size = new System.Drawing.Size(677, 345);
            ((System.ComponentModel.ISupportInitialize)(this.poisonStyleManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).EndInit();
            this.poisonPanel3.ResumeLayout(false);
            this.poisonPanel1.ResumeLayout(false);
            this.poisonPanel1.PerformLayout();
            this.poisonPanel2.ResumeLayout(false);
            this.poisonPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Manager.PoisonStyleManager poisonStyleManager1;
        private BindingSource dataConnectionsBindingSource;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel3;
        private ReaLTaiizor.Controls.PoisonButton ExitCancelpoisonButton;
        private ReaLTaiizor.Controls.PoisonButton SaveButton;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel1;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel2;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1;
        private ReaLTaiizor.Controls.PoisonTextBox connectionNameTextBox;
        private ReaLTaiizor.Controls.PoisonComboBox driverVersionComboBox;
        private ReaLTaiizor.Controls.PoisonComboBox driverNameComboBox;
        private ReaLTaiizor.Controls.PoisonTextBox userIDTextBox;
        private ReaLTaiizor.Controls.PoisonTextBox passwordTextBox;
        private ReaLTaiizor.Controls.PoisonTextBox FilePathpoisonTextBox1;
        private ReaLTaiizor.Controls.PoisonTextBox FileNamepoisonTextBox;
    }
}
