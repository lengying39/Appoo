using Appoo.Views;

namespace Appoo;

public partial class App : Application
{
    public static IServiceProvider Services { get; set; } = null!;   // 全局服务容器

    public App()
    {
        InitializeComponent();
        // 启动时显示登录页，并用 NavigationPage 包裹以便导航到注册页
        MainPage = new NavigationPage(new LoginPage());
    }
}