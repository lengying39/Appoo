using Appoo.Models;
namespace Appoo.Views;

public partial class RecommendPage : ContentPage
{
    private List<TouristSpot> spots = new()
    {
        new TouristSpot { Name="大雁塔", Description="唐代古塔，大慈恩寺内。", ImageFile="dyt.png",
            NearbyFood = new List<string>{ "肉夹馍", "凉皮", "羊肉泡馍" } },
        new TouristSpot { Name="钟楼", Description="西安市中心地标。", ImageFile="spot_zhonglou.jpg",
            NearbyFood = new List<string>{ "德发长饺子", "回民街烤肉", "酸梅汤" } },
        new TouristSpot { Name="兵马俑", Description="世界第八大奇迹。", ImageFile="bmy.png",
            NearbyFood = new List<string>{ "临潼石榴", "柿子饼", "梆梆肉" } },
        new TouristSpot { Name="华清宫", Description="唐代皇家温泉行宫。", ImageFile="spot_huaqinggong.jpg",
            NearbyFood = new List<string>{ "温泉蛋", "御膳点心", "石榴汁" } }
    };

    public RecommendPage()
    {
        InitializeComponent();
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
                BackgroundColor = (Color)Application.Current.Resources["PureWhite"],
                Stroke = Colors.Transparent,
                Padding = new Thickness(15),
                ClassId = spot.Name
            };
            card.Shadow = new Shadow { Brush = Colors.Black, Offset = new Point(2, 4), Radius = 6, Opacity = 0.2f };

            var stack = new VerticalStackLayout { Spacing = 10 };
            var image = new Image
            {
                Source = spot.ImageFile,
                Aspect = Aspect.AspectFill,
                HeightRequest = 180
            };
            var labelName = new Label
            {
                Text = spot.Name,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = (Color)Application.Current.Resources["DarkBlack"]
            };
            var descLabel = new Label
            {
                Text = spot.Description,
                FontSize = 14,
                TextColor = (Color)Application.Current.Resources["GrayText"]
            };
            var foodBtn = new Button
            {
                Text = "🍜 附近美食",
                BackgroundColor = (Color)Application.Current.Resources["WineRed"],
                TextColor = Colors.White,
                CornerRadius = 8,
                FontAttributes = FontAttributes.Bold
            };
            foodBtn.Clicked += async (s, e) =>
            {
                await Shell.Current.GoToAsync($"{nameof(FoodPage)}?spotName={spot.Name}");
            };

            stack.Children.Add(image);
            stack.Children.Add(labelName);
            stack.Children.Add(descLabel);
            stack.Children.Add(foodBtn);
            card.Content = stack;

            // 悬浮动画：加载后执行轻微上浮
            card.Loaded += (s, e) =>
            {
                card.Scale = 0.95;
                card.TranslateTo(0, -10, 500, Easing.CubicOut);
                card.ScaleTo(1.0, 500, Easing.CubicOut);
            };

            SpotList.Children.Add(card);
        }
    }
}