namespace Appoo.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage() => InitializeComponent();

    private async void OnSwitchRecog(object sender, EventArgs e)
    {
        await DisplayAlertAsync("切换识别服务", "请在 MauiProgram.cs 中将 UnrecognizableImageService 替换为 RecognizableImageService，并配置 Azure 密钥。", "确定");
    }
    private async void OnClearCache(object sender, EventArgs e)
    {
        await DisplayAlertAsync("缓存", "缓存已清除（模拟）。", "OK");
    }
    private async void OnAbout(object sender, EventArgs e)
    {
        await DisplayAlertAsync("关于", "旅行指南 v1.0\n集地图、拍照识景、美食推荐于一体。", "OK");
    }
}