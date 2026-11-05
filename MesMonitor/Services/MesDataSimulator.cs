using System.Collections.ObjectModel;
using System.Windows;
using MesMonitor.Models;

namespace MesMonitor.Services;

public class MesDataSimulator : IMesDataSimulator
{
    private readonly System.Timers.Timer _timer;
    private readonly Random _random = new();
    private readonly Dictionary<string, List<SensorDataPoint>> _sensorHistories = new();

    public ObservableCollection<LineStatusItem> Lines { get; } = new();
    public ObservableCollection<WorkOrderItem> WorkOrders { get; } = new();
    public ObservableCollection<AlarmItem> Alarms { get; } = new();
    public List<DefectItem> DefectStats { get; private set; } = new();
    public List<SpcDataPoint> SpcSamples { get; private set; } = new();

    public event Action? DataUpdated;
    public event Action<string, SensorDataPoint>? SensorStreamUpdated;

    public MesDataSimulator()
    {
        InitializeSeedData();
        _timer = new System.Timers.Timer(1500);
        _timer.Elapsed += OnTimerElapsed;
    }

    private void InitializeSeedData()
    {
        // 1. Initial Production Lines
        Lines.Add(new LineStatusItem
        {
            LineId = "L01",
            LineName = "Line 01 - SMT 표면실장",
            State = MachineState.Running,
            CurrentOrderNo = "WO-261105-01",
            ProductName = "PCB Control Board v3",
            TargetQty = 1200,
            ProducedQty = 890,
            DefectQty = 12,
            Efficiency = 94.5,
            Temperature = 58.4,
            Rpm = 1820
        });

        Lines.Add(new LineStatusItem
        {
            LineId = "L02",
            LineName = "Line 02 - CNC 정밀가공",
            State = MachineState.Running,
            CurrentOrderNo = "WO-261105-02",
            ProductName = "Aluminum Heat Sink B-Type",
            TargetQty = 800,
            ProducedQty = 610,
            DefectQty = 7,
            Efficiency = 91.2,
            Temperature = 72.1,
            Rpm = 3450
        });

        Lines.Add(new LineStatusItem
        {
            LineId = "L03",
            LineName = "Line 03 - 사출성형 (Injection)",
            State = MachineState.Alarm,
            CurrentOrderNo = "WO-261105-03",
            ProductName = "ABS Enclosure Case #4",
            TargetQty = 1500,
            ProducedQty = 980,
            DefectQty = 45,
            Efficiency = 76.8,
            Temperature = 88.6,
            Rpm = 940
        });

        Lines.Add(new LineStatusItem
        {
            LineId = "L04",
            LineName = "Line 04 - 최종조립 및 검사",
            State = MachineState.Running,
            CurrentOrderNo = "WO-261105-04",
            ProductName = "Smart Inverter Module",
            TargetQty = 500,
            ProducedQty = 340,
            DefectQty = 5,
            Efficiency = 96.0,
            Temperature = 42.0,
            Rpm = 1200
        });

        // Seed Sensor Histories
        foreach (var line in Lines)
        {
            var history = new List<SensorDataPoint>();
            var now = DateTime.Now;
            for (int i = 25; i >= 0; i--)
            {
                history.Add(new SensorDataPoint
                {
                    Timestamp = now.AddSeconds(-i * 3),
                    Temperature = Math.Round(line.Temperature + (_random.NextDouble() * 4 - 2), 1),
                    Pressure = Math.Round(5.0 + (_random.NextDouble() * 1.5 - 0.7), 2),
                    Vibration = Math.Round(1.2 + (_random.NextDouble() * 0.4 - 0.2), 2),
                    Rpm = Math.Round(line.Rpm + (_random.Next(80) - 40), 0),
                    PowerUsage = Math.Round(14.5 + (_random.NextDouble() * 2 - 1), 1)
                });
            }
            _sensorHistories[line.LineId] = history;
        }

        // 2. Initial Work Orders
        WorkOrders.Add(new WorkOrderItem
        {
            OrderNo = "WO-261105-01",
            LineId = "L01",
            ProductCode = "PRD-PCB-03",
            ProductName = "PCB Control Board v3",
            PlanQty = 1200,
            ActualQty = 890,
            DefectQty = 12,
            Status = OrderStatus.InProgress,
            StartDate = DateTime.Today.AddHours(8),
            DueDate = DateTime.Today.AddHours(18)
        });

        WorkOrders.Add(new WorkOrderItem
        {
            OrderNo = "WO-261105-02",
            LineId = "L02",
            ProductCode = "PRD-MEC-88",
            ProductName = "Aluminum Heat Sink B-Type",
            PlanQty = 800,
            ActualQty = 610,
            DefectQty = 7,
            Status = OrderStatus.InProgress,
            StartDate = DateTime.Today.AddHours(8).AddMinutes(30),
            DueDate = DateTime.Today.AddHours(17)
        });

        WorkOrders.Add(new WorkOrderItem
        {
            OrderNo = "WO-261105-03",
            LineId = "L03",
            ProductCode = "PRD-CAS-12",
            ProductName = "ABS Enclosure Case #4",
            PlanQty = 1500,
            ActualQty = 980,
            DefectQty = 45,
            Status = OrderStatus.Paused,
            StartDate = DateTime.Today.AddHours(7),
            DueDate = DateTime.Today.AddHours(19)
        });

        WorkOrders.Add(new WorkOrderItem
        {
            OrderNo = "WO-261105-04",
            LineId = "L04",
            ProductCode = "PRD-ASY-09",
            ProductName = "Smart Inverter Module",
            PlanQty = 500,
            ActualQty = 340,
            DefectQty = 5,
            Status = OrderStatus.InProgress,
            StartDate = DateTime.Today.AddHours(9),
            DueDate = DateTime.Today.AddHours(20)
        });

        WorkOrders.Add(new WorkOrderItem
        {
            OrderNo = "WO-261105-05",
            LineId = "L01",
            ProductCode = "PRD-PCB-05",
            ProductName = "Power Supply Unit Board",
            PlanQty = 1000,
            ActualQty = 0,
            DefectQty = 0,
            Status = OrderStatus.Waiting,
            StartDate = DateTime.Today.AddHours(18),
            DueDate = DateTime.Today.AddDays(1)
        });

        WorkOrders.Add(new WorkOrderItem
        {
            OrderNo = "WO-261105-06",
            LineId = "L02",
            ProductCode = "PRD-MEC-14",
            ProductName = "Motor Shaft Precision Rod",
            PlanQty = 1200,
            ActualQty = 1200,
            DefectQty = 8,
            Status = OrderStatus.Completed,
            StartDate = DateTime.Today.AddHours(6),
            DueDate = DateTime.Today.AddHours(12)
        });

        // 3. Initial Alarms
        Alarms.Add(new AlarmItem
        {
            Timestamp = DateTime.Now.AddMinutes(-12),
            EquipmentId = "L03",
            EquipmentName = "Line 03 - 사출기 Noズル",
            Severity = AlarmSeverity.Critical,
            Message = "사출 노즐 온도 상한 임계치(88°C) 초과 경보 발생",
            IsAcknowledged = false
        });

        Alarms.Add(new AlarmItem
        {
            Timestamp = DateTime.Now.AddMinutes(-34),
            EquipmentId = "L02",
            EquipmentName = "Line 02 - 주축 스핀들",
            Severity = AlarmSeverity.Warning,
            Message = "스핀들 진동 주파수 편차 경고 (1.5G 초과 감지)",
            IsAcknowledged = true
        });

        Alarms.Add(new AlarmItem
        {
            Timestamp = DateTime.Now.AddHours(-1).AddMinutes(15),
            EquipmentId = "L01",
            EquipmentName = "Line 01 - 피더 #12",
            Severity = AlarmSeverity.Info,
            Message = "자재(0603 Chip) 잔량 부족 사전 알림 (10% 이하)",
            IsAcknowledged = true
        });

        // 4. Initial Defect Stats for Pareto Chart
        var rawDefects = new List<(string Type, int Count)>
        {
            ("표면 스크래치 (Scratch)", 142),
            ("치수 공차 미달 (Dimension)", 86),
            ("기포 발생 (Void/Bubble)", 48),
            ("납땜 불량 (Cold Solder)", 25),
            ("이물질 혼입 (Foreign Matter)", 16),
            ("크랙 및 파손 (Crack)", 9),
            ("기타 불량 (Etc)", 4)
        };

        double totalDefectCount = rawDefects.Sum(x => x.Count);
        double cumCount = 0;
        DefectStats = rawDefects.Select(d =>
        {
            cumCount += d.Count;
            return new DefectItem
            {
                DefectType = d.Type,
                Count = d.Count,
                Percentage = Math.Round((d.Count / totalDefectCount) * 100, 1),
                CumulativePercentage = Math.Round((cumCount / totalDefectCount) * 100, 1)
            };
        }).ToList();

        // 5. Initial SPC Samples (Control Chart)
        double target = 25.0; // Specification 25.0mm
        double stdDev = 0.15;
        double ucl = target + 3 * stdDev; // 25.45
        double lcl = target - 3 * stdDev; // 24.55
        SpcSamples = new List<SpcDataPoint>();
        for (int i = 1; i <= 25; i++)
        {
            // Introduce one or two out-of-spec points for realistic demonstration
            double val;
            if (i == 17) val = 25.49; // UCL violation!
            else if (i == 9) val = 24.51; // Near boundary
            else val = Math.Round(target + (_random.NextDouble() * 2 - 1) * (stdDev * 1.5), 3);

            SpcSamples.Add(new SpcDataPoint
            {
                SampleIndex = i,
                Value = Math.Round(val, 3),
                CL = target,
                UCL = ucl,
                LCL = lcl
            });
        }
    }

    public void Start() => _timer.Start();
    public void Stop() => _timer.Stop();

    private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        Application.Current?.Dispatcher?.Invoke(() =>
        {
            // Simulate Line Updates & Production Output
            foreach (var line in Lines)
            {
                if (line.State == MachineState.Running)
                {
                    // Small increments to production
                    if (_random.Next(10) > 3 && line.ProducedQty < line.TargetQty)
                    {
                        // 생산 수량 증가 (1 ~ 3개 중 랜덤 증가)
                        line.ProducedQty += _random.Next(1, 4);

                        // 확률 시뮬레이션 (0 ~ 24 중 0일 때 ➔ 1/25 확률, 즉 4% 확률로 불량 발생)
                        if ( _random.Next(25) == 0)
                        {
                            line.DefectQty += 1;
                        }
                    }

                    // Temperature and RPM jitter
                    // 음수/양수 떨림 (Jitter) 시뮬레이션
                    line.Temperature = Math.Round(line.Temperature + (_random.NextDouble() * 0.6 - 0.3), 1);
                    line.Rpm = Math.Round(line.Rpm + (_random.Next(20) - 10), 0);
                }

                // Update Sensor History
                if (_sensorHistories.TryGetValue(line.LineId, out var history))
                {
                    var newPoint = new SensorDataPoint
                    {
                        Timestamp = DateTime.Now,
                        Temperature = line.Temperature,
                        Pressure = Math.Round(5.0 + (_random.NextDouble() * 1.2 - 0.6), 2),
                        Vibration = Math.Round(1.2 + (_random.NextDouble() * 0.3 - 0.15), 2),
                        Rpm = line.Rpm,
                        PowerUsage = Math.Round(14.0 + (_random.NextDouble() * 1.8 - 0.9), 1)
                    };

                    history.Add(newPoint);
                    if (history.Count > 40)
                    {
                        history.RemoveAt(0);
                    }

                    SensorStreamUpdated?.Invoke(line.LineId, newPoint);
                }
            }

            // Sync with corresponding WorkOrders
            foreach (var order in WorkOrders.Where(o => o.Status == OrderStatus.InProgress))
            {
                var matchedLine = Lines.FirstOrDefault(l => l.LineId == order.LineId);
                if (matchedLine != null)
                {
                    order.ActualQty = matchedLine.ProducedQty;
                    order.DefectQty = matchedLine.DefectQty;
                }
            }

            DataUpdated?.Invoke();
        });
    }

    public SensorDataPoint GetCurrentSensorData(string lineId)
    {
        if (_sensorHistories.TryGetValue(lineId, out var list) && list.Count > 0)
        {
            return list[^1];
        }
        return new SensorDataPoint { Timestamp = DateTime.Now, Temperature = 50, Pressure = 5 };
    }

    public List<SensorDataPoint> GetRecentSensorHistory(string lineId, int count = 20)
    {
        if (_sensorHistories.TryGetValue(lineId, out var list))
        {
            return list.TakeLast(count).ToList();
        }
        return new List<SensorDataPoint>();
    }

    public void AddWorkOrder(WorkOrderItem order)
    {
        WorkOrders.Insert(0, order);
        DataUpdated?.Invoke();
    }

    public void AcknowledgeAlarm(AlarmItem alarm)
    {
        alarm.IsAcknowledged = true;
        DataUpdated?.Invoke();
    }

    public void SetLineState(string lineId, MachineState state)
    {
        var line = Lines.FirstOrDefault(l => l.LineId == lineId);
        if (line != null)
        {
            line.State = state;
            DataUpdated?.Invoke();
        }
    }
}

