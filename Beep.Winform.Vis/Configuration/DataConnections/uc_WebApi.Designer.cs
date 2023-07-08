namespace Beep.Config.Winform.DataConnections
{
    partial class uc_WebApi
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
            ReaLTaiizor.Controls.PoisonLabel poisonLabel4;
            ReaLTaiizor.Controls.PoisonLabel poisonLabel5;
            this.connectionNameTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.dataConnectionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.driverVersionComboBox = new ReaLTaiizor.Controls.PoisonComboBox();
            this.driverNameComboBox = new ReaLTaiizor.Controls.PoisonComboBox();
            this.passwordTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.poisonLabel1 = new ReaLTaiizor.Controls.PoisonLabel();
            this.poisonPanel2 = new ReaLTaiizor.Controls.PoisonPanel();
            this.poisonPanel1 = new ReaLTaiizor.Controls.PoisonPanel();
            this.poisonPanel3 = new ReaLTaiizor.Controls.PoisonPanel();
            this.ExitCancelpoisonButton = new ReaLTaiizor.Controls.PoisonButton();
            this.SaveButton = new ReaLTaiizor.Controls.PoisonButton();
            this.userIDTextBox = new ReaLTaiizor.Controls.PoisonTextBox();
            this.poisonStyleManager1 = new ReaLTaiizor.Manager.PoisonStyleManager(this.components);
            this.UrlpoisonTextBox1 = new ReaLTaiizor.Controls.PoisonTextBox();
            this.ParameterspoisonTextBox1 = new ReaLTaiizor.Controls.PoisonTextBox();
            this.ApiKeypoisonTextBox1 = new ReaLTaiizor.Controls.PoisonTextBox();
            this.CertificatePathpoisonTextBox1 = new ReaLTaiizor.Controls.PoisonTextBox();
            driverVersionLabel = new ReaLTaiizor.Controls.PoisonLabel();
            connectionNameLabel = new ReaLTaiizor.Controls.PoisonLabel();
            driverNameLabel = new ReaLTaiizor.Controls.PoisonLabel();
            passwordLabel = new ReaLTaiizor.Controls.PoisonLabel();
            userIDLabel = new ReaLTaiizor.Controls.PoisonLabel();
            poisonLabel2 = new ReaLTaiizor.Controls.PoisonLabel();
            poisonLabel3 = new ReaLTaiizor.Controls.PoisonLabel();
            poisonLabel4 = new ReaLTaiizor.Controls.PoisonLabel();
            poisonLabel5 = new ReaLTaiizor.Controls.PoisonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).BeginInit();
            this.poisonPanel2.SuspendLayout();
            this.poisonPanel1.SuspendLayout();
            this.poisonPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.poisonStyleManager1)).BeginInit();
            this.SuspendLayout();
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
         //   this.connectionNameTextBox.Icon = global::Beep.Config.Winform.Properties.Resources._221_database_6;
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
            // dataConnectionsBindingSource
            // 
            this.dataConnectionsBindingSource.DataSource = typeof(TheTechIdea.Util.ConnectionProperties);
            // 
            // driverVersionComboBox
            // 
            this.driverVersionComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "DriverVersion", true));
            this.driverVersionComboBox.FormattingEnabled = true;
            this.driverVersionComboBox.ItemHeight = 23;
            this.driverVersionComboBox.Location = new System.Drawing.Point(209, 290);
            this.driverVersionComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.driverVersionComboBox.Name = "driverVersionComboBox";
            this.driverVersionComboBox.Size = new System.Drawing.Size(82, 29);
            this.driverVersionComboBox.TabIndex = 62;
            this.driverVersionComboBox.UseSelectable = true;
            this.driverVersionComboBox.UseStyleColors = true;
            // 
            // driverNameComboBox
            // 
            this.driverNameComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "DriverName", true));
            this.driverNameComboBox.FormattingEnabled = true;
            this.driverNameComboBox.ItemHeight = 23;
            this.driverNameComboBox.Location = new System.Drawing.Point(209, 255);
            this.driverNameComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.driverNameComboBox.Name = "driverNameComboBox";
            this.driverNameComboBox.Size = new System.Drawing.Size(218, 29);
            this.driverNameComboBox.TabIndex = 61;
            this.driverNameComboBox.UseSelectable = true;
            this.driverNameComboBox.UseStyleColors = true;
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
            // poisonLabel1
            // 
            this.poisonLabel1.AutoSize = true;
            this.poisonLabel1.FontSize = ReaLTaiizor.Extension.Poison.PoisonLabelSize.Tall;
            this.poisonLabel1.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Bold;
            this.poisonLabel1.Location = new System.Drawing.Point(207, 13);
            this.poisonLabel1.Name = "poisonLabel1";
            this.poisonLabel1.Size = new System.Drawing.Size(183, 25);
            this.poisonLabel1.TabIndex = 3;
            this.poisonLabel1.Text = "WebAPI Connection";
            this.poisonLabel1.UseStyleColors = true;
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
            this.poisonPanel2.Size = new System.Drawing.Size(664, 58);
            this.poisonPanel2.TabIndex = 4;
            this.poisonPanel2.UseStyleColors = true;
            this.poisonPanel2.VerticalScrollbarBarColor = true;
            this.poisonPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.poisonPanel2.VerticalScrollbarSize = 10;
            // 
            // poisonPanel1
            // 
            this.poisonPanel1.Controls.Add(poisonLabel5);
            this.poisonPanel1.Controls.Add(this.CertificatePathpoisonTextBox1);
            this.poisonPanel1.Controls.Add(poisonLabel4);
            this.poisonPanel1.Controls.Add(this.ApiKeypoisonTextBox1);
            this.poisonPanel1.Controls.Add(poisonLabel3);
            this.poisonPanel1.Controls.Add(this.ParameterspoisonTextBox1);
            this.poisonPanel1.Controls.Add(poisonLabel2);
            this.poisonPanel1.Controls.Add(this.UrlpoisonTextBox1);
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
            this.poisonPanel1.Size = new System.Drawing.Size(664, 399);
            this.poisonPanel1.TabIndex = 69;
            this.poisonPanel1.VerticalScrollbarBarColor = true;
            this.poisonPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.poisonPanel1.VerticalScrollbarSize = 10;
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
            this.poisonPanel3.Location = new System.Drawing.Point(0, 344);
            this.poisonPanel3.Name = "poisonPanel3";
            this.poisonPanel3.Size = new System.Drawing.Size(664, 55);
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
            this.SaveButton.Location = new System.Drawing.Point(556, 20);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseSelectable = true;
            this.SaveButton.UseStyleColors = true;
            // 
            // driverVersionLabel
            // 
            driverVersionLabel.AutoSize = true;
            driverVersionLabel.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            driverVersionLabel.Location = new System.Drawing.Point(103, 290);
            driverVersionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            driverVersionLabel.Name = "driverVersionLabel";
            driverVersionLabel.Size = new System.Drawing.Size(98, 19);
            driverVersionLabel.TabIndex = 64;
            driverVersionLabel.Text = "Driver Version:";
            driverVersionLabel.UseStyleColors = true;
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
            driverNameLabel.Location = new System.Drawing.Point(112, 257);
            driverNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            driverNameLabel.Name = "driverNameLabel";
            driverNameLabel.Size = new System.Drawing.Size(89, 19);
            driverNameLabel.TabIndex = 63;
            driverNameLabel.Text = "Driver Name:";
            driverNameLabel.UseStyleColors = true;
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
            // poisonStyleManager1
            // 
            this.poisonStyleManager1.Owner = null;
            // 
            // poisonLabel2
            // 
            poisonLabel2.AutoSize = true;
            poisonLabel2.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            poisonLabel2.Location = new System.Drawing.Point(171, 153);
            poisonLabel2.Name = "poisonLabel2";
            poisonLabel2.Size = new System.Drawing.Size(30, 19);
            poisonLabel2.TabIndex = 67;
            poisonLabel2.Text = "Url:";
            poisonLabel2.UseStyleColors = true;
            // 
            // UrlpoisonTextBox1
            // 
            // 
            // 
            // 
            this.UrlpoisonTextBox1.CustomButton.Image = null;
            this.UrlpoisonTextBox1.CustomButton.Location = new System.Drawing.Point(103, 2);
            this.UrlpoisonTextBox1.CustomButton.Name = "";
            this.UrlpoisonTextBox1.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.UrlpoisonTextBox1.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.UrlpoisonTextBox1.CustomButton.TabIndex = 1;
            this.UrlpoisonTextBox1.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.UrlpoisonTextBox1.CustomButton.UseSelectable = true;
            this.UrlpoisonTextBox1.CustomButton.Visible = false;
            this.UrlpoisonTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "Url", true));
            this.UrlpoisonTextBox1.Lines = new string[0];
            this.UrlpoisonTextBox1.Location = new System.Drawing.Point(209, 152);
            this.UrlpoisonTextBox1.MaxLength = 32767;
            this.UrlpoisonTextBox1.Name = "UrlpoisonTextBox1";
            this.UrlpoisonTextBox1.PasswordChar = '*';
            this.UrlpoisonTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.UrlpoisonTextBox1.SelectedText = "";
            this.UrlpoisonTextBox1.SelectionLength = 0;
            this.UrlpoisonTextBox1.SelectionStart = 0;
            this.UrlpoisonTextBox1.ShortcutsEnabled = true;
            this.UrlpoisonTextBox1.Size = new System.Drawing.Size(121, 20);
            this.UrlpoisonTextBox1.TabIndex = 66;
            this.UrlpoisonTextBox1.UseSelectable = true;
            this.UrlpoisonTextBox1.UseStyleColors = true;
            this.UrlpoisonTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.UrlpoisonTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // poisonLabel3
            // 
            poisonLabel3.AutoSize = true;
            poisonLabel3.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            poisonLabel3.Location = new System.Drawing.Point(120, 178);
            poisonLabel3.Name = "poisonLabel3";
            poisonLabel3.Size = new System.Drawing.Size(81, 19);
            poisonLabel3.TabIndex = 69;
            poisonLabel3.Text = "Parameters:";
            poisonLabel3.UseStyleColors = true;
            // 
            // ParameterspoisonTextBox1
            // 
            // 
            // 
            // 
            this.ParameterspoisonTextBox1.CustomButton.Image = null;
            this.ParameterspoisonTextBox1.CustomButton.Location = new System.Drawing.Point(103, 2);
            this.ParameterspoisonTextBox1.CustomButton.Name = "";
            this.ParameterspoisonTextBox1.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.ParameterspoisonTextBox1.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.ParameterspoisonTextBox1.CustomButton.TabIndex = 1;
            this.ParameterspoisonTextBox1.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.ParameterspoisonTextBox1.CustomButton.UseSelectable = true;
            this.ParameterspoisonTextBox1.CustomButton.Visible = false;
            this.ParameterspoisonTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "Parameters", true));
            this.ParameterspoisonTextBox1.Lines = new string[0];
            this.ParameterspoisonTextBox1.Location = new System.Drawing.Point(209, 178);
            this.ParameterspoisonTextBox1.MaxLength = 32767;
            this.ParameterspoisonTextBox1.Name = "ParameterspoisonTextBox1";
            this.ParameterspoisonTextBox1.PasswordChar = '*';
            this.ParameterspoisonTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ParameterspoisonTextBox1.SelectedText = "";
            this.ParameterspoisonTextBox1.SelectionLength = 0;
            this.ParameterspoisonTextBox1.SelectionStart = 0;
            this.ParameterspoisonTextBox1.ShortcutsEnabled = true;
            this.ParameterspoisonTextBox1.Size = new System.Drawing.Size(121, 20);
            this.ParameterspoisonTextBox1.TabIndex = 68;
            this.ParameterspoisonTextBox1.UseSelectable = true;
            this.ParameterspoisonTextBox1.UseStyleColors = true;
            this.ParameterspoisonTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.ParameterspoisonTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // poisonLabel4
            // 
            poisonLabel4.AutoSize = true;
            poisonLabel4.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            poisonLabel4.Location = new System.Drawing.Point(142, 205);
            poisonLabel4.Name = "poisonLabel4";
            poisonLabel4.Size = new System.Drawing.Size(59, 19);
            poisonLabel4.TabIndex = 71;
            poisonLabel4.Text = "API Key:";
            poisonLabel4.UseStyleColors = true;
            // 
            // ApiKeypoisonTextBox1
            // 
            // 
            // 
            // 
            this.ApiKeypoisonTextBox1.CustomButton.Image = null;
            this.ApiKeypoisonTextBox1.CustomButton.Location = new System.Drawing.Point(103, 2);
            this.ApiKeypoisonTextBox1.CustomButton.Name = "";
            this.ApiKeypoisonTextBox1.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.ApiKeypoisonTextBox1.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.ApiKeypoisonTextBox1.CustomButton.TabIndex = 1;
            this.ApiKeypoisonTextBox1.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.ApiKeypoisonTextBox1.CustomButton.UseSelectable = true;
            this.ApiKeypoisonTextBox1.CustomButton.Visible = false;
            this.ApiKeypoisonTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "ApiKey", true));
            this.ApiKeypoisonTextBox1.Lines = new string[0];
            this.ApiKeypoisonTextBox1.Location = new System.Drawing.Point(209, 204);
            this.ApiKeypoisonTextBox1.MaxLength = 32767;
            this.ApiKeypoisonTextBox1.Name = "ApiKeypoisonTextBox1";
            this.ApiKeypoisonTextBox1.PasswordChar = '*';
            this.ApiKeypoisonTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ApiKeypoisonTextBox1.SelectedText = "";
            this.ApiKeypoisonTextBox1.SelectionLength = 0;
            this.ApiKeypoisonTextBox1.SelectionStart = 0;
            this.ApiKeypoisonTextBox1.ShortcutsEnabled = true;
            this.ApiKeypoisonTextBox1.Size = new System.Drawing.Size(121, 20);
            this.ApiKeypoisonTextBox1.TabIndex = 70;
            this.ApiKeypoisonTextBox1.UseSelectable = true;
            this.ApiKeypoisonTextBox1.UseStyleColors = true;
            this.ApiKeypoisonTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.ApiKeypoisonTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // poisonLabel5
            // 
            poisonLabel5.AutoSize = true;
            poisonLabel5.FontWeight = ReaLTaiizor.Extension.Poison.PoisonLabelWeight.Regular;
            poisonLabel5.Location = new System.Drawing.Point(96, 231);
            poisonLabel5.Name = "poisonLabel5";
            poisonLabel5.Size = new System.Drawing.Size(105, 19);
            poisonLabel5.TabIndex = 73;
            poisonLabel5.Text = "Certificate Path:";
            poisonLabel5.UseStyleColors = true;
            // 
            // CertificatePathpoisonTextBox1
            // 
            // 
            // 
            // 
            this.CertificatePathpoisonTextBox1.CustomButton.Image = null;
            this.CertificatePathpoisonTextBox1.CustomButton.Location = new System.Drawing.Point(367, 2);
            this.CertificatePathpoisonTextBox1.CustomButton.Name = "";
            this.CertificatePathpoisonTextBox1.CustomButton.Size = new System.Drawing.Size(15, 15);
            this.CertificatePathpoisonTextBox1.CustomButton.Style = ReaLTaiizor.Enum.Poison.ColorStyle.Blue;
            this.CertificatePathpoisonTextBox1.CustomButton.TabIndex = 1;
            this.CertificatePathpoisonTextBox1.CustomButton.Theme = ReaLTaiizor.Enum.Poison.ThemeStyle.Light;
            this.CertificatePathpoisonTextBox1.CustomButton.UseSelectable = true;
            this.CertificatePathpoisonTextBox1.CustomButton.Visible = false;
            this.CertificatePathpoisonTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dataConnectionsBindingSource, "CertificatePath", true));
            this.CertificatePathpoisonTextBox1.Lines = new string[0];
            this.CertificatePathpoisonTextBox1.Location = new System.Drawing.Point(209, 230);
            this.CertificatePathpoisonTextBox1.MaxLength = 32767;
            this.CertificatePathpoisonTextBox1.Name = "CertificatePathpoisonTextBox1";
            this.CertificatePathpoisonTextBox1.PasswordChar = '*';
            this.CertificatePathpoisonTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.CertificatePathpoisonTextBox1.SelectedText = "";
            this.CertificatePathpoisonTextBox1.SelectionLength = 0;
            this.CertificatePathpoisonTextBox1.SelectionStart = 0;
            this.CertificatePathpoisonTextBox1.ShortcutsEnabled = true;
            this.CertificatePathpoisonTextBox1.Size = new System.Drawing.Size(385, 20);
            this.CertificatePathpoisonTextBox1.TabIndex = 72;
            this.CertificatePathpoisonTextBox1.UseSelectable = true;
            this.CertificatePathpoisonTextBox1.UseStyleColors = true;
            this.CertificatePathpoisonTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.CertificatePathpoisonTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // uc_WebApi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.poisonPanel1);
            this.Name = "uc_WebApi";
            this.Size = new System.Drawing.Size(664, 399);
            ((System.ComponentModel.ISupportInitialize)(this.dataConnectionsBindingSource)).EndInit();
            this.poisonPanel2.ResumeLayout(false);
            this.poisonPanel2.PerformLayout();
            this.poisonPanel1.ResumeLayout(false);
            this.poisonPanel1.PerformLayout();
            this.poisonPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.poisonStyleManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.PoisonTextBox connectionNameTextBox;
        private BindingSource dataConnectionsBindingSource;
        private ReaLTaiizor.Controls.PoisonComboBox driverVersionComboBox;
        private ReaLTaiizor.Controls.PoisonComboBox driverNameComboBox;
        private ReaLTaiizor.Controls.PoisonTextBox passwordTextBox;
        private ReaLTaiizor.Controls.PoisonLabel poisonLabel1;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel2;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel1;
        private ReaLTaiizor.Controls.PoisonPanel poisonPanel3;
        private ReaLTaiizor.Controls.PoisonButton ExitCancelpoisonButton;
        private ReaLTaiizor.Controls.PoisonButton SaveButton;
        private ReaLTaiizor.Controls.PoisonTextBox userIDTextBox;
        private ReaLTaiizor.Manager.PoisonStyleManager poisonStyleManager1;
        private ReaLTaiizor.Controls.PoisonTextBox UrlpoisonTextBox1;
        private ReaLTaiizor.Controls.PoisonTextBox ParameterspoisonTextBox1;
        private ReaLTaiizor.Controls.PoisonTextBox CertificatePathpoisonTextBox1;
        private ReaLTaiizor.Controls.PoisonTextBox ApiKeypoisonTextBox1;
    }
}
