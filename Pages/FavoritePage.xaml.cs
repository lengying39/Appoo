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
            var layout = new HorizontalStackLayout
            {
                Spacing = 12,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            var label = new Label
            {
                Text = title,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            var deleteBtn = new Button
            {
                Text = "🗑️",
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Gray,
                FontSize = 20,
                WidthRequest = 40,
                HeightRequest = 40,
                CornerRadius = 20,
                Padding = 0
            };

            deleteBtn.CommandParameter = title;
            deleteBtn.Clicked += OnDeleteSingleFavorite;

            layout.Children.Add(label);
            layout.Children.Add(deleteBtn);

            var frame = new Frame
            {
                BackgroundColor = Colors.White,
                CornerRadius = 15,
                Padding = new Thickness(20, 16),
                Content = layout
            };

            FavoriteContainer.Children.Add(frame);
        }
    }

    private async void OnDeleteSingleFavorite(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string title)
        {
            bool confirm = await DisplayAlert("Delete Favorite", $"Do you want to remove \"{title}\" from favorites?", "Yes", "No");
            if (confirm)
            {
                FavoriteManager.RemoveFavorite(title);
                LoadFavorites();
            }
        }
    }

    private async void ClearAllFavorites_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Clear All", "Are you sure you want to clear all favorites?", "Yes", "No");
        if (confirm)
        {
            FavoriteManager.ClearAllFavorites();
            LoadFavorites();
            await DisplayAlert("Success", "All favorites have been cleared.", "OK");
        }
    }
}