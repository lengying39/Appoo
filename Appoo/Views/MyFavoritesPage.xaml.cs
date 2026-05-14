using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

public partial class MyFavoritesPage : ContentPage
{
    private readonly IDataService _dataService;

    public MyFavoritesPage(IDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadFavorites();
    }

    private void LoadFavorites()
    {
        // 检查是否登录
        if (_dataService.CurrentUser == null)
        {
            NoFavsLabel.Text = "Please login to see your favorites.";
            NoFavsLabel.IsVisible = true;
            FavoritesStack.Children.Clear();
            return;
        }

        var favs = _dataService.GetFavorites();
        FavoritesStack.Children.Clear();
        if (favs.Count == 0)
        {
            NoFavsLabel.Text = "No favorites yet.";
            NoFavsLabel.IsVisible = true;
        }
        else
        {
            NoFavsLabel.IsVisible = false;
            foreach (var name in favs)
            {
                FavoritesStack.Children.Add(new Label
                {
                    Text = name,
                    FontSize = 16,
                    Margin = new Thickness(5, 2)
                });
            }
        }
    }

    private async void OnClearAll(object sender, EventArgs e)
    {
        if (_dataService.CurrentUser == null)
        {
            await DisplayAlert("Info", "Please login first.", "OK");
            return;
        }
        _dataService.ClearFavorites();
        LoadFavorites();
        await DisplayAlert("Info", "All favorites cleared.", "OK");
    }

}