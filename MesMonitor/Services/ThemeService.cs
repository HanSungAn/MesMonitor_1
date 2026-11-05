using System;
using System.Windows;

namespace MesMonitor.Services;

public enum AppTheme
{
    Dark,
    Light
}

public interface IThemeService
{
    AppTheme CurrentTheme { get; }
    event Action<AppTheme>? ThemeChanged;
    void SetTheme(AppTheme theme);
    void ToggleTheme();
}

public class ThemeService : IThemeService
{
    public AppTheme CurrentTheme { get; private set; } = AppTheme.Dark;

    public event Action<AppTheme>? ThemeChanged;

    public void ToggleTheme()
    {
        SetTheme(CurrentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark);
    }

    public void SetTheme(AppTheme theme)
    {
        CurrentTheme = theme;
        string themeUri = theme == AppTheme.Dark
            ? "Themes/DarkTheme.xaml"
            : "Themes/LightTheme.xaml";

        var newDict = new ResourceDictionary
        {
            Source = new Uri(themeUri, UriKind.RelativeOrAbsolute)
        };

        var appResources = Application.Current.Resources;
        var merged = appResources.MergedDictionaries;

        // Find existing theme dictionary if any
        ResourceDictionary? existingTheme = null;
        foreach (var dict in merged)
        {
            if (dict.Source != null && (dict.Source.OriginalString.Contains("DarkTheme") || dict.Source.OriginalString.Contains("LightTheme")))
            {
                existingTheme = dict;
                break;
            }
        }

        if (existingTheme != null)
        {
            merged.Remove(existingTheme);
        }

        merged.Insert(0, newDict);

        ThemeChanged?.Invoke(CurrentTheme);
    }
}
