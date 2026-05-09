using Appoo.Models;
using Microsoft.Maui.Devices.Sensors;

namespace Appoo.Views;

public partial class HomePage : ContentPage
{
    private List<TouristSpot> spots = new()
    {
        new TouristSpot { Name = "Dayan Pagoda", Latitude = 34.2136, Longitude = 108.9594 },
        new TouristSpot { Name = "The Xi'an Circumvallation", Latitude = 34.2583, Longitude = 108.9427 },
        new TouristSpot { Name = "Emperor Qinshihuang's Mausoleum", Latitude = 34.3849, Longitude = 109.2731 }
    };

    public HomePage() => InitializeComponent();

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
}