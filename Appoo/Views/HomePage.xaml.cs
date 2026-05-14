using Appoo.Models;
using Appoo.Services;
using Microsoft.Maui.Devices.Sensors;

namespace Appoo.Views;

public partial class HomePage : ContentPage
{
    private readonly IDataService _dataService;
    private List<TouristSpot> spots = new()
    {
        new TouristSpot { Name = "Dayan Pagoda", Latitude = 34.2136, Longitude = 108.9594 },
        new TouristSpot { Name = "The Xi'an Circumvallation", Latitude = 34.2583, Longitude = 108.9427 },
        new TouristSpot { Name = "Emperor Qinshihuang's Mausoleum", Latitude = 34.3849, Longitude = 109.2731 }
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

    // ★ 实时搜索过滤
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ResultsStack.Children.Clear();
        var keyword = e.NewTextValue?.Trim().ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(keyword))
        {
            ResultsContainer.IsVisible = false;
            return;
        }

        // 搜索景点（可点击跳转）
        var matchedSpots = _dataService.GetAllSpots()
            .Where(s => s.Name.ToLower().Contains(keyword));
        foreach (var spot in matchedSpots)
        {
            var label = new Label
            {
                Text = spot.Name,
                FontSize = 16,
                TextColor = (Color)Application.Current.Resources["DarkBlack"],
                Margin = new Thickness(5, 2)
            };
            // 添加点击手势
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                await Shell.Current.GoToAsync($"{nameof(AttractionDetailPage)}?spotName={spot.Name}");
            };
            label.GestureRecognizers.Add(tapGesture);
            ResultsStack.Children.Add(label);
        }

        // 搜索美食（暂时不可点击，仅展示）
        var matchedFoods = _dataService.GetAllFoods()
            .Where(f => f.ToLower().Contains(keyword));
        foreach (var food in matchedFoods)
        {
            ResultsStack.Children.Add(new Label
            {
                Text = food,
                FontSize = 16,
                TextColor = (Color)Application.Current.Resources["GrayText"],
                Margin = new Thickness(5, 2)
            });
        }

        // 搜索设施（暂时不可点击）
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
            });
        }

        // 有结果就显示容器，无结果隐藏
        ResultsContainer.IsVisible = ResultsStack.Children.Count > 0;
    }
}