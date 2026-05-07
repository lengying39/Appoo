using Microsoft.Maui.Devices.Sensors;
using App00.Models;
using System.Collections.Generic;

namespace App00.Views;

public partial class HomePage : ContentPage
{
    // 预置几个西安景点作为附近示例
    private List<TouristSpot> nearbySpots = new()
    {
        new TouristSpot { Name = "大雁塔", Latitude = 34.2136, Longitude = 108.9594, Description = "唐代古塔" },
        new TouristSpot { Name = "钟楼", Latitude = 34.2583, Longitude = 108.9427, Description = "西安市中心" },
        new TouristSpot { Name = "兵马俑", Latitude = 34.3849, Longitude = 109.2731, Description = "世界八大奇迹" }
    };

    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnGetLocationClicked(object sender, EventArgs e)
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
            }

            if (location != null)
            {
                LocationLabel.Text = $"纬度: {location.Latitude:F4}, 经度: {location.Longitude:F4}";
                // 计算最近景点（简单直线距离演示）
                string nearest = "未找到附近景点";
                double minDist = double.MaxValue;
                foreach (var spot in nearbySpots)
                {
                    double d = Distance(location.Latitude, location.Longitude, spot.Latitude, spot.Longitude);
                    if (d < minDist)
                    {
                        minDist = d;
                        nearest = spot.Name;
                    }
                }
                NearbyLabel.Text = $"最近景点: {nearest}，距离约 {minDist:0.00} 公里";
            }
            else
            {
                LocationLabel.Text = "无法获取位置";
            }
        }
        catch (Exception ex)
        {
            LocationLabel.Text = $"错误: {ex.Message}";
        }
    }

    // 球面距离计算（Haversine公式，返回公里）
    private double Distance(double lat1, double lon1, double lat2, double lon2)
    {
        double R = 6371;
        double dLat = (lat2 - lat1) * Math.PI / 180;
        double dLon = (lon2 - lon1) * Math.PI / 180;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
}