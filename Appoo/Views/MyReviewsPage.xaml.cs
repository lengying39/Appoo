using Appoo.Models;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

public partial class MyReviewsPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly IDataService _dataService;

    public MyReviewsPage()
    {
        InitializeComponent();
        _dbService = App.Services.GetRequiredService<DatabaseService>();
        _dataService = App.Services.GetRequiredService<IDataService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var user = _dataService.CurrentUser;
        if (user == null)
        {
            await DisplayAlert("Tip", "Please login First!", "OK");
            await Shell.Current.GoToAsync("..");
            return;
        }
        var reviews = await _dbService.GetReviewsByUsernameAsync(user.Username);
        LoadReviews(reviews);
    }

    private void LoadReviews(List<UserReview> reviews)
    {
        ReviewsStack.Children.Clear();
        if (reviews == null || reviews.Count == 0)
        {
            ReviewsStack.Children.Add(new Label
            {
                Text = "You haven't written a review yet, go write one～",
                FontSize = 16,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 50, 0, 0)
            });
            return;
        }

        foreach (var review in reviews)
        {
            var frame = new Frame
            {
                BackgroundColor = Color.FromArgb("#80FFFFFF"),
                CornerRadius = 15,
                Padding = 10,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var layout = new VerticalStackLayout { Spacing = 8 };

            layout.Children.Add(new Label
            {
                Text = $"景点：{review.SpotName}",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            });

            layout.Children.Add(new Label
            {
                Text = review.Comment,
                FontSize = 14,
                LineBreakMode = LineBreakMode.WordWrap
            });

            if (!string.IsNullOrEmpty(review.ImagePath) && File.Exists(review.ImagePath))
            {
                layout.Children.Add(new Image
                {
                    Source = ImageSource.FromFile(review.ImagePath),
                    HeightRequest = 120,
                    Aspect = Aspect.AspectFill,
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }

            layout.Children.Add(new Label
            {
                Text = review.DatePosted.ToString("yyyy-MM-dd HH:mm"),
                FontSize = 10,
                TextColor = Colors.Gray
            });

            frame.Content = layout;
            ReviewsStack.Children.Add(frame);
        }
    }
}