using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls.AppBars.Helpers;
using TheTechIdea.Beep.Winform.Controls.Models;

namespace TheTechIdea.Beep.Winform.Controls.AppBars
{
    [ToolboxItem(true)]
    [DisplayName("Beep Advanced AppBar")]
    [Category("Controls")]
    [Description("An AppBar built on the helper-architecture (layout/drawing/events/drag).")]
    public class BeepAdvancedAppBar : BeepControl, IBeepAppBarHost
    {
        private readonly BeepAppBarMasterHelper _master;

        // Backing fields
        private string _title = "Beep Form";
        private bool _showTitle = true;
        private bool _showLogo = true;
        private bool _showSearchBox = true;
        private bool _showProfileIcon = true;
        private bool _showNotificationIcon = true;
        private bool _showThemeIcon = true;
        private bool _showCloseIcon = true;
        private bool _showMaximizeIcon = true;
        private bool _showMinimizeIcon = true;
        private string _logoImage = string.Empty;
        private Size _logoSize = new Size(48, 48);
        private Font _titleFont = new Font("Segoe UI", 10f, FontStyle.Regular);
        private Font _textFont = new Font("Segoe UI", 9f, FontStyle.Regular);
        private int _searchBoxWidth = 150;
        private int _titleLabelWidth = 200;

        public BeepAdvancedAppBar()
        {
            // Rendering styles
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            UpdateStyles();

            IsBorderAffectedByTheme = false;
            IsShadowAffectedByTheme = false;
            IsRoundedAffectedByTheme = false;
            ShowAllBorders = false;
            ShowShadow = false;
            IsFrameless = false;
            IsRounded = false;
            ApplyThemeToChilds = false;

            _master = new BeepAppBarMasterHelper(this);

            // bubble helper events
            _master.Clicked += (s, e) => Clicked?.Invoke(this, e);
            _master.OnButtonClicked += (s, e) => OnButtonClicked?.Invoke(this, e);
            _master.OnSearchBoxSelected += (s, e) => OnSearchBoxSelected?.Invoke(this, e);
        }

        #region Events
        public event EventHandler<BeepMouseEventArgs> Clicked;
        public event EventHandler<BeepAppBarEventsArgs> OnButtonClicked;
        public event EventHandler<BeepAppBarEventsArgs> OnSearchBoxSelected;
        #endregion

        #region IBeepAppBarHost implementation
        Control IBeepAppBarHost.AsControl => this;
        IBeepTheme IBeepAppBarHost.CurrentTheme => _currentTheme; // from BeepControl/BaseControl
        // Use explicit interface to avoid name shadowing issues with Control.DesignMode
        bool IBeepAppBarHost.DesignMode => DesignMode;

        public Rectangle DrawingRect => base.DrawingRect;
        public new string Theme { get => base.Theme; set => base.Theme = value; }
        public Size Size => base.Size;

        public string Title { get => _title; set { if (_title == value) return; _title = value ?? string.Empty; _master.HandlePropertyChanged(nameof(Title)); } }
        public bool ShowTitle { get => _showTitle; set { if (_showTitle == value) return; _showTitle = value; _master.HandlePropertyChanged(nameof(ShowTitle)); } }
        public bool ShowLogo { get => _showLogo; set { if (_showLogo == value) return; _showLogo = value; _master.HandlePropertyChanged(nameof(ShowLogo)); } }
        public bool ShowSearchBox { get => _showSearchBox; set { if (_showSearchBox == value) return; _showSearchBox = value; _master.HandlePropertyChanged(nameof(ShowSearchBox)); } }
        public bool ShowProfileIcon { get => _showProfileIcon; set { if (_showProfileIcon == value) return; _showProfileIcon = value; _master.HandlePropertyChanged(nameof(ShowProfileIcon)); } }
        public bool ShowNotificationIcon { get => _showNotificationIcon; set { if (_showNotificationIcon == value) return; _showNotificationIcon = value; _master.HandlePropertyChanged(nameof(ShowNotificationIcon)); } }
        public bool ShowThemeIcon { get => _showThemeIcon; set { if (_showThemeIcon == value) return; _showThemeIcon = value; _master.HandlePropertyChanged(nameof(ShowThemeIcon)); } }
        public bool ShowCloseIcon { get => _showCloseIcon; set { if (_showCloseIcon == value) return; _showCloseIcon = value; _master.HandlePropertyChanged(nameof(ShowCloseIcon)); } }
        public bool ShowMaximizeIcon { get => _showMaximizeIcon; set { if (_showMaximizeIcon == value) return; _showMaximizeIcon = value; _master.HandlePropertyChanged(nameof(ShowMaximizeIcon)); } }
        public bool ShowMinimizeIcon { get => _showMinimizeIcon; set { if (_showMinimizeIcon == value) return; _showMinimizeIcon = value; _master.HandlePropertyChanged(nameof(ShowMinimizeIcon)); } }
        public string LogoImage { get => _logoImage; set { if (_logoImage == value) return; _logoImage = value ?? string.Empty; _master.HandlePropertyChanged(nameof(LogoImage)); } }
        public Size LogoSize { get => _logoSize; set { if (_logoSize == value) return; _logoSize = value; _master.HandlePropertyChanged(nameof(LogoSize)); } }
        public Font TitleFont { get => _titleFont; set { _titleFont = value ?? _titleFont; _master.HandlePropertyChanged(nameof(TitleFont)); } }
        public Font TextFont { get => _textFont; set { _textFont = value ?? _textFont; _master.HandlePropertyChanged(nameof(TextFont)); } }
        public int SearchBoxWidth { get => _searchBoxWidth; }
        public int TitleLabelWidth { get => _titleLabelWidth; }

        public void InvalidateLayout() => _master.RefreshLayout();
        public int ScaleValue(int value) => base.ScaleValue(value);
        public Size ScaleSize(Size size) => base.ScaleSize(size);
        #endregion

        #region Drawing
        protected override void DrawContent(Graphics g)
        {
            base.DrawContent(g);
            _master.DrawAll(g);
        }
        #endregion

        #region Mouse/Resize
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            _master.HandleMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Cursor = _master.HandleMouseMove(e) ?? Cursors.Default;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _master.HandleMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _master.HandleMouseLeave();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _master.HandleResize();
        }
        #endregion

        #region Theme
        public override void ApplyTheme()
        {
            base.ApplyTheme();
            _master.ApplyTheme();
        }
        #endregion

        #region Public helpers
        public void SetDraggableAreas(params string[] areas) => _master.SetDraggableAreas(areas);
        public void SetFormDraggingEnabled(bool enabled) => _master.SetFormDraggingEnabled(enabled);
        public void ShowBadgeOnNotificationIcon(string badgeText) => _master.ShowBadgeOnNotificationIcon(badgeText);

        public void GetLayoutRectangles(out Rectangle logo, out Rectangle title, out Rectangle search,
            out Rectangle notification, out Rectangle profile, out Rectangle theme,
            out Rectangle minimize, out Rectangle maximize, out Rectangle close)
            => _master.GetLayoutRectangles(out logo, out title, out search, out notification, out profile, out theme, out minimize, out maximize, out close);
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _master?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
