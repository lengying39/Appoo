using Appoo.Platforms.Android;
using Microsoft.Maui.Handlers;

namespace Appoo.Views;

public partial class GaodeMapPage : ContentPage
{
    public GaodeMapPage()
    {
        InitializeComponent();
        LoadMapWithBaseUrl();
        mapView.Loaded += OnWebViewLoaded;
    }

    private void LoadMapWithBaseUrl()
    {
        var htmlContent = "";
        using (var stream = FileSystem.OpenAppPackageFileAsync("wwwroot/amap.html").GetAwaiter().GetResult())
        using (var reader = new StreamReader(stream))
        {
            htmlContent = reader.ReadToEnd();
        }

        var baseUrl = "wwwroot";
        var htmlSource = new HtmlWebViewSource
        {
            Html = htmlContent,
            BaseUrl = baseUrl
        };
        mapView.Source = htmlSource;
    }

    private void OnWebViewLoaded(object sender, EventArgs e)
    {
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
            }
        }
#endif
    }
}