namespace App00.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await DisplayAlert("关于", "旅游指南 v1.0\n提供GPS定位、拍照识景等功能。", "OK");
    }
}