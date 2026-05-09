using Appoo.Services;

namespace Appoo.Views;

public partial class LoginPage : ContentPage
{
    private readonly IDataService _dataService;

    public LoginPage(IDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (await _dataService.LoginAsync(UsernameEntry.Text, PasswordEntry.Text))
        {
            await DisplayAlert("Success", "Logged in", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
            await DisplayAlert("Error", "Invalid credentials", "OK");
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}