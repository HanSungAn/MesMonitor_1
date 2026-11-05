using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MesMonitor.Services;

namespace MesMonitor.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IMesDataSimulator _simulator;
    private readonly IThemeService _themeService;

    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    private string _currentTab = "Dashboard";
    public string CurrentTab
    {
        get => _currentTab;
        set => SetProperty(ref _currentTab, value);
    }

    private string _currentTimeString = string.Empty;
    public string CurrentTimeString
    {
        get => _currentTimeString;
        set => SetProperty(ref _currentTimeString, value);
    }

    public bool IsDarkMode => _themeService.CurrentTheme == AppTheme.Dark;
    public string ThemeButtonText => IsDarkMode ? "☀️ 라이트 모드" : "🌙 다크 모드";

    public DashboardViewModel DashboardVM { get; }
    public EquipmentSensorViewModel EquipmentVM { get; }
    public QualitySpcViewModel QualityVM { get; }
    public WorkOrderViewModel WorkOrderVM { get; }
    public HistoryAlarmViewModel HistoryVM { get; }

    public IRelayCommand<string> NavigateCommand { get; }
    public IRelayCommand ToggleThemeCommand { get; }

    public MainViewModel()
    {
        _simulator = new MesDataSimulator();
        _themeService = new ThemeService();

        DashboardVM = new DashboardViewModel(_simulator, _themeService);
        EquipmentVM = new EquipmentSensorViewModel(_simulator, _themeService);
        QualityVM = new QualitySpcViewModel(_simulator, _themeService);
        WorkOrderVM = new WorkOrderViewModel(_simulator);
        HistoryVM = new HistoryAlarmViewModel(_simulator);

        _currentViewModel = DashboardVM;

        NavigateCommand = new RelayCommand<string>(ExecuteNavigate);
        ToggleThemeCommand = new RelayCommand(ExecuteToggleTheme);

        // Clock timer
        var clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        clockTimer.Tick += (s, e) => CurrentTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        clockTimer.Start();
        CurrentTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        _simulator.Start();
    }

    private void ExecuteToggleTheme()
    {
        _themeService.ToggleTheme();
        OnPropertyChanged(nameof(IsDarkMode));
        OnPropertyChanged(nameof(ThemeButtonText));
    }

    private void ExecuteNavigate(string? tab)
    {
        if (string.IsNullOrEmpty(tab)) return;
        CurrentTab = tab;

        CurrentViewModel = tab switch
        {
            "Dashboard" => DashboardVM,
            "Equipment" => EquipmentVM,
            "Quality" => QualityVM,
            "WorkOrder" => WorkOrderVM,
            "History" => HistoryVM,
            _ => DashboardVM
        };
    }
}
