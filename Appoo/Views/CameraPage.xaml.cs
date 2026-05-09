using Appoo.Services;
using Microsoft.Maui.Media;
namespace Appoo.Views;

public partial class CameraPage : ContentPage
{
    private readonly IImageRecognitionService _recognition;
    public CameraPage(IImageRecognitionService recognition)
    {
        InitializeComponent();
        _recognition = recognition;
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlert("错误", "此设备不支持拍照", "OK");
                return;
            }
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;

            PreviewFrame.IsVisible = true;
            CapturedImage.Source = ImageSource.FromFile(photo.FullPath);

            ResultFrame.IsVisible = true;
            ResultLabel.Text = "⏳ 识别中...";
            var result = await _recognition.RecognizeAsync(photo.FullPath);
            ResultLabel.Text = $"🏷️ {result}";
        }
        catch (PermissionException)
        {
            await DisplayAlert("权限不足", "请授予相机和存储权限", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("错误", ex.Message, "OK");
        }
    }
}