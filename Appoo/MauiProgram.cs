using Appoo.Services;
using Appoo.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
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
        builder.Services.AddSingleton<DatabaseService>();                    // 数据库服务
        builder.Services.AddSingleton<IDataService, DataService>();         // 业务数据服务
        builder.Services.AddSingleton<IImageRecognitionService, LandmarkRecognitionService>(); // 图片识别

        // Pages (注意：这里应该全部使用 AddTransient，而不是 AddSingleton)
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
        builder.Services.AddTransient<TestimonialOptionsPage>();
        builder.Services.AddTransient<WriteReviewPage>();
        builder.Services.AddTransient<ViewReviewsPage>();
        builder.Services.AddTransient<MyReviewsPage>();

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
    }
}