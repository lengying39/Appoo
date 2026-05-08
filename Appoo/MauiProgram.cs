using Appoo.Services;
using Appoo.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Maps;

namespace Appoo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 注册识别服务：这里用不能识别的版本，体验完整流程后可换成 RecognizableImageService
        builder.Services.AddSingleton<IImageRecognitionService, UnrecognizableImageService>();

        // 注册所有页面
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