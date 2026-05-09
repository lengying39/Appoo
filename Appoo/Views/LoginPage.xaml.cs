using Appoo.Services;
using System.Windows.Input;

namespace Appoo.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly IDataService _dataService;
        public string loginText = "Login";
        public string UsernamePlaceholder => "Username";
        public string PasswordPlaceholder => "Password";
        public string LoginButton => "Login";
        public string RegisterButton => "Register";
        public string Username { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public LoginPage(IDataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            LoginCommand = new Command(async () => await Login());
            GoToRegisterCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(RegisterPage)));
            BindingContext = this;
        }

        private async Task Login()
        {
            if (await _dataService.LoginAsync(Username, Password))
                await Shell.Current.GoToAsync("//home");
            else
                await DisplayAlertAsync("Error", "Invalid username or password", "OK");
        }
    }
}