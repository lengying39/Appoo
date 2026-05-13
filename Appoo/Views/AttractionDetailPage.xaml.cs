using Appoo.Models;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class AttractionDetailPage : ContentPage
{
    public string SpotName { get; set; }
    private readonly IDataService _dataService;
    private TouristSpot _spot;

    public AttractionDetailPage()
    {
        InitializeComponent();
        _dataService = App.Services.GetRequiredService<IDataService>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var allSpots = _dataService.GetAllSpots();
        _spot = allSpots.FirstOrDefault(s => s.Name == SpotName);
        if (_spot != null)
        {
            NameLabel.Text = _spot.Name;
            DescLabel.Text = _spot.Description;
            OpenLabel.Text = $"开放时间：{_spot.OpenTime}";
            LocationLabel.Text = $"位置：{_spot.Location}";
            UpdateFavoriteButton();
        }
    }

    private void UpdateFavoriteButton()
    {
        var favorites = _dataService.GetFavorites();
        bool isFav = favorites.Contains(SpotName);
        FavoriteButton.Text = isFav ? "❤️ 已收藏" : "🤍 收藏";
    }

    private async void OnFavoriteButtonClicked(object sender, EventArgs e)
    {
        if (_dataService.CurrentUser == null)
        {
            await DisplayAlert("提示", "请先登录", "OK");
            return;
        }
        var favorites = _dataService.GetFavorites();
        if (favorites.Contains(SpotName))
        {
            _dataService.RemoveFavorite(SpotName);
            await DisplayAlert("提示", "已取消收藏", "OK");
        }
        else
        {
            _dataService.AddFavorite(SpotName);
            await DisplayAlert("提示", "已添加到收藏", "OK");
        }
        UpdateFavoriteButton();
    }
}