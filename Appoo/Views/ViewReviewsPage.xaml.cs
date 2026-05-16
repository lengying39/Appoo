using Appoo.Models;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class ViewReviewsPage : ContentPage
{
    public string SpotName { get; set; }
    private readonly DatabaseService _dbService;
    private List<UserReview> _allReviews = new();

    public ViewReviewsPage()
    {
        InitializeComponent();
        _dbService = App.Services.GetRequiredService<DatabaseService>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _allReviews = await _dbService.GetReviewsBySpotNameAsync(SpotName);
        LoadReviews(_allReviews);
    }

    private void LoadReviews(List<UserReview> reviews)
    {
        ReviewsStack.Children.Clear();
        if (reviews == null || reviews.Count == 0)
        {
            ReviewsStack.Children.Add(new Label
            {
                Text = "No reviews yet. Be the first to add one~",
                FontSize = 16,
                TextColor = Colors.Black,
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
                Text = $"User: {review.Username}",
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

    // Real-time filtering when search text changes
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue?.Trim();
        if (string.IsNullOrWhiteSpace(keyword))
        {
            LoadReviews(_allReviews);
        }
        else
        {
            var filtered = _allReviews.Where(r =>
                r.Comment.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                r.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();
            LoadReviews(filtered);
        }
    }
}