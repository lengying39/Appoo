using Appoo.Models;
using Appoo.Services;

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
        LoadSpots();
    }

    private void LoadSpots()
    {
        foreach (var spot in spots)
        {
            // 外层卡片
            var card = new Border
            {
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 15 },
                BackgroundColor = Color.FromRgba(255, 255, 255, 100), // 半透明白色
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
                FontSize = 14,
                TextColor = (Color)Application.Current.Resources["GrayText"]
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
                Text = "📄 Deatils",
                BackgroundColor = (Color)Application.Current.Resources["DarkBlack"],
                TextColor = Colors.White,
                CornerRadius = 8,
                FontAttributes = FontAttributes.Bold
            };
            detailBtn.Clicked += async (s, e) =>
            {
                await Shell.Current.GoToAsync($"{nameof(AttractionDetailPage)}?spotName={spot.Name}");
            };

            // 按顺序添加
            stack.Children.Add(image);
            stack.Children.Add(nameLabel);
            stack.Children.Add(descLabel);
            stack.Children.Add(foodBtn);
            stack.Children.Add(detailBtn);

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
}