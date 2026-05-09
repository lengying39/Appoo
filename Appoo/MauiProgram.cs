using Appoo.Services;
using Appoo.Views;
using Microsoft.Extensions.Logging;
namespace Appoo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
               .ConfigureFonts(fonts =>
               {
                   fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                   fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
               });

        // 识别服务：默认使用无法识别版本，拍照不崩溃
        builder.Services.AddSingleton<IImageRecognitionService, UnrecognizableImageService>();

        // 注册页面
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<GaodeMapPage>();
        builder.Services.AddTransient<CameraPage>();
        builder.Services.AddTransient<RecommendPage>();
        builder.Services.AddTransient<FoodPage>();
        builder.Services.AddTransient<SettingsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}