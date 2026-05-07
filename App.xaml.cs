using Microsoft.Maui.Controls;
using Assignment.Pages;

namespace Assignment;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new MainPage());
    }
}