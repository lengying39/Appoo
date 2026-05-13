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
        SpotNameLabel.Text = $"景点：{SpotName}";
    }

    private async void OnAttachPhoto(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "选择一张照片",
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
            await DisplayAlert("错误", "无法选择图片：" + ex.Message, "OK");
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ReviewEditor.Text))
        {
            await DisplayAlert("提示", "请填写评价内容", "OK");
            return;
        }

        var currentUser = _dataService.CurrentUser;
        if (currentUser == null)
        {
            await DisplayAlert("提示", "请先登录", "OK");
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
        await DisplayAlert("发布成功", "你的评价已提交", "OK");
        await Shell.Current.GoToAsync(".."); // 返回上一页
    }
}