using Appoo.Models;
using Appoo.Services;
using Microsoft.Maui.Controls.Shapes;

namespace Appoo.Views;

public partial class RecommendPage : ContentPage
{
    private readonly IDataService _dataService;
    private List<TouristSpot> spots;

    public RecommendPage(IDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        spots = _dataService.GetAllSpots();
        _ = LoadSpotsAsync();
    }

    private async Task LoadSpotsAsync()
    {
        try
        {
            // 检查容器是否存在
            if (SpotList == null)
            {
                await DisplayAlert("错误", "SpotList 控件未在 XAML 中定义", "OK");
                return;
            }

            // 清空旧内容
            SpotList.Children.Clear();

            if (spots == null || spots.Count == 0)
            {
                await DisplayAlert("提示", "没有景点数据", "OK");
                return;
            }

            foreach (var spot in spots)
            {
                // 外层卡片
                var card = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = 15 },
                    BackgroundColor = Color.FromRgba(255, 255, 255, 180),
                    Stroke = Colors.White,
                    StrokeThickness = 2,
                    Padding = new Thickness(15),
                    ClassId = spot.Name
                };
                card.Shadow = new Shadow { Brush = Colors.Black, Offset = new Point(2, 4), Radius = 6, Opacity = 0.2f };

                var stack = new VerticalStackLayout { Spacing = 10 };

                // 景点图片
                var image = new Image
                {
                    Source = spot.ImageFile,
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 180
                };

                // 名称
                var nameLabel = new Label
                {
                    Text = spot.Name,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = (Color)Application.Current.Resources["DarkBlack"]
                };

                // 描述
                var descLabel = new Label
                {
                    Text = spot.Description,
                    FontSize = 16,
                    TextColor = (Color)Application.Current.Resources["DarkBlack"]
                };

                // 附近美食按钮
                var foodBtn = new Button
                {
                    Text = "🍜 Nearby Food",
                    BackgroundColor = (Color)Application.Current.Resources["WineRed"],
                    TextColor = Colors.White,
                    CornerRadius = 8,
                    FontAttributes = FontAttributes.Bold
                };
                foodBtn.Clicked += async (s, e) =>
                {
                    await Shell.Current.GoToAsync($"{nameof(FoodPage)}?spotName={spot.Name}");
                };

                // 详情按钮
                var detailBtn = new Button
                {
                    Text = "📄 Details",
                    BackgroundColor = (Color)Application.Current.Resources["DarkBlack"],
                    TextColor = Colors.White,
                    CornerRadius = 8,
                    FontAttributes = FontAttributes.Bold
                };
                detailBtn.Clicked += async (s, e) =>
                {
                    await Shell.Current.GoToAsync($"{nameof(AttractionDetailPage)}?spotName={spot.Name}");
                };

                // 评价按钮
                var testimonialBtn = new Button
                {
                    Text = "📝 Reviews",
                    BackgroundColor = (Color)Application.Current.Resources["WineRed"],
                    TextColor = Colors.White,
                    CornerRadius = 8,
                    FontAttributes = FontAttributes.Bold
                };
                var spotNameForReview = spot.Name;
                testimonialBtn.Clicked += async (s, e) =>
                {
                    await Shell.Current.GoToAsync($"{nameof(TestimonialOptionsPage)}?spotName={spotNameForReview}");
                };

                // 按顺序添加
                stack.Children.Add(image);
                stack.Children.Add(nameLabel);
                stack.Children.Add(descLabel);
                stack.Children.Add(foodBtn);
                stack.Children.Add(detailBtn);
                stack.Children.Add(testimonialBtn);

                card.Content = stack;

                // 卡片加载动画
                card.Loaded += async (s, e) =>
                {
                    card.Scale = 0.95;
                    await card.TranslateToAsync(0, -10, 500, Easing.CubicOut);
                    await card.ScaleToAsync(1.0, 500, Easing.CubicOut);
                };

                SpotList.Children.Add(card);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("加载失败", ex.ToString(), "OK");
        }
    }
}