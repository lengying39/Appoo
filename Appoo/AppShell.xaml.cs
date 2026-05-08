using Appoo.Views;

namespace Appoo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // 注册高德地图页面的导航路由
        Routing.RegisterRoute(nameof(GaodeMapPage), typeof(GaodeMapPage));
    }
}