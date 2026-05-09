namespace Appoo.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage() => InitializeComponent();

    private async void OnSwitchRecog(object sender, EventArgs e)
    {
        await DisplayAlert("Switch Recognition",
            "In MauiProgram.cs, replace UnrecognizableImageService with RecognizableImageService and configure Azure keys.",
            "OK");
    }

    private async void OnClearCache(object sender, EventArgs e)
    {
        await DisplayAlert("Cache", "Cache cleared (simulated).", "OK");
    }

    private async void OnAbout(object sender, EventArgs e)
    {
        await DisplayAlert("About", "Travel Guide v1.0\nIntegrated map, photo recognition, food recommendation.", "OK");
    }
}