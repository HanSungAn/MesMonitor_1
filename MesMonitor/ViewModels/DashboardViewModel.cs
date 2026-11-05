using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using MesMonitor.Helpers;
using MesMonitor.Models;
using MesMonitor.Services;
using SkiaSharp;

namespace MesMonitor.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly IMesDataSimulator _simulator;
    private readonly IThemeService _themeService;

    private int _totalPlanQty;
    public int TotalPlanQty
    {
        get => _totalPlanQty;
        set => SetProperty(ref _totalPlanQty, value);
    }

    private int _totalProducedQty;
    public int TotalProducedQty
    {
        get => _totalProducedQty;
        set => SetProperty(ref _totalProducedQty, value);
    }

    private int _totalDefectQty;
    public int TotalDefectQty
    {
        get => _totalDefectQty;
        set => SetProperty(ref _totalDefectQty, value);
    }

    private double _overallOee;
    public double OverallOee
    {
        get => _overallOee;
        set => SetProperty(ref _overallOee, value);
    }

    private double _averageEfficiency;
    public double AverageEfficiency
    {
        get => _averageEfficiency;
        set => SetProperty(ref _averageEfficiency, value);
    }

    private int _activeAlarmCount;
    public int ActiveAlarmCount
    {
        get => _activeAlarmCount;
        set => SetProperty(ref _activeAlarmCount, value);
    }

    public ObservableCollection<LineStatusItem> Lines => _simulator.Lines;
    public ObservableCollection<AlarmItem> RecentAlarms => _simulator.Alarms;

    // Charts
    private ISeries[] _oeeSeries = Array.Empty<ISeries>();
    public ISeries[] OeeSeries
    {
        get => _oeeSeries;
        private set => SetProperty(ref _oeeSeries, value);
    }

    private ISeries[] _hourlyProductionSeries = Array.Empty<ISeries>();
    public ISeries[] HourlyProductionSeries
    {
        get => _hourlyProductionSeries;
        private set => SetProperty(ref _hourlyProductionSeries, value);
    }

    private Axis[] _hourlyXAxes = Array.Empty<Axis>();
    public Axis[] HourlyXAxes
    {
        get => _hourlyXAxes;
        private set => SetProperty(ref _hourlyXAxes, value);
    }

    private Axis[] _hourlyYAxes = Array.Empty<Axis>();
    public Axis[] HourlyYAxes
    {
        get => _hourlyYAxes;
        private set => SetProperty(ref _hourlyYAxes, value);
    }

    private SolidColorPaint _chartLegendTextPaint;
    public SolidColorPaint ChartLegendTextPaint
    {
        get => _chartLegendTextPaint;
        private set => SetProperty(ref _chartLegendTextPaint, value);
    }

    private SolidColorPaint _chartTooltipBgPaint;
    public SolidColorPaint ChartTooltipBgPaint
    {
        get => _chartTooltipBgPaint;
        private set => SetProperty(ref _chartTooltipBgPaint, value);
    }

    private SolidColorPaint _chartTooltipTextPaint;
    public SolidColorPaint ChartTooltipTextPaint
    {
        get => _chartTooltipTextPaint;
        private set => SetProperty(ref _chartTooltipTextPaint, value);
    }

    public DashboardViewModel(IMesDataSimulator simulator, IThemeService themeService)
    {
        Title = "종합 생산 관제 대시보드 (Dashboard & Andon)";
        _simulator = simulator;
        _themeService = themeService;

        _chartLegendTextPaint = ChartThemeHelper.GetLegendTextPaint(_themeService.CurrentTheme);
        _chartTooltipBgPaint = ChartThemeHelper.GetTooltipBgPaint(_themeService.CurrentTheme);
        _chartTooltipTextPaint = ChartThemeHelper.GetTooltipTextPaint(_themeService.CurrentTheme);

        InitializeCharts(_themeService.CurrentTheme);

        _themeService.ThemeChanged += OnThemeChanged;

        UpdateKpiMetrics();
        _simulator.DataUpdated += OnSimulatorDataUpdated;
    }

    private void InitializeCharts(AppTheme theme)
    {
        ChartLegendTextPaint = ChartThemeHelper.GetLegendTextPaint(theme);
        ChartTooltipBgPaint = ChartThemeHelper.GetTooltipBgPaint(theme);
        ChartTooltipTextPaint = ChartThemeHelper.GetTooltipTextPaint(theme);

        OeeSeries = new ISeries[]
        {
            new PieSeries<double>
            {
                Name = "시간 가동률 (88.4%)",
                Values = new double[] { 88.4 },
                InnerRadius = 55,
                Fill = new SolidColorPaint(new SKColor(16, 185, 129)) // Green
            },
            new PieSeries<double>
            {
                Name = "성능 가동률 (92.8%)",
                Values = new double[] { 92.8 },
                InnerRadius = 55,
                Fill = new SolidColorPaint(new SKColor(59, 130, 246)) // Blue
            },
            new PieSeries<double>
            {
                Name = "양품률 (98.6%)",
                Values = new double[] { 98.6 },
                InnerRadius = 55,
                Fill = new SolidColorPaint(new SKColor(245, 158, 11)) // Amber
            }
        };

        HourlyProductionSeries = new ISeries[]
        {
            new ColumnSeries<int>
            {
                Name = "계획 목표",
                Values = new int[] { 350, 350, 380, 380, 400, 400, 420, 420 },
                Fill = ChartThemeHelper.GetTargetBarPaint(theme),
                MaxBarWidth = 24
            },
            new ColumnSeries<int>
            {
                Name = "실제 생산량",
                Values = new int[] { 342, 355, 375, 360, 412, 395, 388, 290 },
                Fill = new SolidColorPaint(new SKColor(6, 182, 212)), // Cyan
                MaxBarWidth = 24
            }
        };

        HourlyXAxes = new Axis[]
        {
            new Axis
            {
                Labels = new string[] { "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00" },
                LabelsPaint = ChartThemeHelper.GetLabelsPaint(theme),
                TextSize = 12
            }
        };

        HourlyYAxes = new Axis[]
        {
            new Axis
            {
                LabelsPaint = ChartThemeHelper.GetLabelsPaint(theme),
                SeparatorsPaint = ChartThemeHelper.GetSeparatorsPaint(theme)
            }
        };
    }

    private void OnThemeChanged(AppTheme theme)
    {
        InitializeCharts(theme);
    }

    private void OnSimulatorDataUpdated()
    {
        UpdateKpiMetrics();
    }

    private void UpdateKpiMetrics()
    {
        TotalPlanQty = Lines.Sum(l => l.TargetQty);
        TotalProducedQty = Lines.Sum(l => l.ProducedQty);
        TotalDefectQty = Lines.Sum(l => l.DefectQty);
        AverageEfficiency = Lines.Count > 0 ? Math.Round(Lines.Average(l => l.Efficiency), 1) : 0;
        
        // OEE = Availability * Performance * Quality
        double avail = 0.884;
        double perf = 0.928;
        double quality = TotalProducedQty > 0 ? (double)(TotalProducedQty - TotalDefectQty) / TotalProducedQty : 0.98;
        OverallOee = Math.Round(avail * perf * quality * 100, 1);

        ActiveAlarmCount = _simulator.Alarms.Count(a => !a.IsAcknowledged);
    }
}
