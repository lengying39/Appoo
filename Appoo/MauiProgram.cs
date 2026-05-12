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

        // Services
        builder.Services.AddSingleton<IDataService, DataService>();
        builder.Services.AddSingleton<IImageRecognitionService, UnrecognizableImageService>();

        // Pages
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

#if ANDROID
        WebViewHandler.Mapper.AppendToMapping("EnableGeolocation", (handler, view) =>
        {
            handler.PlatformView.Settings.SetGeolocationEnabled(true);
            handler.PlatformView.Settings.JavaScriptEnabled = true;
            handler.PlatformView.SetWebChromeClient(
                new MyWebChromeClient(handler.PlatformView.Context as Android.App.Activity ?? throw new InvalidOperationException("Activity is null")));
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var app = builder.Build();
        App.Services = app.Services;
        return app;

        return builder.Build();
    }
}