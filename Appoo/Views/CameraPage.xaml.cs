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
                await DisplayAlert("Error", "Capture not supported", "OK");
                return;
            }
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;
            PreviewFrame.IsVisible = true;
            CapturedImage.Source = ImageSource.FromFile(photo.FullPath);
            ResultFrame.IsVisible = true;
            ResultLabel.Text = "⏳ Recognizing...";
            var result = await _recognition.RecognizeAsync(photo.FullPath);
            ResultLabel.Text = $"🏷️ {result}";
        }
        catch (PermissionException)
        {
            await DisplayAlert("Permission Denied", "Please grant camera and storage permissions", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}