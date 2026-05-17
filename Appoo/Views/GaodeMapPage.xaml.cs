#if ANDROID
using Appoo.Platforms.Android;
#endif
using Microsoft.Maui.Handlers;
using System.Diagnostics;

namespace Appoo.Views;

public partial class GaodeMapPage : ContentPage
{
    public GaodeMapPage()
    {
        InitializeComponent();
        LoadMapWithBaseUrl();
        mapView.Navigated += OnWebViewNavigated;
    }

    private void LoadMapWithBaseUrl()
    {
        // 使用 GitHub Pages 的 HTTPS URL（请替换为你的实际地址）
        var url = "https://lengying39.github.io/aamap/amap.html";
        mapView.Source = url;
        Debug.WriteLine($"Loading map from: {url}");
    }

    private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
    {
        Debug.WriteLine($"WebView navigation completed: {e.Result}");
        ConfigureWebView();
    }

    private void ConfigureWebView()
    {
#if ANDROID
        if (mapView.Handler?.PlatformView is Android.Webkit.WebView androidWebView)
        {
            androidWebView.Settings.JavaScriptEnabled = true;
            androidWebView.Settings.SetGeolocationEnabled(true);
            androidWebView.Settings.DomStorageEnabled = true;
            var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            if (activity != null)
            {
                androidWebView.SetWebChromeClient(new MyWebChromeClient(activity));
                Debug.WriteLine("WebView location configuration applied.");
            }
        }
#endif
    }
}