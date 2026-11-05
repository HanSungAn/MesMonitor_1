using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MesMonitor.Models;
using MesMonitor.Services;
using SkiaSharp;

namespace MesMonitor.ViewModels;

public class EquipmentSensorViewModel : ViewModelBase
{
    private readonly IMesDataSimulator _simulator;

    public ObservableCollection<LineStatusItem> EquipmentList => _simulator.Lines;

    private LineStatusItem? _selectedEquipment;
    public LineStatusItem? SelectedEquipment
    {
        get => _selectedEquipment;
        set
        {
            if (SetProperty(ref _selectedEquipment, value) && value != null)
            {
                LoadEquipmentData(value.LineId);
            }
        }
    }

    // Real-time sensor metrics
    private double _currentTemperature;
    public double CurrentTemperature
    {
        get => _currentTemperature;
        set => SetProperty(ref _currentTemperature, value);
    }

    private double _currentPressure;
    public double CurrentPressure
    {
        get => _currentPressure;
        set => SetProperty(ref _currentPressure, value);
    }

    private double _currentRpm;
    public double CurrentRpm
    {
        get => _currentRpm;
        set => SetProperty(ref _currentRpm, value);
    }

    private double _currentVibration;
    public double CurrentVibration
    {
        get => _currentVibration;
        set => SetProperty(ref _currentVibration, value);
    }

    private double _currentPower;
    public double CurrentPower
    {
        get => _currentPower;
        set => SetProperty(ref _currentPower, value);
    }

    // Chart Observable Values
    public ObservableCollection<double> TemperatureValues { get; } = new();
    public ObservableCollection<double> PressureValues { get; } = new();
    public ObservableCollection<double> VibrationValues { get; } = new();

    public ISeries[] SensorSeries { get; private set; }
    public Axis[] XAxes { get; private set; }
    public Axis[] YAxes { get; private set; }

    public IRelayCommand<string> ChangeStateCommand { get; }

    public EquipmentSensorViewModel(IMesDataSimulator simulator)
    {
        Title = "설비 센서 모니터링 & 상태 분석 (IoT Sensor & Diagnostics)";
        _simulator = simulator;

        SensorSeries = new ISeries[]
        {
            new LineSeries<double>
            {
                Name = "온도 (°C)",
                Values = TemperatureValues,
                Stroke = new SolidColorPaint(new SKColor(239, 68, 68), 2.5f), // Red
                Fill = null,
                GeometrySize = 6,
                GeometryStroke = new SolidColorPaint(new SKColor(239, 68, 68), 2f),
                ScalesYAt = 0
            },
            new LineSeries<double>
            {
                Name = "진동 (G)",
                Values = VibrationValues,
                Stroke = new SolidColorPaint(new SKColor(245, 158, 11), 2f), // Amber
                Fill = null,
                GeometrySize = 5,
                GeometryStroke = new SolidColorPaint(new SKColor(245, 158, 11), 1.5f),
                ScalesYAt = 1
            },
            new LineSeries<double>
            {
                Name = "압력 (bar)",
                Values = PressureValues,
                Stroke = new SolidColorPaint(new SKColor(59, 130, 246), 2f), // Blue
                Fill = null,
                GeometrySize = 5,
                GeometryStroke = new SolidColorPaint(new SKColor(59, 130, 246), 1.5f),
                ScalesYAt = 1
            }
        };

        XAxes = new Axis[]
        {
            new Axis
            {
                Name = "시계열 샘플 (실시간 스트림)",
                NamePaint = new SolidColorPaint(new SKColor(148, 163, 184)),
                LabelsPaint = new SolidColorPaint(new SKColor(148, 163, 184)),
                TextSize = 11
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                Name = "온도 (°C)",
                NamePaint = new SolidColorPaint(new SKColor(239, 68, 68)),
                LabelsPaint = new SolidColorPaint(new SKColor(239, 68, 68)),
                MinLimit = 20,
                MaxLimit = 110,
                SeparatorsPaint = new SolidColorPaint(new SKColor(51, 65, 85, 80))
            },
            new Axis
            {
                Name = "압력(bar) / 진동(G)",
                NamePaint = new SolidColorPaint(new SKColor(59, 130, 246)),
                LabelsPaint = new SolidColorPaint(new SKColor(59, 130, 246)),
                Position = LiveChartsCore.Measure.AxisPosition.End,
                MinLimit = 0,
                MaxLimit = 12,
                ShowSeparatorLines = false
            }
        };

        ChangeStateCommand = new RelayCommand<string>(ExecuteChangeState);

        SelectedEquipment = EquipmentList.FirstOrDefault();
        _simulator.SensorStreamUpdated += OnSensorStreamUpdated;
    }

    private void ExecuteChangeState(string? stateStr)
    {
        if (SelectedEquipment == null || string.IsNullOrEmpty(stateStr)) return;

        if (Enum.TryParse<MachineState>(stateStr, out var state))
        {
            _simulator.SetLineState(SelectedEquipment.LineId, state);
            SelectedEquipment.State = state;
            OnPropertyChanged(nameof(SelectedEquipment));
        }
    }

    private void LoadEquipmentData(string lineId)
    {
        var history = _simulator.GetRecentSensorHistory(lineId, 25);
        TemperatureValues.Clear();
        PressureValues.Clear();
        VibrationValues.Clear();

        foreach (var p in history)
        {
            TemperatureValues.Add(p.Temperature);
            PressureValues.Add(p.Pressure);
            VibrationValues.Add(p.Vibration);
        }

        var latest = _simulator.GetCurrentSensorData(lineId);
        CurrentTemperature = latest.Temperature;
        CurrentPressure = latest.Pressure;
        CurrentVibration = latest.Vibration;
        CurrentRpm = latest.Rpm;
        CurrentPower = latest.PowerUsage;
    }

    private void OnSensorStreamUpdated(string lineId, SensorDataPoint point)
    {
        if (SelectedEquipment?.LineId == lineId)
        {
            CurrentTemperature = point.Temperature;
            CurrentPressure = point.Pressure;
            CurrentVibration = point.Vibration;
            CurrentRpm = point.Rpm;
            CurrentPower = point.PowerUsage;

            TemperatureValues.Add(point.Temperature);
            PressureValues.Add(point.Pressure);
            VibrationValues.Add(point.Vibration);

            if (TemperatureValues.Count > 30) TemperatureValues.RemoveAt(0);
            if (PressureValues.Count > 30) PressureValues.RemoveAt(0);
            if (VibrationValues.Count > 30) VibrationValues.RemoveAt(0);
        }
    }
}

