using Microsoft.Maui.Controls;

namespace Assignment.Pages;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    private async void GoToTerracottaArmy(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("Terracotta Army"));
    }

    private async void GoToGiantWildGoosePagoda(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("Giant Wild Goose Pagoda"));
    }

    private async void GoToXianCityWall(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("Xi'an City Wall"));
    }
}