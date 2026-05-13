using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

public partial class ProfilePage : ContentPage
{
    private readonly IDataService _dataService;

    public ProfilePage()
    {
        InitializeComponent();
        _dataService = App.Services.GetRequiredService<IDataService>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateUI();
    }

    private void UpdateUI()
    {
        var user = _dataService.CurrentUser;
        if (user != null)
        {
            UsernameLabel.Text = user.Nickname ?? user.Username;
            AuthButton.Text = "Logout";
            AuthButton.BackgroundColor = (Color)Application.Current.Resources["WineRed"];
        }
        else
        {
            UsernameLabel.Text = "Not logged in";
            AuthButton.Text = "Login";
            AuthButton.BackgroundColor = Colors.Gray;
            ProfileImage.Source = "default_avatar.png";
        }
    }

    private async void OnAvatarTapped(object sender, EventArgs e)
    {
        if (_dataService.CurrentUser == null)
        {
            await DisplayAlert("Tip", "Please login to change avatar.", "OK");
            return;
        }

        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select Avatar",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                ProfileImage.Source = ImageSource.FromFile(result.FullPath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to open gallery: " + ex.Message, "OK");
        }
    }

    private async void OnMyReviewsClicked(object sender, EventArgs e)
    {
        if (_dataService.CurrentUser == null)
        {
            await DisplayAlert("提示", "请先登录", "OK");
            return;
        }
        await Shell.Current.GoToAsync(nameof(MyReviewsPage));
    }

    private async void OnMyFavoritesClicked(object sender, EventArgs e)
    {
        if (_dataService.CurrentUser == null)
        {
            await DisplayAlert("Sorry", "Please login to access My Favorites.", "OK");
            return;
        }
        await Shell.Current.GoToAsync(nameof(MyFavoritesPage));
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    private async void OnAuthButtonClicked(object sender, EventArgs e)
    {
        if (_dataService.CurrentUser != null)
        {
            _dataService.Logout();   // 关键修改
        }
        else
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        UpdateUI();
    }
}