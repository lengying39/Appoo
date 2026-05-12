using System;
using System.IO;
using Appoo.Services;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace Appoo.Views;

public partial class CameraPage : ContentPage
{
    private readonly IImageRecognitionService _recognition;
    private readonly IDataService _dataService;

    public CameraPage(IImageRecognitionService recognition, IDataService dataService)
    {
        InitializeComponent();
        _recognition = recognition;
        _dataService = dataService;
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

            await ProcessImage(photo.FullPath);
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

    private async void OnPickFromAlbumClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a photo",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                await ProcessImage(result.FullPath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task ProcessImage(string filePath)
    {
        // 读取图片字节
        byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
        System.Diagnostics.Debug.WriteLine($"[Camera] Image size: {imageBytes.Length} bytes");

        PreviewFrame.IsVisible = true;
        CapturedImage.Source = ImageSource.FromFile(filePath);

        ResultFrame.IsVisible = true;
        ResultLabel.Text = "⏳ Recognizing...";

        // 调用识别服务
        string rawResult = await _recognition.RecognizeAsync(imageBytes);

        if (rawResult.StartsWith("[Error]"))
        {
            ResultLabel.Text = rawResult;
            return;
        }

        string landmarkName = rawResult;

        // 与本地景点数据库对照
        var matchedSpot = _dataService.RecognizeAndMatchSpot(landmarkName);

        if (matchedSpot != null)
        {
            ResultLabel.Text = $"🏷️ {matchedSpot.Name}\n" +
                               $"📝 {matchedSpot.Description}\n" +
                               $"🕒 Open: {matchedSpot.OpenTime}\n" +
                               $"📍 {matchedSpot.Location}";
        }
        else if (!string.IsNullOrWhiteSpace(landmarkName))
        {
            ResultLabel.Text = $"🏷️ {landmarkName}\n(Not yet in your local travel guide)";
        }
        else
        {
            ResultLabel.Text = "Unable to recognize this landmark.";
        }
    }
}