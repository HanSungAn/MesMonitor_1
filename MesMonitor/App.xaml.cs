using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MesMonitor.ViewModels;
using AnimatedGif;

namespace MesMonitor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        File.AppendAllText(@"d:\workspace\MES Sample\debug.log", $"Args count: {e.Args.Length}, Args: {string.Join(", ", e.Args)}\n");

        RenderOptions.ProcessRenderMode = RenderMode.Default;

        var win = new MainWindow();
        win.Show();

        if (e.Args.Length > 0 && (e.Args[0] == "--capture-screenshots" || e.Args[0] == "--capture-all"))
        {
            string outputDir = e.Args.Length > 1 ? e.Args[1] : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            Directory.CreateDirectory(outputDir);

            // Wait for MainWindow to load and initial data to arrive
            var tcs = new TaskCompletionSource();
            if (win.IsLoaded) tcs.SetResult();
            else win.Loaded += (s, args) => tcs.TrySetResult();
            await tcs.Task;

            if (win.DataContext is not MainViewModel vm) return;

            // Wait for charts and simulator to populate
            await Task.Delay(2500);

            var tabs = new[]
            {
                ("Dashboard", "dashboard.png"),
                ("Equipment", "equipment.png"),
                ("Quality", "quality.png"),
                ("WorkOrder", "workorder.png"),
                ("History", "history.png")
            };

            var gifFrames = new List<string>();

            // 1. Capture each main view
            foreach (var (tab, fileName) in tabs)
            {
                vm.NavigateCommand.Execute(tab);
                await Task.Delay(2000);
                win.UpdateLayout();

                string filePath = Path.Combine(outputDir, fileName);
                SaveWindowToPng(win, filePath);
                gifFrames.Add(filePath);
            }

            // 2. Capture a live update on dashboard & theme toggle for the GIF
            vm.NavigateCommand.Execute("Dashboard");
            await Task.Delay(1500);
            win.UpdateLayout();
            string dash2 = Path.Combine(outputDir, "dashboard_live.png");
            SaveWindowToPng(win, dash2);
            gifFrames.Add(dash2);

            // Toggle Light Theme
            vm.ToggleThemeCommand.Execute(null);
            await Task.Delay(1500);
            win.UpdateLayout();
            string lightModePath = Path.Combine(outputDir, "dashboard_light.png");
            SaveWindowToPng(win, lightModePath);
            gifFrames.Add(lightModePath);

            // Toggle back to Dark Theme
            vm.ToggleThemeCommand.Execute(null);
            await Task.Delay(1000);

            // 3. Create Animated GIF
            string gifPath = Path.Combine(outputDir, "MesMonitor_Demo.gif");
            using (var creator = AnimatedGif.AnimatedGif.Create(gifPath, 1800, 0)) // 1.8s per frame, infinite loop
            {
                foreach (var framePath in gifFrames)
                {
                    creator.AddFrame(framePath, delay: 1800, quality: GifQuality.Bit8);
                }
            }

            Shutdown(0);
        }
    }

    private static void SaveWindowToPng(Window window, string filePath)
    {
        int width = (int)window.ActualWidth;
        int height = (int)window.ActualHeight;
        if (width <= 0) width = (int)window.Width;
        if (height <= 0) height = (int)window.Height;

        var rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
        rtb.Render(window);

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(rtb));
        using var fs = File.Create(filePath);
        encoder.Save(fs);
    }
}
