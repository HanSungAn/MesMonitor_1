using System.Collections.ObjectModel;
using MesMonitor.Models;

namespace MesMonitor.Services;

public interface IMesDataSimulator
{
    ObservableCollection<LineStatusItem> Lines { get; }
    ObservableCollection<WorkOrderItem> WorkOrders { get; }
    ObservableCollection<AlarmItem> Alarms { get; }
    List<DefectItem> DefectStats { get; }
    List<SpcDataPoint> SpcSamples { get; }

    event Action? DataUpdated;
    event Action<string, SensorDataPoint>? SensorStreamUpdated;

    void Start();
    void Stop();
    SensorDataPoint GetCurrentSensorData(string lineId);
    List<SensorDataPoint> GetRecentSensorHistory(string lineId, int count = 20);
    void AddWorkOrder(WorkOrderItem order);
    void AcknowledgeAlarm(AlarmItem alarm);
    void SetLineState(string lineId, MachineState state);
}

