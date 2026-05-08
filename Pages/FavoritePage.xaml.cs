using Microsoft.Maui.Controls;

namespace Assignment.Pages;

public partial class FavoritePage : ContentPage
{
    public FavoritePage()
    {
        InitializeComponent();
        LoadFavorites();
    }

    private void LoadFavorites()
    {
        var favorites = FavoriteManager.GetFavorites();
        FavoriteContainer.Children.Clear();

        if (favorites.Count == 0)
        {
            FavoriteContainer.Children.Add(new Label
            {
                Text = "You have no favorites yet.",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 30, 0, 0)
            });
            return;
        }

        foreach (var title in favorites)
        {
            var frame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 15,
                Padding = 16,
                Content = new Label
                {
                    Text = title,
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold
                }
            };
            FavoriteContainer.Children.Add(frame);
        }
    }
}