using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using MesMonitor.Models;
using MesMonitor.Services;
using SkiaSharp;

namespace MesMonitor.ViewModels;

public class QualitySpcViewModel : ViewModelBase
{
    private readonly IMesDataSimulator _simulator;

    public List<DefectItem> DefectStats => _simulator.DefectStats;
    public List<SpcDataPoint> SpcSamples => _simulator.SpcSamples;

    // Pareto Chart
    public ISeries[] ParetoSeries { get; private set; }
    public Axis[] ParetoXAxes { get; private set; }
    public Axis[] ParetoYAxes { get; private set; }

    // SPC Control Chart
    public ISeries[] SpcSeries { get; private set; }
    public Axis[] SpcXAxes { get; private set; }
    public Axis[] SpcYAxes { get; private set; }

    // Quality KPI Metrics
    public double CpkValue { get; } = 1.33;
    public double CpValue { get; } = 1.48;
    public int TotalInspected { get; } = 3200;
    public int TotalDefects { get; } = 69;
    public double DefectRatePercent => Math.Round((double)TotalDefects / TotalInspected * 100, 2);
    public int Ppm => (int)Math.Round(((double)TotalDefects / TotalInspected) * 1_000_000);

    public QualitySpcViewModel(IMesDataSimulator simulator)
    {
        Title = "품질 관리 및 SPC 공정 분석 (Pareto & Xbar Control Chart)";
        _simulator = simulator;

        // Setup Pareto Chart
        var defectLabels = _simulator.DefectStats.Select(d => d.DefectType).ToArray();
        var counts = _simulator.DefectStats.Select(d => d.Count).ToArray();
        var cumPercents = _simulator.DefectStats.Select(d => d.CumulativePercentage).ToArray();

        ParetoSeries = new ISeries[]
        {
            new ColumnSeries<int>
            {
                Name = "불량 발생 건수",
                Values = counts,
                Fill = new SolidColorPaint(new SKColor(239, 68, 68)), // Red
                MaxBarWidth = 32,
                ScalesYAt = 0
            },
            new LineSeries<double>
            {
                Name = "누적 백분율 (%)",
                Values = cumPercents,
                Stroke = new SolidColorPaint(new SKColor(245, 158, 11), 3f), // Amber
                Fill = null,
                GeometrySize = 8,
                GeometryStroke = new SolidColorPaint(new SKColor(245, 158, 11), 2f),
                GeometryFill = new SolidColorPaint(SKColors.White),
                ScalesYAt = 1
            }
        };

        ParetoXAxes = new Axis[]
        {
            new Axis
            {
                Labels = defectLabels,
                LabelsPaint = new SolidColorPaint(new SKColor(148, 163, 184)),
                LabelsRotation = 15,
                TextSize = 11
            }
        };

        ParetoYAxes = new Axis[]
        {
            new Axis
            {
                Name = "불량 건수 (EA)",
                NamePaint = new SolidColorPaint(new SKColor(239, 68, 68)),
                LabelsPaint = new SolidColorPaint(new SKColor(239, 68, 68)),
                SeparatorsPaint = new SolidColorPaint(new SKColor(51, 65, 85, 80))
            },
            new Axis
            {
                Name = "누적 비율 (%)",
                NamePaint = new SolidColorPaint(new SKColor(245, 158, 11)),
                LabelsPaint = new SolidColorPaint(new SKColor(245, 158, 11)),
                Position = LiveChartsCore.Measure.AxisPosition.End,
                MinLimit = 0,
                MaxLimit = 100,
                ShowSeparatorLines = false
            }
        };

        // Setup SPC Control Chart (Xbar)
        var spcPoints = _simulator.SpcSamples;
        var sampleIndexes = spcPoints.Select(p => $"#{p.SampleIndex}").ToArray();
        var actualValues = spcPoints.Select(p => p.Value).ToArray();
        var uclValues = spcPoints.Select(p => p.UCL).ToArray();
        var clValues = spcPoints.Select(p => p.CL).ToArray();
        var lclValues = spcPoints.Select(p => p.LCL).ToArray();

        SpcSeries = new ISeries[]
        {
            new LineSeries<double>
            {
                Name = "UCL (상한선: 25.45)",
                Values = uclValues,
                Stroke = new SolidColorPaint(new SKColor(239, 68, 68, 180), 1.5f) { PathEffect = new DashEffect(new float[] { 6, 6 }) },
                Fill = null,
                GeometrySize = 0
            },
            new LineSeries<double>
            {
                Name = "CL (중심선: 25.00)",
                Values = clValues,
                Stroke = new SolidColorPaint(new SKColor(16, 185, 129, 200), 1.5f) { PathEffect = new DashEffect(new float[] { 4, 4 }) },
                Fill = null,
                GeometrySize = 0
            },
            new LineSeries<double>
            {
                Name = "LCL (하한선: 24.55)",
                Values = lclValues,
                Stroke = new SolidColorPaint(new SKColor(239, 68, 68, 180), 1.5f) { PathEffect = new DashEffect(new float[] { 6, 6 }) },
                Fill = null,
                GeometrySize = 0
            },
            new LineSeries<double>
            {
                Name = "측정 실측치 (mm)",
                Values = actualValues,
                Stroke = new SolidColorPaint(new SKColor(59, 130, 246), 2.5f),
                Fill = null,
                GeometrySize = 7,
                GeometryStroke = new SolidColorPaint(new SKColor(59, 130, 246), 2f),
                GeometryFill = new SolidColorPaint(SKColors.White)
            }
        };

        SpcXAxes = new Axis[]
        {
            new Axis
            {
                Labels = sampleIndexes,
                LabelsPaint = new SolidColorPaint(new SKColor(148, 163, 184)),
                TextSize = 10
            }
        };

        SpcYAxes = new Axis[]
        {
            new Axis
            {
                Name = "규격 수치 (mm)",
                NamePaint = new SolidColorPaint(new SKColor(148, 163, 184)),
                LabelsPaint = new SolidColorPaint(new SKColor(148, 163, 184)),
                MinLimit = 24.3,
                MaxLimit = 25.7,
                SeparatorsPaint = new SolidColorPaint(new SKColor(51, 65, 85, 80))
            }
        };
    }
}

