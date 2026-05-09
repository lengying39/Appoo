using Appoo.Services;
using Appoo.Models;

namespace Appoo.Views;

public partial class RegisterPage : ContentPage
{
    private readonly IDataService _dataService;
    public RegisterPage(IDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
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
            await Shell.Current.GoToAsync("..");
        }
        else
            await DisplayAlert("Error", "Username already exists", "OK");
    }
}