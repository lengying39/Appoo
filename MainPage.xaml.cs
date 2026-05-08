using Assignment.Pages;
using Microsoft.Maui.Controls;

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

    private async void GoToFoodPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FoodPage());
    }

    private async void GoToFacilitiesPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FacilitiesPage());
    }

    private async void GoToSearchPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage());
    }
}