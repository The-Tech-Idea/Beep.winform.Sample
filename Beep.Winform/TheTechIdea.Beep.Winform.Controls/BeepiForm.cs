using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace TheTechIdea.Beep.Winform.Controls
{
    public partial class BeepiForm : Form
    {
        private int _borderRadius = 30;
        private int _borderThickness = 5;
        private Color _borderColor = Color.Black;

        public BeepiForm()
        {
            InitializeComponent();
            // Don't set region until form is properly initialized and has valid dimensions
            // UpdateFormRegion will be called in OnResize when dimensions are valid
        }

        protected override void OnResize(EventArgs e)
        {
            SuspendLayout();
            base.OnResize(e);
            
            // Only update form region if we have valid dimensions
            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                UpdateFormRegion(); // keep Region in sync while resizing (prevents "transparent" corners)
                Invalidate();       // repaint current frame
            }
            
            ResumeLayout(true);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // Now that the form is shown and has valid dimensions, update the region
            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                UpdateFormRegion();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_borderThickness > 0 && _borderColor != Color.Transparent && ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                using var borderPen = new Pen(_borderColor, _borderThickness);
                var rect = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
                
                if (rect.Width > 0 && rect.Height > 0)
                {
                    if (_borderRadius > 0)
                    {
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        using var path = GetRoundedRectanglePath(rect, _borderRadius);
                        if (path.PointCount > 0) // Only draw if path is valid
                        {
                            e.Graphics.DrawPath(borderPen, path);
                        }
                    }
                    else
                    {
                        e.Graphics.DrawRectangle(borderPen, rect);
                    }
                }
            }
        }

        private void UpdateFormRegion()
        {
            if (_borderRadius > 0 && ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                Rectangle rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
                using (GraphicsPath path = GetRoundedRectanglePath(rect, _borderRadius))
                {
                    if (path.PointCount > 0) // Only set region if path is valid
                    {
                        this.Region = new Region(path);
                    }
                    else
                    {
                        this.Region = null;
                    }
                }
            }
            else
            {
                this.Region = null;
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            // Validate input rectangle - if width or height is too small, fall back to rectangle
            if (rect.Width <= 0 || rect.Height <= 0 || radius <= 0)
            {
                if (rect.Width > 0 && rect.Height > 0)
                {
                    path.AddRectangle(rect);
                }
                return path;
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
                if (rect.Width > 0 && rect.Height > 0)
                {
                    path.AddRectangle(rect);
                }
                return path;
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

            return path;
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The radius of the form's border.")]
        [DefaultValue(5)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius
        {
            get { return _borderRadius; }
            set 
            { 
                _borderRadius = Math.Max(0, value); // Ensure non-negative value
                // Only update region if we have valid dimensions
                if (IsHandleCreated && ClientSize.Width > 0 && ClientSize.Height > 0)
                {
                    UpdateFormRegion(); 
                }
                Invalidate(); 
            }
        }
    }
}