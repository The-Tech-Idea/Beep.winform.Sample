using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Winform.Controls
{

    public partial class FloatingBadgeForm : Form
    {
        public string BadgeText { get; set; } = "";
        public Color BadgeBackColor { get; set; } = Color.Red;
        public Color BadgeForeColor { get; set; } = Color.White;
        public Font BadgeFont { get; set; } = new Font("Arial", 10, FontStyle.Bold);
        public BadgeShape BadgeShape { get; set; } = BadgeShape.Circle;
        public int BorderRadius { get; set; } = 10; // Only for rounded rectangles

        private Size _lastSize = Size.Empty;

        public FloatingBadgeForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.Magenta;
            TransparencyKey = Color.Magenta;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // **?? STEP 1: Measure text size before drawing**
            SizeF textSize = e.Graphics.MeasureString(BadgeText, BadgeFont);
            int padding = 10;
            int badgeWidth = Math.Max((int)textSize.Width + padding, 20);
            int badgeHeight = Math.Max((int)textSize.Height + padding, 20);

            // Keep circle proportional
            if (BadgeShape == BadgeShape.Circle)
            {
                int diameter = Math.Max(badgeWidth, badgeHeight);
                badgeWidth = badgeHeight = diameter;
            }

            // **?? STEP 2: Resize badge if necessary**
            if (_lastSize.Width != badgeWidth || _lastSize.Height != badgeHeight)
            {
                this.Size = new Size(badgeWidth, badgeHeight);
                _lastSize = this.Size;
                Invalidate(); // Repaint with new size
                return; // Stop drawing on this frame, will repaint on next
            }

            // **?? STEP 3: Draw the badge background**
            using (GraphicsPath path = new GraphicsPath())
            {
                switch (BadgeShape)
                {
                    case BadgeShape.Circle:
                        if (Width > 0 && Height > 0)
                        {
                            path.AddEllipse(new Rectangle(0, 0, Width, Height));
                            this.Region = new Region(path);
                        }
                        break;

                    case BadgeShape.RoundedRectangle:
                        Rectangle rrRect = new Rectangle(0, 0, Width, Height);
                        if (rrRect.Width > 0 && rrRect.Height > 0)
                        {
                            AddRoundedRectangle(path, rrRect, BorderRadius);
                            this.Region = new Region(path);
                        }
                        break;

                    case BadgeShape.Rectangle:
                    default:
                        if (Width > 0 && Height > 0)
                        {
                            path.AddRectangle(new Rectangle(0, 0, Width, Height));
                            this.Region = new Region(path);
                        }
                        break;
                }

                if (path.PointCount > 0)
                {
                    using (SolidBrush brush = new SolidBrush(BadgeBackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            }

            // **?? STEP 4: Draw the text**
            if (!string.IsNullOrEmpty(BadgeText))
            {
                using (SolidBrush textBrush = new SolidBrush(BadgeForeColor))
                {
                    var textRect = new Rectangle(0, 0, Width, Height);
                    var format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(BadgeText, BadgeFont, textBrush, textRect, format);
                }
            }
        }

        private void AddRoundedRectangle(GraphicsPath path, Rectangle rect, int radius)
        {
            // Validate input rectangle - if width or height is too small, fall back to rectangle
            if (rect.Width <= 0 || rect.Height <= 0 || radius <= 0)
            {
                if (rect.Width > 0 && rect.Height > 0)
                {
                    path.AddRectangle(rect);
                }
                return;
            }

            int diameter = radius * 2;

            // Ensure diameter doesn't exceed rectangle dimensions
            if (diameter > rect.Width)
                diameter = rect.Width;
            if (diameter > rect.Height)
                diameter = rect.Height;
            
            // If diameter becomes too small after adjustment, fall back to rectangle
            if (diameter <= 0)
            {
                path.AddRectangle(rect);
                return;
            }

            try
            {
                // Top-left arc
                Rectangle arcRect = new Rectangle(rect.X, rect.Y, diameter, diameter);
                if (arcRect.Width > 0 && arcRect.Height > 0)
                    path.AddArc(arcRect, 180, 90);

                // Top-right arc
                arcRect.X = rect.Right - diameter;
                if (arcRect.X >= rect.X && arcRect.Width > 0 && arcRect.Height > 0)
                    path.AddArc(arcRect, 270, 90);

                // Bottom-right arc
                arcRect.Y = rect.Bottom - diameter;
                if (arcRect.Y >= rect.Y && arcRect.Width > 0 && arcRect.Height > 0)
                    path.AddArc(arcRect, 0, 90);

                // Bottom-left arc
                arcRect.X = rect.X;
                if (arcRect.Width > 0 && arcRect.Height > 0)
                    path.AddArc(arcRect, 90, 90);

                path.CloseFigure();
            }
            catch (ArgumentException)
            {
                // If any arc operation fails, clear the path and fall back to a rectangle
                path.Reset();
                if (rect.Width > 0 && rect.Height > 0)
                {
                    path.AddRectangle(rect);
                }
            }
        }
    }
}