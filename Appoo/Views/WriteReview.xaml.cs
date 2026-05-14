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
            // 保存图片到应用本地目录
            var destDir = Path.Combine(FileSystem.AppDataDirectory, "review_images");
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            var fileName = $"{Guid.NewGuid()}.jpg";
            savedImagePath = Path.Combine(destDir, fileName);
            File.Copy(_selectedImagePath, savedImagePath);
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
        await Shell.Current.GoToAsync(".."); // 返回上一页
    }
}