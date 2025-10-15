using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using TheTechIdea.Beep.Winform.Controls.Common;

namespace TheTechIdea.Beep.Winform.Controls.Styling.ShadowPainters
{
    /// <summary>
    /// Helper utilities for painting shadows
    /// Provides common methods for soft shadows, material elevation, and special effects
    /// </summary>
    public static class ShadowPainterHelpers
    {
        /// <summary>
        /// Creates a rounded rectangle path for shadow
        /// </summary>
        public static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Creates an offset path from a GraphicsPath
        /// </summary>
        private static GraphicsPath CreateOffsetPath(GraphicsPath sourcePath, int offsetX, int offsetY, int spread, int radius)
        {
            RectangleF bounds = sourcePath.GetBounds();
            Rectangle offsetBounds = new Rectangle(
                (int)bounds.X + offsetX - spread,
                (int)bounds.Y + offsetY - spread,
                (int)bounds.Width + (spread * 2),
                (int)bounds.Height + (spread * 2)
            );
            
            int adjustedRadius = Math.Max(0, radius + spread);
            return CreateRoundedRectangle(offsetBounds, adjustedRadius);
        }

        /// <summary>
        /// Creates an inset path from a GraphicsPath
        /// </summary>
        private static GraphicsPath CreateInsetPath(GraphicsPath sourcePath, int inset, int radius)
        {
            RectangleF bounds = sourcePath.GetBounds();
            Rectangle insetBounds = new Rectangle(
                (int)bounds.X + inset,
                (int)bounds.Y + inset,
                Math.Max(1, (int)bounds.Width - (inset * 2)),
                Math.Max(1, (int)bounds.Height - (inset * 2))
            );
            
            return CreateRoundedRectangle(insetBounds, Math.Max(0, radius - inset));
        }

        /// <summary>
        /// Paints a soft multi-layer shadow
        /// </summary>
        public static GraphicsPath PaintSoftShadow(Graphics g, GraphicsPath bounds, int radius, int offsetX, int offsetY, 
            Color shadowColor, float opacity, int layers = 6)
        {
            if (opacity <= 0 || opacity > 1) return (GraphicsPath)bounds.Clone();

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;

            for (int i = 1; i <= layers; i++)
            {
                float layerOpacityFactor = (float)(layers - i + 1) / layers;
                float finalOpacity = opacity * layerOpacityFactor * 0.6f;
                int layerAlpha = Math.Max(5, (int)(255 * finalOpacity));

                Color layerShadowColor = Color.FromArgb(layerAlpha, shadowColor);

                int spread = i - 1;

                using (SolidBrush shadowBrush = new SolidBrush(layerShadowColor))
                using (GraphicsPath shadowPath = CreateOffsetPath(bounds, offsetX, offsetY, spread, radius))
                {
                    g.FillPath(shadowBrush, shadowPath);
                }
            }

            // Return the area inside the shadow
            int maxInset = Math.Max(Math.Abs(offsetX), Math.Abs(offsetY)) + layers;
            return CreateInsetPath(bounds, maxInset, radius);
        }

        /// <summary>
        /// Paints Material Design elevation shadow
        /// </summary>
        public static GraphicsPath PaintMaterialShadow(Graphics g, GraphicsPath bounds, int radius, MaterialElevation elevation)
        {
            if (elevation == MaterialElevation.Level0) return (GraphicsPath)bounds.Clone();

            // Material shadows use two layers: key light (top) and ambient light (bottom)
            int elevationValue = (int)elevation;
            
            // Key light shadow (directional, smaller)
            int keyOffsetY = elevationValue * 2;
            int keyBlur = elevationValue * 2;
            Color keyShadowColor = Color.FromArgb(40, 0, 0, 0);
            
            // Ambient light shadow (larger, softer)
            int ambientOffsetY = elevationValue;
            int ambientBlur = elevationValue * 4;
            Color ambientShadowColor = Color.FromArgb(30, 0, 0, 0);

            // Draw ambient shadow first (larger)
            PaintSoftShadow(g, bounds, radius, 0, ambientOffsetY, ambientShadowColor, 0.3f, ambientBlur);
            
            // Draw key shadow on top (smaller, more defined)
            PaintSoftShadow(g, bounds, radius, 0, keyOffsetY, keyShadowColor, 0.4f, keyBlur);
            
            // Return the area inside the shadow
            int maxInset = Math.Max(keyOffsetY, ambientOffsetY) + Math.Max(keyBlur, ambientBlur);
            return CreateInsetPath(bounds, maxInset, radius);
        }

        /// <summary>
        /// Paints neumorphic embossed shadow (dual shadow for raised effect)
        /// </summary>
        public static GraphicsPath PaintNeumorphicShadow(Graphics g, GraphicsPath bounds, int radius, Color backgroundColor)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Light shadow (top-left)
            Color lightShadow = Lighten(backgroundColor, 0.15f);
            using (SolidBrush lightBrush = new SolidBrush(Color.FromArgb(80, lightShadow)))
            using (GraphicsPath lightPath = CreateOffsetPath(bounds, -4, -4, 0, radius))
            {
                g.FillPath(lightBrush, lightPath);
            }

            // Dark shadow (bottom-right)
            Color darkShadow = Darken(backgroundColor, 0.15f);
            using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(80, darkShadow)))
            using (GraphicsPath darkPath = CreateOffsetPath(bounds, 4, 4, 0, radius))
            {
                g.FillPath(darkBrush, darkPath);
            }
            
            // Return the area inside the shadow (8 pixel inset for the 4+4 offset)
            return CreateInsetPath(bounds, 8, radius);
        }

        /// <summary>
        /// Paints a glow effect (for DarkGlow style)
        /// </summary>
        public static GraphicsPath PaintGlow(Graphics g, GraphicsPath bounds, int radius, Color glowColor, float intensity)
        {
            if (intensity <= 0) return (GraphicsPath)bounds.Clone();

            g.SmoothingMode = SmoothingMode.AntiAlias;

            int glowSize = (int)(8 * intensity);
            
            for (int i = 0; i < glowSize; i++)
            {
                int alpha = (int)(30 * intensity * (1f - (float)i / glowSize));
                if (alpha <= 0) continue;

                using (SolidBrush glowBrush = new SolidBrush(Color.FromArgb(alpha, glowColor)))
                using (GraphicsPath glowPath = CreateOffsetPath(bounds, -i, -i, i, radius))
                {
                    g.FillPath(glowBrush, glowPath);
                }
            }
            
            // Return the area inside the glow
            return CreateInsetPath(bounds, glowSize, radius);
        }

        /// <summary>
        /// Lightens a color by a percentage
        /// </summary>
        public static Color Lighten(Color color, float percent)
        {
            return Color.FromArgb(
                color.A,
                Math.Min(255, color.R + (int)(255 * percent)),
                Math.Min(255, color.G + (int)(255 * percent)),
                Math.Min(255, color.B + (int)(255 * percent))
            );
        }

        /// <summary>
        /// Darkens a color by a percentage
        /// </summary>
        public static Color Darken(Color color, float percent)
        {
            return Color.FromArgb(
                color.A,
                Math.Max(0, color.R - (int)(color.R * percent)),
                Math.Max(0, color.G - (int)(color.G * percent)),
                Math.Max(0, color.B - (int)(color.B * percent))
            );
        }
    }
}
