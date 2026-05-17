using Appoo.Models;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Shapes;

namespace Appoo.Views;

public partial class MyFavoritesPage : ContentPage
{
    private readonly IDataService _dataService;
    private readonly DatabaseService _dbService;   // 用于获取景点图片，也可以从 DataService 获取景点列表

    public MyFavoritesPage(IDataService dataService, DatabaseService dbService)
    {
        InitializeComponent();
        _dataService = dataService;
        _dbService = dbService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFavoritesAsync();
    }

    private async Task LoadFavoritesAsync()
    {
        FavoritesStack.Children.Clear();

        if (_dataService.CurrentUser == null)
        {
            NoFavsLabel.Text = "Please login to see your favorites.";
            NoFavsLabel.IsVisible = true;
            return;
        }

        var allFavs = _dataService.GetAllFavorites();
        if (allFavs == null || allFavs.Count == 0)
        {
            NoFavsLabel.Text = "No favorites yet.";
            NoFavsLabel.IsVisible = true;
            return;
        }

        NoFavsLabel.IsVisible = false;

        // 获取所有景点（用于景点图片和名称）
        var allSpots = _dataService.GetAllSpots();

        // 美食图片映射（请根据你的实际图片文件名补充完整）
        var foodImageMap = new Dictionary<string, string>
        {
            // 大雁塔
            { "Qin Sheng Xuan Home Cuisine", "dyt1.jpg" },
            { "Shi San Chao · Dayanta Impression", "dyt2.jpg" },
            { "Da Cheng Zhi · Shang Shi Fang", "dyt3.jpg" },
            { "Rain Flower Western Restaurant", "dyt4.jpg" },
            { "Chang'an Grand Brand Stall", "dyt5.jpg" },
            // 钟楼
            { "Suan La Gu · Shaanxi Style Halal Shabu", "zl1.jpg" },
            { "Zui Chang'an (Bell Tower Flagship)", "zl2.jpg" },
            { "Tong Sheng Xiang · Intangible Heritage (Bell Tower)", "zl3.jpg" },
            { "De Fa Chang Dumpling House", "zl4.jpg" },
            { "Ben Pa Ba Shaanxi Cuisine", "zl5.jpg" },
            // 兵马俑
            { "Qin Feng Lao Pu", "bmy1.jpg" },
            { "Wei Jia Liang Pi (Terracotta Army Branch)", "bmy2.jpg" },
            { "Hou Gong Yuan Lin Restaurant", "bmy3.jpg" },
            { "Cheng Cheng Chong Bin Shui Pen Yang Rou", "bmy4.jpg" },
            { "Tang Wu Da Pan Ji", "bmy5.jpg" },
            // 华清宫
            { "Yu Shan Yuan", "hqg1.jpg" },
            { "Qin Restaurant", "hqg2.jpg" },
            { "Xi'an Xinjiang Big Plate Chicken (Lintong Branch)", "hqg3.jpg" },
            { "Ke Lai Zhou Dao (Lintong Branch)", "hqg4.jpg" },
            { "Huaqing Eros Palace Yu Shan Ge", "hqg5.jpg" },
            // 陕博
            { "Qing Zhen Gang Gang Roast Meat", "shanbo1.jpg" },
            { "Bai Nian Shou Zhua Yang Rou", "shanbo2.jpg" },
            { "Da Chu Xiao Guan (Xiaozhai Museum Branch)", "shanbo3.jpg" },
            { "Qin Jiu Wei", "shanbo4.jpg" },
            { "Chang'an Grand Brand Stall (Shaanxi History Museum Cultural Restaurant)", "shanbo5.jpg" },
            // 大唐芙蓉园
            { "Yin Tang Chinese Cuisine", "dtfry1.jpg" },
            { "Ma La Shang Xi", "dtfry2.jpg" },
            { "Fu Tao Museum · Tang Yu Yin", "dtfry3.jpg" },
            { "Zi Wu Lu Zhang Ji Rou Jia Mo", "dtfry4.jpg" },
            { "Yan Ran Ju", "dtfry5.jpg" }
        };

        foreach (var fav in allFavs)
        {
            if (fav.StartsWith("Spot:"))
            {
                var spotName = fav.Substring(5);
                var spot = allSpots.FirstOrDefault(s => s.Name == spotName);
                if (spot != null)
                {
                    var card = CreateCard(spot.Name, spot.ImageFile, "Spot", spotName);
                    FavoritesStack.Children.Add(card);
                }
            }
            else if (fav.StartsWith("Food:"))
            {
                var foodName = fav.Substring(5);
                var imageFile = foodImageMap.ContainsKey(foodName) ? foodImageMap[foodName] : "default_food.png";
                var card = CreateCard(foodName, imageFile, "Food", foodName);
                FavoritesStack.Children.Add(card);
            }
        }
    }

    private Border CreateCard(string title, string imageSource, string type, string id)
    {
        var image = new Image
        {
            Source = imageSource,
            HeightRequest = 80,
            WidthRequest = 80,
            Aspect = Aspect.AspectFill,
            Margin = new Thickness(5)
        };

        var nameLabel = new Label
        {
            Text = title,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = (Color)Application.Current.Resources["DarkBlack"],
            VerticalOptions = LayoutOptions.Center
        };

        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(90) },
                new ColumnDefinition { Width = GridLength.Star }
            },
            Padding = new Thickness(10),
            BackgroundColor = Color.FromRgba(255, 255, 255, 180),
            Margin = new Thickness(0, 0, 0, 10)
        };

        grid.Children.Add(image);
        Grid.SetColumn(image, 0);
        grid.Children.Add(nameLabel);
        Grid.SetColumn(nameLabel, 1);

        // 添加点击手势：景点跳转详情，美食提示（可自行扩展跳转）
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += async (s, e) =>
        {
            if (type == "Spot")
                await Shell.Current.GoToAsync($"{nameof(AttractionDetailPage)}?spotName={id}");
            else
                await DisplayAlert("Info", $"\"{title}\" is a favorite food.", "OK");
        };
        grid.GestureRecognizers.Add(tapGesture);

        var border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = 15 },
            Stroke = Colors.Transparent,
            Content = grid,
            Padding = 0
        };
        border.Shadow = new Shadow { Brush = Colors.Black, Offset = new Point(2, 2), Radius = 5, Opacity = 0.2f };
        return border;
    }

    private async void OnClearAll(object sender, EventArgs e)
    {
        if (_dataService.CurrentUser == null)
        {
            await DisplayAlert("Info", "Please login first.", "OK");
            return;
        }
        _dataService.ClearFavorites();   // 注意：这个方法清空所有收藏（包括景点和美食）
        await LoadFavoritesAsync();
        await DisplayAlert("Info", "All favorites cleared.", "OK");
    }
}