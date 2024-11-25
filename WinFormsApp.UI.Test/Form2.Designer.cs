namespace WinFormsApp.UI.Test
{
    partial class Form2
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
            beepImage1 = new TheTechIdea.Beep.Winform.Controls.BeepImage();
            SuspendLayout();
            // 
            // beepImage1
            // 
            beepImage1.ActiveBackColor = Color.Gray;
            beepImage1.AnimationDuration = 500;
            beepImage1.AnimationType = TheTechIdea.Beep.Winform.Controls.DisplayAnimationType.None;
            beepImage1.ApplyThemeOnImage = false;
            beepImage1.BlockID = null;
            beepImage1.BorderColor = Color.Black;
            beepImage1.BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            beepImage1.BorderRadius = 5;
            beepImage1.BorderStyle = BorderStyle.FixedSingle;
            beepImage1.BorderThickness = 1;
            beepImage1.DataContext = null;
            beepImage1.DisabledBackColor = Color.Gray;
            beepImage1.DisabledForeColor = Color.Empty;
            beepImage1.DrawingRect = new Rectangle(4, 4, 92, 92);
            beepImage1.Easing = TheTechIdea.Beep.Winform.Controls.EasingType.Linear;
            beepImage1.FieldID = null;
            beepImage1.FocusBackColor = Color.Gray;
            beepImage1.FocusBorderColor = Color.Gray;
            beepImage1.FocusForeColor = Color.Black;
            beepImage1.FocusIndicatorColor = Color.Blue;
            beepImage1.Form = null;
            beepImage1.GradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            beepImage1.GradientEndColor = Color.Gray;
            beepImage1.GradientStartColor = Color.Gray;
            beepImage1.HoverBackColor = Color.Gray;
            beepImage1.HoverBorderColor = Color.Gray;
            beepImage1.HoveredBackcolor = Color.Wheat;
            beepImage1.HoverForeColor = Color.Black;
            beepImage1.Id = -1;
            beepImage1.Image = null;
            beepImage1.ImagePath = null;
            beepImage1.InactiveBackColor = Color.Gray;
            beepImage1.InactiveBorderColor = Color.Gray;
            beepImage1.InactiveForeColor = Color.Black;
            beepImage1.IsAcceptButton = false;
            beepImage1.IsBorderAffectedByTheme = true;
            beepImage1.IsCancelButton = false;
            beepImage1.IsChild = false;
            beepImage1.IsCustomeBorder = false;
            beepImage1.IsDefault = false;
            beepImage1.IsFocused = false;
            beepImage1.IsFramless = false;
            beepImage1.IsHovered = false;
            beepImage1.IsPressed = false;
            beepImage1.IsRounded = true;
            beepImage1.IsRoundedAffectedByTheme = true;
            beepImage1.IsShadowAffectedByTheme = true;
            beepImage1.IsStillImage = false;
            beepImage1.Location = new Point(283, 151);
            beepImage1.Name = "beepImage1";
            beepImage1.OverrideFontSize = TheTechIdea.Beep.Winform.Controls.TypeStyleFontSize.None;
            beepImage1.ParentBackColor = Color.Empty;
            beepImage1.PressedBackColor = Color.Gray;
            beepImage1.PressedBorderColor = Color.Gray;
            beepImage1.PressedForeColor = Color.Black;
            beepImage1.SavedGuidID = null;
            beepImage1.SavedID = null;
            beepImage1.ScaleMode = TheTechIdea.Beep.Winform.Controls.ImageScaleMode.KeepAspectRatio;
            beepImage1.ShadowColor = Color.Black;
            beepImage1.ShadowOffset = 3;
            beepImage1.ShadowOpacity = 0.5F;
            beepImage1.ShowAllBorders = true;
            beepImage1.ShowBottomBorder = true;
            beepImage1.ShowFocusIndicator = false;
            beepImage1.ShowLeftBorder = true;
            beepImage1.ShowRightBorder = true;
            beepImage1.ShowShadow = true;
            beepImage1.ShowTopBorder = true;
            beepImage1.Size = new Size(100, 100);
            beepImage1.SlideFrom = TheTechIdea.Beep.Winform.Controls.SlideDirection.Left;
            beepImage1.StaticNotMoving = false;
            beepImage1.TabIndex = 0;
            beepImage1.Text = "beepImage1";
            beepImage1.Theme = TheTechIdea.Beep.Vis.Modules.EnumBeepThemes.DefaultTheme;
            beepImage1.ToolTipText = "";
            beepImage1.UseGradientBackground = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(beepImage1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private TheTechIdea.Beep.Winform.Controls.BeepImage beepImage1;
    }
}