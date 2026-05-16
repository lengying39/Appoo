using Appoo.Models;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class AttractionDetailPage : ContentPage
{
    private string? _spotName;
    public string? SpotName
    {
        get => _spotName;
        set { _spotName = value; LoadSpot(); }
    }

    private readonly IDataService _dataService;
    private TouristSpot? _spot;

    public AttractionDetailPage()
    {
        InitializeComponent();
        _dataService = App.Services.GetRequiredService<IDataService>();
    }

    private void LoadSpot()
    {
        if (string.IsNullOrEmpty(SpotName)) return;
        var allSpots = _dataService.GetAllSpots();
        _spot = allSpots.FirstOrDefault(s => s.Name == SpotName);
        if (_spot != null)
        {
            NameLabel.Text = _spot.Name;
            DescLabel.Text = GetDetailedDescription(_spot.Name);
            OpenLabel.Text = $"Opening Hours: {_spot.OpenTime}";
            LocationLabel.Text = $"Location: {_spot.Location}";
            UpdateFavoriteButton();
        }
    }

    private void UpdateFavoriteButton()
    {
        var favorites = _dataService.GetFavorites();
        bool isFav = favorites.Contains(SpotName);
        FavoriteButton.Text = isFav ? "❤️ Favorited" : "💗   Add to MyFavorite";
    }

    private async void OnFavoriteButtonClicked(object sender, EventArgs e)
    {
        if (_dataService.CurrentUser == null)
        {
            await DisplayAlert("Info", "Please login first", "OK");
            return;
        }
        var favorites = _dataService.GetFavorites();
        if (favorites.Contains(SpotName))
        {
            _dataService.RemoveFavorite(SpotName);
            await DisplayAlert("Info", "Removed from favorites", "OK");
        }
        else
        {
            _dataService.AddFavorite(SpotName);
            await DisplayAlert("Info", "Added to favorites", "OK");
        }
        UpdateFavoriteButton();
    }

    // 详细英文描述（仅在此页面使用，不影响推荐页面的简短描述）
    private string GetDetailedDescription(string spotName)
    {
        return spotName switch
        {
            "Giant Wild Goose Pagoda" =>
                "Built in 652 AD during the Tang Dynasty, the Giant Wild Goose Pagoda was originally used to store Buddhist scriptures brought from India by the famous monk Xuanzang. Standing at 64 meters tall, this brick pagoda is a masterpiece of ancient Chinese architecture. Visitors can climb to the top for a panoramic view of Xi'an. The surrounding square features musical fountains and traditional Tang Dynasty performances, making it a cultural hub for both history buffs and casual tourists.",

            "Bell Tower" =>
                "Located at the exact center of Xi'an's ancient city walls, the Bell Tower was built in 1384 during the Ming Dynasty. It once served as the city's timekeeper, with bells ringing at dawn to mark the start of the day. The tower's wooden structure, intricate carvings, and painted beams showcase traditional Chinese craftsmanship. Today, it is a museum where you can see ancient bells and enjoy panoramic views of the bustling city center, including the nearby Drum Tower and Muslim Quarter.",

            "Terra Cotta Warriors" =>
                "Discovered in 1974 by local farmers, the Terracotta Army is one of the greatest archaeological finds of the 20th century. Buried with China's first emperor, Qin Shi Huang, over 8,000 life-sized soldiers, chariots, and horses guard his tomb. Each figure has unique facial features, hairstyles, and armor, reflecting the incredible skill of ancient artisans. The site includes three main pits and a museum displaying bronze chariots and weapons. A visit here offers a profound glimpse into imperial China's power and artistry.",

            "Huaqing Palace" =>
                "Nestled at the foot of Mount Li, Huaqing Palace has been a royal hot spring resort since the Zhou Dynasty (over 3,000 years ago). It gained fame as the romantic retreat of Emperor Xuanzong and his beloved concubine Yang Guifei during the Tang Dynasty. The complex features ancient bathhouses, a fragrant lotus pool, and modern hot spring pools. Visitors can enjoy the natural thermal waters while exploring the scenic gardens, pavilions, and the site of the famous Xi'an Incident that changed modern Chinese history.",

            "Shaanxi History Museum" =>
                "Often called the 'Pearl of the Han and Tang Dynasties', the Shaanxi History Museum houses over 370,000 relics spanning more than a million years. Highlights include prehistoric tools, Zhou Dynasty bronzes, Qin terracotta warriors, Tang gold and silverware, and exquisite murals from imperial tombs. The museum's architecture blends traditional Tang style with modern design. With comprehensive exhibits and bilingual descriptions, it's the perfect place to understand the rich history of Xi'an and ancient China.",

            "Tang Paradise (Datang Furong Garden)" =>
                "Tang Paradise is a large-scale cultural theme park re-creating the splendor of the Tang Dynasty's imperial gardens. Covering 165 acres, the park features magnificent palaces, pavilions, and a lake with nightly light and music shows. Visitors can watch traditional dance performances, attend a Tang-style wedding ceremony, and try on period costumes. The park's landmark is the Ziyun Tower, where a spectacular fountain show tells love stories of the Tang court. It's a living museum that brings ancient Chinese poetry and art to life.",

            _ => _spot?.Description ?? "A famous attraction in Xi'an, full of history and culture."
        };
    }
}