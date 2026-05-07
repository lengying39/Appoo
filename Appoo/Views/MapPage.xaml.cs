using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;

namespace App00.Views;

public partial class MapPage : ContentPage
{
    public MapPage()
    {
        InitializeComponent();
        // 初始位置：西安
        MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(34.2583, 108.9427), Distance.FromKilometers(10)));
        AddPredefinedPins();
    }

    private void AddPredefinedPins()
    {
        MyMap.Pins.Add(new Pin
        {
            Label = "大雁塔",
            Address = "西安市雁塔区",
            Location = new Location(34.2136, 108.9594)
        });
        MyMap.Pins.Add(new Pin
        {
            Label = "钟楼",
            Address = "西安市中心",
            Location = new Location(34.2583, 108.9427)
        });
        MyMap.Pins.Add(new Pin
        {
            Label = "兵马俑",
            Address = "西安市临潼区",
            Location = new Location(34.3849, 109.2731)
        });
    }

    private async void OnLocateClicked(object sender, EventArgs e)
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
                location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
            if (location != null)
            {
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location(location.Latitude, location.Longitude), Distance.FromKilometers(5)));
                // 添加用户当前位置的标记
                MyMap.Pins.Add(new Pin
                {
                    Label = "我的位置",
                    Location = new Location(location.Latitude, location.Longitude)
                });
            }
            else
            {
                await DisplayAlert("提示", "无法获取位置", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("错误", ex.Message, "OK");
        }
    }
}