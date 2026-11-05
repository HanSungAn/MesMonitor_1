using LiveChartsCore.SkiaSharpView.Painting;
using MesMonitor.Services;
using SkiaSharp;

namespace MesMonitor.Helpers;

public static class ChartThemeHelper
{
    public static SolidColorPaint GetLabelsPaint(AppTheme theme)
    {
        return theme == AppTheme.Dark
            ? new SolidColorPaint(new SKColor(203, 213, 225)) // Slate-300
            : new SolidColorPaint(new SKColor(71, 85, 105));   // Slate-600
    }

    public static SolidColorPaint GetTitlePaint(AppTheme theme)
    {
        return theme == AppTheme.Dark
            ? new SolidColorPaint(new SKColor(241, 245, 249)) // Slate-100
            : new SolidColorPaint(new SKColor(15, 23, 42));   // Slate-900
    }

    public static SolidColorPaint GetSeparatorsPaint(AppTheme theme)
    {
        return theme == AppTheme.Dark
            ? new SolidColorPaint(new SKColor(51, 65, 85, 90))     // Dark Slate-700
            : new SolidColorPaint(new SKColor(226, 232, 240, 220)); // Light Slate-200
    }

    public static SolidColorPaint GetTargetBarPaint(AppTheme theme)
    {
        return theme == AppTheme.Dark
            ? new SolidColorPaint(new SKColor(71, 85, 105, 180))
            : new SolidColorPaint(new SKColor(203, 213, 225, 220));
    }

    public static SolidColorPaint GetLegendTextPaint(AppTheme theme)
    {
        return theme == AppTheme.Dark
            ? new SolidColorPaint(new SKColor(241, 245, 249)) // Dark mode: white text
            : new SolidColorPaint(new SKColor(15, 23, 42));   // Light mode: dark text
    }

    public static SolidColorPaint GetTooltipBgPaint(AppTheme theme)
    {
        return theme == AppTheme.Dark
            ? new SolidColorPaint(new SKColor(17, 27, 51, 240))
            : new SolidColorPaint(new SKColor(255, 255, 255, 245));
    }

    public static SolidColorPaint GetTooltipTextPaint(AppTheme theme)
    {
        return theme == AppTheme.Dark
            ? new SolidColorPaint(new SKColor(255, 255, 255))
            : new SolidColorPaint(new SKColor(15, 23, 42));
    }
}
