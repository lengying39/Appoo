using Microsoft.Maui.Controls.PlatformConfiguration;
using System.IO;
using Android.OS;  // 如果使用 Android.OS.Environment，仅在 Android 平台有效。建议使用跨平台方式。

namespace Appoo.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage() => InitializeComponent();

    private async void OnSwitchRecog(object sender, EventArgs e)
    {
        await DisplayAlert("Switch Recognition",
            "In MauiProgram.cs, replace UnrecognizableImageService with RecognizableImageService and configure Azure keys.",
            "OK");
    }

    private async void OnClearCache(object sender, EventArgs e)
    {
        await DisplayAlert("Cache", "Cache cleared (simulated).", "OK");
    }

    private async void OnAbout(object sender, EventArgs e)
    {
        await DisplayAlert("About", "Travel Guide v1.0\nIntegrated map, photo recognition, food recommendation.", "OK");
    }

    private async void OnExportDatabaseClicked(object sender, EventArgs e)
    {
        try
        {
            // 源文件：应用私有目录中的数据库
            var source = Path.Combine(FileSystem.AppDataDirectory, "travelapp.db3");
            if (!File.Exists(source))
            {
                await DisplayAlert("错误", "数据库文件不存在，请先登录并发表一些数据。", "OK");
                return;
            }

            // 目标文件夹：Download（需要 WRITE_EXTERNAL_STORAGE 权限，但 Android 10+ 可以通过 MediaStore 或直接路径尝试）
            var downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)?.AbsolutePath;
            if (string.IsNullOrEmpty(downloadsPath))
            {
                await DisplayAlert("错误", "无法获取下载文件夹路径。", "OK");
                return;
            }
            var dest = Path.Combine(downloadsPath, "travelapp.db3");

            // 复制文件（覆盖已存在）
            File.Copy(source, dest, true);

            await DisplayAlert("成功", $"数据库已导出到：{dest}\n\n你可以通过文件管理器或 adb pull 获取。", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("导出失败", ex.Message, "OK");
        }
    }
}