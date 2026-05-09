namespace Appoo.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage() => InitializeComponent();

    private async void OnMyFavoritesClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(MyFavoritesPage));

    private async void OnPublicFacilitiesClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(PublicFacilitiesPage));

    private async void OnSearchClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(SearchPage));

    private async void OnSettingsClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(SettingsPage));

    private async void OnLogoutClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(LoginPage));
}