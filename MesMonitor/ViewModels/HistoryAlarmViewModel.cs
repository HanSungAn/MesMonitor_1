using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MesMonitor.Models;
using MesMonitor.Services;

namespace MesMonitor.ViewModels;

public class HistoryAlarmViewModel : ViewModelBase
{
    private readonly IMesDataSimulator _simulator;

    public ObservableCollection<AlarmItem> AllAlarms => _simulator.Alarms;
    public ObservableCollection<AlarmItem> FilteredAlarms { get; } = new();

    private string _severityFilter = "전체";
    public string SeverityFilter
    {
        get => _severityFilter;
        set
        {
            if (SetProperty(ref _severityFilter, value))
            {
                ApplyFilter();
            }
        }
    }

    private string _statusMessage = "시스템 정상 가동 중";
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public IRelayCommand<AlarmItem> AcknowledgeCommand { get; }
    public IRelayCommand ExportCsvCommand { get; }

    public HistoryAlarmViewModel(IMesDataSimulator simulator)
    {
        Title = "알람 이력 및 기준정보 설정 (Alarm Logs & Master Configuration)";
        _simulator = simulator;

        AcknowledgeCommand = new RelayCommand<AlarmItem>(ExecuteAcknowledge);
        ExportCsvCommand = new RelayCommand(ExecuteExportCsv);

        ApplyFilter();
        _simulator.DataUpdated += OnDataUpdated;
    }

    private void OnDataUpdated()
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        FilteredAlarms.Clear();
        var query = AllAlarms.AsEnumerable();

        if (SeverityFilter == "Critical")
            query = query.Where(a => a.Severity == AlarmSeverity.Critical);
        else if (SeverityFilter == "Warning")
            query = query.Where(a => a.Severity == AlarmSeverity.Warning);
        else if (SeverityFilter == "Info")
            query = query.Where(a => a.Severity == AlarmSeverity.Info);

        foreach (var a in query)
        {
            FilteredAlarms.Add(a);
        }
    }

    private void ExecuteAcknowledge(AlarmItem? alarm)
    {
        if (alarm == null) return;
        _simulator.AcknowledgeAlarm(alarm);
        ApplyFilter();
        StatusMessage = $"알람 확인 완료: [{alarm.EquipmentName}] {alarm.Message}";
    }

    private void ExecuteExportCsv()
    {
        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, $"MES_Alarm_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");

            using (var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("Timestamp,EquipmentId,EquipmentName,Severity,Message,IsAcknowledged");
                foreach (var a in AllAlarms)
                {
                    writer.WriteLine($"{a.Timestamp:yyyy-MM-dd HH:mm:ss},{a.EquipmentId},{a.EquipmentName},{a.Severity},\"{a.Message}\",{a.IsAcknowledged}");
                }
            }

            StatusMessage = $"CSV 내보내기 성공: {Path.GetFileName(filePath)}";
            MessageBox.Show($"CSV 파일이 성공적으로 생성되었습니다:\n{filePath}", "알람 내보내기", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            StatusMessage = $"내보내기 실패: {ex.Message}";
        }
    }
}

