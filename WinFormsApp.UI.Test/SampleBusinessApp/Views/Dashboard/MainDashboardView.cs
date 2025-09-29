using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.Container.Services;
using TheTechIdea.Beep.Editor;
using TheTechIdea.Beep.Utilities;
using TheTechIdea.Beep.Vis;
using TheTechIdea.Beep.Vis.Modules;
using TheTechIdea.Beep.Winform.Controls;
using TheTechIdea.Beep.Winform.Controls.Charts;
using TheTechIdea.Beep.Winform.Controls.StatusCards;
using TheTechIdea.Beep.Winform.Default.Views.Template;
using WinFormsApp.UI.Test.SampleBusinessApp.Data;

namespace WinFormsApp.UI.Test.SampleBusinessApp.Views.Dashboard
{
    [AddinAttribute(Caption = "Dashboard", Name = "MainDashboardView", misc = "SampleBusinessApp", menu = "Dashboard", ObjectType = "Beep")]
    [AddinVisSchema(BranchID = 100, RootNodeName = "Sample Business App", Order = 100, ID = 100,
        BranchText = "Dashboard", BranchType = EnumPointType.Function,
        IconImageName = "dashboard.svg", BranchClass = "ADDIN",
        BranchDescription = "Executive overview dashboard")]
    public class MainDashboardView : TemplateUserControl, IAddinVisSchema
    {
        private readonly IDMEEditor _editor;

        private BeepPanel _header;
        private BeepPanel _content;
        private BeepLabel _title;
        private BeepButton _refreshBtn;
        private BeepLabel _statusLbl;
        private BeepStatCard _kpi1;
        private BeepStatCard _kpi2;
        private BeepStatCard _kpi3;
        private BeepChart _chart;
        private CancellationTokenSource _cts;

        public string RootNodeName { get; set; } = "Sample Business App";
        public string CatgoryName { get; set; }
        public int Order { get; set; } = 100;
        public int ID { get; set; } = 100;
        public string BranchText { get; set; } = "Dashboard";
        public int Level { get; set; }
        public EnumPointType BranchType { get; set; } = EnumPointType.Function;
        public int BranchID { get; set; } = 100;
        public string IconImageName { get; set; } = "dashboard.svg";
        public string BranchStatus { get; set; }
        public int ParentBranchID { get; set; }
        public string BranchDescription { get; set; } = "Executive overview dashboard";
        public string BranchClass { get; set; } = "ADDIN";
        public string AddinName { get; set; }

        public MainDashboardView(IServiceProvider services) : base(services)
        {
            _editor = services.GetRequiredService<IDMEEditor>();
            InitializeComponent();
            BuildUI();
            _ = LoadDashboardAsync();
        }

        private void BuildUI()
        {
            _header = new BeepPanel { Dock = DockStyle.Top, Height = 60, ShowTitle = false, Padding = new Padding(12), Theme = this.Theme };
            _title = new BeepLabel { Text = "?? Executive Dashboard", AutoSize = true, Font = new Font("Segoe UI", 16, FontStyle.Bold), Theme = this.Theme };
            _header.Controls.Add(_title);

            // Refresh and status
            _refreshBtn = new BeepButton { Text = "Refresh", Size = new Size(90, 30), Theme = this.Theme, IsRounded = true, BorderRadius = 6 };
            _refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _refreshBtn.Location = new Point(Width - 200, 15);
            _refreshBtn.Click += async (s, e) => await LoadDashboardAsync();
            _statusLbl = new BeepLabel { Text = string.Empty, AutoSize = true, Theme = this.Theme };
            _statusLbl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _statusLbl.Location = new Point(Width - 100, 20);
            _header.Controls.Add(_refreshBtn);
            _header.Controls.Add(_statusLbl);

            _content = new BeepPanel { Dock = DockStyle.Fill, ShowTitle = false, Padding = new Padding(12), Theme = this.Theme };

            // KPI cards
            _kpi1 = new BeepStatCard { HeaderText = "Total Customers", ValueText = "-", Location = new Point(12, 12), Size = new Size(250, 120), Theme = this.Theme };
            _kpi2 = new BeepStatCard { HeaderText = "Open Invoices", ValueText = "-", Location = new Point(274, 12), Size = new Size(250, 120), Theme = this.Theme };
            _kpi3 = new BeepStatCard { HeaderText = "Revenue (30d)", ValueText = "-", Location = new Point(536, 12), Size = new Size(250, 120), Theme = this.Theme };

            // Chart
            _chart = new BeepChart { Location = new Point(12, 146), Size = new Size(1000, 400), Theme = this.Theme, ChartType = ChartType.Line, ChartTitle = "Revenue by Day (Last 14d)", XAxisTitle = "Day", YAxisTitle = "Paid Amount" };

            _content.Controls.AddRange(new Control[] { _kpi1, _kpi2, _kpi3, _chart });

            Controls.Add(_content);
            Controls.Add(_header);
        }

        private async Task LoadDashboardAsync()
        {
            try
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                var token = _cts.Token;
                _statusLbl.Text = "Loading...";
                _refreshBtn.Enabled = false;

                var ds = AppDbContext.EnsureSqliteDataSource(_editor) as TheTechIdea.Beep.DataBase.IRDBSource;
                ds?.Openconnection();
                if (ds == null)
                {
                    _statusLbl.Text = "No data source";
                    _refreshBtn.Enabled = true;
                    return;
                }

                var result = await Task.Run(() =>
                {
                    var totalCustomers = Convert.ToInt32(ds.GetScalar("SELECT COUNT(*) FROM Customers"));
                    var openInvoices = Convert.ToInt32(ds.GetScalar("SELECT COUNT(*) FROM Invoices WHERE PaymentStatus <> 'Paid'"));
                    var revenue30 = Convert.ToDecimal(ds.GetScalar("SELECT COALESCE(SUM(PaidAmount),0) FROM Invoices WHERE InvoiceDate >= datetime('now','-30 day')"));

                    var series = new ChartDataSeries { Name = "Revenue", Color = Color.SeaGreen, ShowLine = true, ShowPoint = true, ChartType = ChartType.Line };
                    var start = DateTime.Today.AddDays(-13);
                    for (int i = 0; i < 14; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        var dayDate = start.AddDays(i);
                        var day = dayDate.ToString("yyyy-MM-dd");
                        var amtObj = ds.GetScalar($"SELECT COALESCE(SUM(PaidAmount),0) FROM Invoices WHERE date(InvoiceDate) = date('{day}')");
                        decimal amt = 0m;
                        try { amt = Convert.ToDecimal(amtObj); } catch { amt = 0m; }
                        series.Points.Add(new ChartDataPoint(day, amt.ToString(), (float)amt, day));
                    }

                    return (totalCustomers, openInvoices, revenue30, series);
                }, token);

                if (IsHandleCreated)
                {
                    BeginInvoke(new Action(() =>
                    {
                        _kpi1.ValueText = result.totalCustomers.ToString("N0");
                        _kpi2.ValueText = result.openInvoices.ToString("N0");
                        _kpi3.ValueText = result.revenue30.ToString("C0");
                        _chart.DataSeries = new List<ChartDataSeries> { result.series };
                        _chart.Invalidate();
                        _statusLbl.Text = string.Empty;
                        _refreshBtn.Enabled = true;
                    }));
                }
                else
                {
                    _refreshBtn.Enabled = true;
                    _statusLbl.Text = string.Empty;
                }
            }
            catch (OperationCanceledException)
            {
                if (IsHandleCreated)
                {
                    BeginInvoke(new Action(() => { _statusLbl.Text = string.Empty; _refreshBtn.Enabled = true; }));
                }
            }
            catch (Exception ex)
            {
                _editor.AddLogMessage("Dashboard", $"Failed to load dashboard: {ex.Message}", DateTime.Now, -1, null, Errors.Failed);
                if (IsHandleCreated)
                {
                    BeginInvoke(new Action(() => { _statusLbl.Text = "Load failed"; _refreshBtn.Enabled = true; }));
                }
            }
        }

        private void LoadDashboard()
        {
            _ = LoadDashboardAsync();
        }

        public override void Configure(Dictionary<string, object> settings) => base.Configure(settings);
        public override void OnNavigatedTo(Dictionary<string, object> parameters)
        {
            base.OnNavigatedTo(parameters);
            _ = LoadDashboardAsync();
        }
        public override void Initialize() => base.Initialize();

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = nameof(MainDashboardView);
            Size = new Size(1200, 800);
            ResumeLayout(false);
        }
    }
}
