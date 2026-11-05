using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace MesMonitor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 프로세스 전체의 렌더링 모드를 GPU 하드웨어 가속(Default)으로 명시적 활성화
        RenderOptions.ProcessRenderMode = RenderMode.Default;
    }
}

