using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;

namespace Appoo.Views;

public partial class MapPage : ContentPage
{
    public MapPage()
    {
        InitializeComponent();
        TryLoadMap();
    }

    private async void TryLoadMap()
    {
        try
        {
            // 尝试初始化地图，若密钥无效会在此抛出异常
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Location(34.2583, 108.9427), Distance.FromKilometers(10)));
            AddPins();
        }
        catch (Exception ex)
        {
            // 地图加载失败时显示友好提示，不让App闪退
            MyMap.IsVisible = false;
            var label = new Label
            {
                Text = "🗺️ 地图暂时不可用\n请检查 Google Maps API 密钥是否正确配置",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 18,
                TextColor = Colors.Gray,
                HorizontalTextAlignment = TextAlignment.Center
            };
            // 假设你的页面根布局是 Grid，将提示添加到 Grid 中
            if (Content is Grid grid)
                grid.Children.Add(label);

            // 可选：输出错误到调试窗口
            System.Diagnostics.Debug.WriteLine($"地图加载失败: {ex.Message}");
        }
    }

    private void AddPins()
    {
        MyMap.Pins.Add(new Pin
        {
            Label = "大雁塔",
            Address = "雁塔区",
            Location = new Location(34.2136, 108.9594)
        });
        // ... 其他Pin同理，保留你原来的代码
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
                MyMap.Pins.Add(new Pin
                {
                    Label = "我的位置",
                    Location = new Location(location.Latitude, location.Longitude)
                });
            }
            else
            {
                await DisplayAlertAsync("定位失败", "无法获取当前位置", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("定位错误", ex.Message, "确定");
        }
    }
}