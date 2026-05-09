using Appoo.Services;

namespace Appoo.Views;

public partial class MyFavoritesPage : ContentPage
{
    private readonly IDataService _dataService;
    public MyFavoritesPage(IDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        LoadFavorites();
    }

    private void LoadFavorites()
    {
        var favs = _dataService.GetFavorites();
        FavoritesStack.Children.Clear();
        if (favs.Count == 0) NoFavsLabel.IsVisible = true;
        else
        {
            NoFavsLabel.IsVisible = false;
            foreach (var name in favs)
                FavoritesStack.Children.Add(new Label { Text = name, FontSize = 16 });
        }
    }

    private void OnClearAll(object sender, EventArgs e)
    {
        _dataService.ClearFavorites();
        LoadFavorites();
    }
}