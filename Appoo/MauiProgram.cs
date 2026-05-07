using Appoo.Services;
using Appoo.Views;
using Appoo;
using Appoo.Views;
using Microsoft.Extensions.Logging;
using Appoo.Views;
using Appoo.Services;

namespace Appoo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()       // 启用地图控件
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 注册识别服务（你可以在这里切换为 RecognizableImageService 或 UnrecognizableImageService）
        builder.Services.AddSingleton<IImageRecognitionService, UnrecognizableImageService>();

        // 注册页面
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<MapPage>();
        builder.Services.AddTransient<CameraPage>();
        builder.Services.AddTransient<RecommendPage>();
        builder.Services.AddTransient<SettingsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}