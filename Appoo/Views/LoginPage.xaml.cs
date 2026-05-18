using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

public partial class LoginPage : ContentPage
{
    private readonly IDataService _dataService;

    private void OnTogglePasswordVisibility(object sender, EventArgs e)
    {
        // 切换密码可见性
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        
        var btn = sender as Button;
        if (btn != null)
        {
            btn.Text = PasswordEntry.IsPassword ? "👀" : "\U0001fae3";
        }
    }

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