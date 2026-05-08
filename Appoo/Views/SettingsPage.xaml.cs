using Microsoft.Maui.Controls;

namespace Appoo.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("关于", "旅行指南 v1.0\n一款集地图、拍照识景、景点推荐于一体的旅游助手。", "OK");
    }
}