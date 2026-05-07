using Appoo.Services;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace Appoo.Views;

public partial class CameraPage : ContentPage
{
    private readonly IImageRecognitionService _recognitionService;

    public CameraPage(IImageRecognitionService recognitionService)
    {
        InitializeComponent();
        _recognitionService = recognitionService;
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlert("提示", "拍照功能不可用", "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;

            // 显示图片预览
            CapturedImage.Source = ImageSource.FromFile(photo.FullPath);

            // 调用识别服务（当前注册的是 UnrecognizableImageService）
            ResultLabel.Text = "识别中...";
            string result = await _recognitionService.RecognizeAsync(photo.FullPath);
            ResultLabel.Text = $"识别结果: {result}";
        }
        catch (PermissionException)
        {
            await DisplayAlert("权限不足", "请授予相机和存储权限", "确定");
        }
        catch (Exception ex)
        {
            await DisplayAlert("错误", ex.Message, "OK");
        }
    }
}