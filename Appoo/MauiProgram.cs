using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Appoo.Services;
using Appoo.Views;
#if ANDROID
using Appoo.Platforms.Android;
#endif

namespace Appoo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 数据服务（单例，全局共享用户信息和收藏）
        builder.Services.AddSingleton<IDataService, DataService>();

        // 注册所有页面（原有 + 新增）
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<GaodeMapPage>();
        builder.Services.AddTransient<CameraPage>();
        builder.Services.AddTransient<RecommendPage>();
        builder.Services.AddTransient<FoodPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<AttractionDetailPage>();
        builder.Services.AddTransient<MyFavoritesPage>();
        builder.Services.AddTransient<PublicFacilitiesPage>();
        builder.Services.AddTransient<SearchPage>();

        // Android 平台：开启 WebView 地理定位
#if ANDROID
        WebViewHandler.Mapper.AppendToMapping("EnableGeolocation", (handler, view) =>
        {
            handler.PlatformView.Settings.SetGeolocationEnabled(true);
            handler.PlatformView.Settings.JavaScriptEnabled = true;
            handler.PlatformView.SetWebChromeClient(
                new MyWebChromeClient(handler.PlatformView.Context as Android.App.Activity));
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}