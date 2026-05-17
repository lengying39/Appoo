using Appoo.Models;
using Appoo.Services;
using Microsoft.Maui.Devices.Sensors;

namespace Appoo.Views;

public partial class HomePage : ContentPage
{
    private readonly IDataService _dataService;
    // 用于定位的附近景点列表（保持不变）
    private List<TouristSpot> spots = new()
    {
        new TouristSpot { Name = "Giant Wild Goose Pagoda", Latitude = 34.2136, Longitude = 108.9594 },
        new TouristSpot { Name = "Bell Tower", Latitude = 34.2583, Longitude = 108.9427 },
        new TouristSpot { Name = "Terra Cotta Warriors", Latitude = 34.3849, Longitude = 109.2731 },
        new TouristSpot { Name = "Huaqing Palace", Latitude = 34.3812, Longitude = 109.2734 },
        new TouristSpot { Name = "Shaanxi History Museum", Latitude = 34.2152, Longitude = 108.9492 },
        new TouristSpot { Name = "Tang Paradise (Datang Furong Garden)", Latitude = 34.2144, Longitude = 108.9771 }
    };

    // 定义每个景点对应的餐厅列表（与 FoodPage 数据一致）
    private Dictionary<string, List<string>> spotFoods = new()
    {
        { "Giant Wild Goose Pagoda", new List<string> {
            "Qin Sheng Xuan Home Cuisine",
            "Shi San Chao · Dayanta Impression",
            "Da Cheng Zhi · Shang Shi Fang",
            "Rain Flower Western Restaurant",
            "Chang'an Grand Brand Stall"
        }},
        { "Bell Tower", new List<string> {
            "Suan La Gu · Shaanxi Style Halal Shabu",
            "Zui Chang'an (Bell Tower Flagship)",
            "Tong Sheng Xiang · Intangible Heritage (Bell Tower)",
            "De Fa Chang Dumpling House",
            "Ben Pa Ba Shaanxi Cuisine"
        }},
        { "Terra Cotta Warriors", new List<string> {
            "Qin Feng Lao Pu",
            "Wei Jia Liang Pi (Terracotta Army Branch)",
            "Hou Gong Yuan Lin Restaurant",
            "Cheng Cheng Chong Bin Shui Pen Yang Rou",
            "Tang Wu Da Pan Ji"
        }},
        { "Huaqing Palace", new List<string> {
            "Yu Shan Yuan",
            "Qin Restaurant",
            "Xi'an Xinjiang Big Plate Chicken (Lintong Branch)",
            "Ke Lai Zhou Dao (Lintong Branch)",
            "Huaqing Eros Palace Yu Shan Ge"
        }},
        { "Shaanxi History Museum", new List<string> {
            "Qing Zhen Gang Gang Roast Meat",
            "Bai Nian Shou Zhua Yang Rou",
            "Da Chu Xiao Guan (Xiaozhai Museum Branch)",
            "Qin Jiu Wei",
            "Chang'an Grand Brand Stall (Shaanxi History Museum Cultural Restaurant)"
        }},
        { "Tang Paradise (Datang Furong Garden)", new List<string> {
            "Yin Tang Chinese Cuisine",
            "Ma La Shang Xi",
            "Fu Tao Museum · Tang Yu Yin",
            "Zi Wu Lu Zhang Ji Rou Jia Mo",
            "Yan Ran Ju"
        }}
    };

    public HomePage(IDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
    }

    private async void OnGetLocationClicked(object sender, EventArgs e)
    {
        try
        {
            var loc = await Geolocation.GetLastKnownLocationAsync()
                      ?? await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
            if (loc != null)
            {
                LocationLabel.Text = $"Lat: {loc.Latitude:F4}, Lon: {loc.Longitude:F4}";
                var nearest = spots.OrderBy(s => Distance(loc.Latitude, loc.Longitude, s.Latitude, s.Longitude)).First();
                double dist = Distance(loc.Latitude, loc.Longitude, nearest.Latitude, nearest.Longitude);
                NearbyLabel.Text = $"🌟 {nearest.Name}, about {dist:0.0} km";
            }
            else LocationLabel.Text = "Unable to get location";
        }
        catch (Exception ex) { LocationLabel.Text = $"Error: {ex.Message}"; }
    }

    private double Distance(double lat1, double lon1, double lat2, double lon2)
    {
        double R = 6371, dLat = (lat2 - lat1) * Math.PI / 180, dLon = (lon2 - lon1) * Math.PI / 180;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    // 实时搜索过滤
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ResultsStack.Children.Clear();
        var keyword = e.NewTextValue?.Trim().ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(keyword))
        {
            ResultsContainer.IsVisible = false;
            return;
        }

        // 1. 搜索景点（可点击跳转）
        var matchedSpots = _dataService.GetAllSpots()
            .Where(s => s.Name.ToLower().Contains(keyword));
        foreach (var spot in matchedSpots)
        {
            var label = new Label
            {
                Text = spot.Name,
                FontSize = 16,
                TextColor = (Color)Application.Current.Resources["DarkBlack"],
                Margin = new Thickness(5, 2),
                TextDecorations = TextDecorations.Underline   // 下划线表示可点击
            };
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await Shell.Current.GoToAsync($"{nameof(AttractionDetailPage)}?spotName={spot.Name}");
            };
            label.GestureRecognizers.Add(tapGesture);
            ResultsStack.Children.Add(label);
        }

        // 2. 搜索美食（根据每个景点的餐厅列表，匹配餐厅名称）
        foreach (var kvp in spotFoods)
        {
            var spotName = kvp.Key;
            var foods = kvp.Value;
            var matchedFoods = foods.Where(f => f.ToLower().Contains(keyword)).ToList();
            foreach (var food in matchedFoods)
            {
                var label = new Label
                {
                    Text = food,
                    FontSize = 16,
                    TextColor = (Color)Application.Current.Resources["DarkBlack"],
                    Margin = new Thickness(5, 2),
                    TextDecorations = TextDecorations.Underline
                };
                // 点击跳转到该景点对应的美食页面
                var tapGesture = new TapGestureRecognizer();
                var spotNameCopy = spotName; // 捕获变量
                tapGesture.Tapped += async (s, e) =>
                {
                    await Shell.Current.GoToAsync($"{nameof(FoodPage)}?spotName={spotNameCopy}");
                };
                label.GestureRecognizers.Add(tapGesture);
                ResultsStack.Children.Add(label);
            }
        }

        // 3. 搜索设施（不可点击，仅展示）
        var matchedFacs = _dataService.GetAllFacilities()
            .Where(f => f.ToLower().Contains(keyword));
        foreach (var fac in matchedFacs)
        {
            ResultsStack.Children.Add(new Label
            {
                Text = fac,
                FontSize = 16,
                TextColor = (Color)Application.Current.Resources["GrayText"],
                Margin = new Thickness(5, 2)
                // 无下划线，不可点击
            });
        }

        // 有结果就显示容器，无结果隐藏
        ResultsContainer.IsVisible = ResultsStack.Children.Count > 0;
    }
}