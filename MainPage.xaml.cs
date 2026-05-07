using Microsoft.Maui.Controls;
using Assignment.Pages;

namespace Assignment;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void GoToListPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListPage());
    }

    private async void GoToFavoritePage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoritePage());
    }
}