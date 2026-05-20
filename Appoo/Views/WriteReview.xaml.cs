using Android.OS;                          // 必须引入，否则 Android.OS.Environment 不可用
using Appoo.Models;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class WriteReviewPage : ContentPage
{
    public string SpotName { get; set; }
    private string _selectedImagePath = null;
    private readonly DatabaseService _dbService;
    private readonly IDataService _dataService;

    public WriteReviewPage()
    {
        InitializeComponent();
        _dbService = App.Services.GetRequiredService<DatabaseService>();
        _dataService = App.Services.GetRequiredService<IDataService>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SpotNameLabel.Text = $"Spot：{SpotName}";
    }

    private async void OnAttachPhoto(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Choose one Picture",
                FileTypes = FilePickerFileType.Images
            });
            if (result != null)
            {
                _selectedImagePath = result.FullPath;
                SelectedImage.Source = ImageSource.FromFile(_selectedImagePath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Wrong", "Unable to select image：" + ex.Message, "OK");
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ReviewEditor.Text))
        {
            await DisplayAlert("Tippp!", "Please fill in the review content", "OK");
            return;
        }

        var currentUser = _dataService.CurrentUser;
        if (currentUser == null)
        {
            await DisplayAlert("Tippp!", "Please log in first to use this feature", "OK");
            return;
        }

        string savedImagePath = null;
        if (ImageToggle.IsToggled && !string.IsNullOrEmpty(_selectedImagePath))
        {
            try
            {
                // 获取公共 Pictures 目录
                string picturesPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
                string appFolder = Path.Combine(picturesPath, "MyTravelGuide");
                if (!Directory.Exists(appFolder))
                    Directory.CreateDirectory(appFolder);

                var fileName = $"{Guid.NewGuid()}.jpg";
                savedImagePath = Path.Combine(appFolder, fileName);
                File.Copy(_selectedImagePath, savedImagePath, true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Save Failed", $"Unable to save image: {ex.Message}", "OK");
                return;
            }
        }

        var review = new UserReview
        {
            SpotName = SpotName,
            Username = currentUser.Username,
            Comment = ReviewEditor.Text,
            ImagePath = savedImagePath,
            DatePosted = DateTime.Now
        };

        await _dbService.AddReviewAsync(review);
        await DisplayAlert("Published successfully🥰", "Your review has been submitted!", "OK");
        await Shell.Current.GoToAsync("..");
    }
}