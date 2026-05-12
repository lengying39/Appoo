using Appoo.Services;
using Appoo.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

public partial class RegisterPage : ContentPage
{
    private readonly IDataService _dataService;

    public RegisterPage()
    {
        InitializeComponent();
        _dataService = App.Services.GetRequiredService<IDataService>();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var user = new User
        {
            Username = UsernameEntry.Text,
            Nickname = NicknameEntry.Text,
            Password = PasswordEntry.Text
        };
        if (await _dataService.RegisterAsync(user))
        {
            await DisplayAlert("Success", "Registration successful", "OK");
            // 返回登录页（当前在 NavigationPage 中）
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Username already exists", "OK");
        }
    }
}