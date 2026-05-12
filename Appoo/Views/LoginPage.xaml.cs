using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

public partial class LoginPage : ContentPage
{
    private readonly IDataService _dataService;

    public LoginPage()
    {
        InitializeComponent();
        _dataService = App.Services.GetRequiredService<IDataService>();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (await _dataService.LoginAsync(UsernameEntry.Text, PasswordEntry.Text))
        {
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Invalid username or password", "OK");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    private async void OnSkipClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}