using Appoo.Models;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class ViewReviewsPage : ContentPage
{
    public string SpotName { get; set; }
    private readonly DatabaseService _dbService;

    public ViewReviewsPage()
    {
        InitializeComponent();
        _dbService = App.Services.GetRequiredService<DatabaseService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var reviews = await _dbService.GetReviewsBySpotNameAsync(SpotName);
        LoadReviews(reviews);
    }

    private void LoadReviews(List<UserReview> reviews)
    {
        ReviewsStack.Children.Clear();
        if (reviews == null || reviews.Count == 0)
        {
            ReviewsStack.Children.Add(new Label
            {
                Text = "暂无评价，成为第一个评价的人吧～",
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
                Text = $"用户：{review.Username}",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#722F37")
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