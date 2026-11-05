using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MesMonitor.Models;

namespace MesMonitor.Converters;

public class MachineStateToBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush RunningBrush = new(Color.FromRgb(16, 185, 129)); // #10B981
    private static readonly SolidColorBrush IdleBrush = new(Color.FromRgb(245, 158, 11));     // #F59E0B
    private static readonly SolidColorBrush AlarmBrush = new(Color.FromRgb(239, 68, 68));    // #EF4444
    private static readonly SolidColorBrush MaintenanceBrush = new(Color.FromRgb(107, 114, 128)); // #6B7280

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MachineState state)
        {
            return state switch
            {
                MachineState.Running => RunningBrush,
                MachineState.Idle => IdleBrush,
                MachineState.Alarm => AlarmBrush,
                MachineState.Maintenance => MaintenanceBrush,
                _ => RunningBrush
            };
        }
        return RunningBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw mechanical();

    private static NotImplementedException mechanical() => new();
}

public class AlarmSeverityToBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush CriticalBrush = new(Color.FromRgb(239, 68, 68));
    private static readonly SolidColorBrush WarningBrush = new(Color.FromRgb(245, 158, 11));
    private static readonly SolidColorBrush InfoBrush = new(Color.FromRgb(59, 130, 246));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AlarmSeverity severity)
        {
            return severity switch
            {
                AlarmSeverity.Critical => CriticalBrush,
                AlarmSeverity.Warning => WarningBrush,
                AlarmSeverity.Info => InfoBrush,
                _ => InfoBrush
            };
        }
        return InfoBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class OrderStatusToBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush InProgressBrush = new(Color.FromRgb(59, 130, 246));
    private static readonly SolidColorBrush CompletedBrush = new(Color.FromRgb(16, 185, 129));
    private static readonly SolidColorBrush WaitingBrush = new(Color.FromRgb(148, 163, 184));
    private static readonly SolidColorBrush PausedBrush = new(Color.FromRgb(245, 158, 11));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is OrderStatus status)
        {
            return status switch
            {
                OrderStatus.InProgress => InProgressBrush,
                OrderStatus.Completed => CompletedBrush,
                OrderStatus.Waiting => WaitingBrush,
                OrderStatus.Paused => PausedBrush,
                _ => WaitingBrush
            };
        }
        return WaitingBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}

