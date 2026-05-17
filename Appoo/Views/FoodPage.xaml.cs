using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class FoodPage : ContentPage
{
    private string _spotName;
    public string SpotName
    {
        get => _spotName;
        set { _spotName = value; LoadFoodItems(); }
    }

    public ObservableCollection<FoodItemViewModel> FoodItems { get; } = new();

    private readonly IDataService _dataService;

    public FoodPage()
    {
        InitializeComponent();
        _dataService = App.Services.GetRequiredService<IDataService>();
        BindingContext = this;
    }

    private void LoadFoodItems()
    {
        FoodItems.Clear();
        if (string.IsNullOrEmpty(SpotName)) return;

        var dataService = App.Services.GetRequiredService<IDataService>();
        var allSpots = dataService.GetAllSpots();
        var currentSpot = allSpots.FirstOrDefault(s => s.Name == SpotName);
        if (currentSpot == null) return;

        // 获取当前用户已收藏的美食名称（带 "Food:" 前缀）
        var favoriteFoods = (_dataService.GetAllFavorites() ?? new List<string>())
            .Where(f => f.StartsWith("Food:"))
            .Select(f => f.Substring(5))
            .ToList();

        void AddItem(string name, string desc, string price, string imageFileName)
        {
            var vm = new FoodItemViewModel
            {
                Name = name,
                Description = desc,
                Price = price,
                ImageFileName = imageFileName,
                IsFavorite = favoriteFoods.Contains(name),
                ToggleFavoriteCommand = new Command(() => ToggleFavorite(name))
            };
            FoodItems.Add(vm);
        }

        switch (currentSpot.Name)
        {
            case "Giant Wild Goose Pagoda":
                AddItem("Qin Sheng Xuan Home Cuisine", "Authentic Shaanxi cuisine, highly recommended by locals", "¥69/person", "dyt1.jpg");
                AddItem("Shi San Chao · Dayanta Impression", "Best view of the pagoda", "¥85/person", "dyt2.jpg");
                AddItem("Da Cheng Zhi · Shang Shi Fang", "Food hub at Tang Paradise", "¥50/person", "dyt3.jpg");
                AddItem("Rain Flower Western Restaurant", "Western dining with pagoda view", "¥121/person", "dyt4.jpg");
                AddItem("Chang'an Grand Brand Stall", "Popular Shaanxi restaurant inside Dayue City", "¥80/person", "dyt5.jpg");
                break;

            case "Bell Tower":
                AddItem("Suan La Gu · Shaanxi Style Halal Shabu", "Halal hot pot facing the Drum Tower", "¥99/person", "zl1.jpg");
                AddItem("Zui Chang'an (Bell Tower Flagship)", "Asia's 100 best Shaanxi restaurant", "¥95/person", "zl2.jpg");
                AddItem("Tong Sheng Xiang · Intangible Heritage (Bell Tower)", "Century-old time-honored brand", "¥52/person", "zl3.jpg");
                AddItem("De Fa Chang Dumpling House", "China time-honored dumpling banquet", "¥65/person", "zl4.jpg");
                AddItem("Ben Pa Ba Shaanxi Cuisine", "High value Shaanxi food", "¥60/person", "zl5.jpg");
                break;

            case "Terra Cotta Warriors":
                AddItem("Qin Feng Lao Pu", "Authentic Pao Mo near the Terracotta Army", "¥40/person", "bmy1.jpg");
                AddItem("Wei Jia Liang Pi (Terracotta Army Branch)", "Fast local snack", "¥15/person", "bmy2.jpg");
                AddItem("Hou Gong Yuan Lin Restaurant", "Garden style restaurant", "¥70/person", "bmy3.jpg");
                AddItem("Cheng Cheng Chong Bin Shui Pen Yang Rou", "Authentic Lintong Shui Pen Lamb", "¥40/person", "bmy4.jpg");
                AddItem("Tang Wu Da Pan Ji", "Lintong old shop big plate chicken", "¥50/person", "bmy5.jpg");
                break;

            case "Huaqing Palace":
                AddItem("Yu Shan Yuan", "Palace Shaanxi cuisine inside Huaqing Hot Spring Hotel", "¥200/person", "hqg1.jpg");
                AddItem("Qin Restaurant", "Immersive Qin Dynasty set meal experience", "¥180/person", "hqg2.jpg");
                AddItem("Xi'an Xinjiang Big Plate Chicken (Lintong Branch)", "Years-old Xinjiang cuisine shop", "¥28/person", "hqg3.jpg");
                AddItem("Ke Lai Zhou Dao (Lintong Branch)", "Specialty 'Bubble Oil Cake'", "¥30/person", "hqg4.jpg");
                AddItem("Huaqing Eros Palace Yu Shan Ge", "Chinese restaurant inside hot spring hotel", "¥93/person", "hqg5.jpg");
                break;

            case "Shaanxi History Museum":
                AddItem("Qing Zhen Gang Gang Roast Meat", "30-year-old 1-yuan BBQ shop", "¥50/person", "shanbo1.jpg");
                AddItem("Bai Nian Shou Zhua Yang Rou", "Walkable from the museum", "¥100/person", "shanbo2.jpg");
                AddItem("Da Chu Xiao Guan (Xiaozhai Museum Branch)", "Guanzhong folk Shaanxi cuisine", "¥55/person", "shanbo3.jpg");
                AddItem("Qin Jiu Wei", "Shaanxi intangible heritage cuisine", "¥40/person", "shanbo4.jpg");
                AddItem("Chang'an Grand Brand Stall (Shaanxi History Museum Cultural Restaurant)", "Tang style cultural restaurant", "¥80/person", "shanbo5.jpg");
                break;

            case "Tang Paradise (Datang Furong Garden)":
                AddItem("Yin Tang Chinese Cuisine", "Opposite the main gate of Tang Paradise", "¥120/person", "dtfry1.jpg");
                AddItem("Ma La Shang Xi", "Garden hot pot inside Yu Yan Palace", "¥111/person", "dtfry2.jpg");
                AddItem("Fu Tao Museum · Tang Yu Yin", "Courtyard restaurant inside a museum", "¥150/person", "dtfry3.jpg");
                AddItem("Zi Wu Lu Zhang Ji Rou Jia Mo", "West side of Tang Paradise", "¥16/person", "dtfry4.jpg");
                AddItem("Yan Ran Ju", "Near South Gate of Tang Paradise", "¥70/person", "dtfry5.jpg");
                break;
        }
    }

    private async void ToggleFavorite(string foodName)
    {
        if (_dataService.CurrentUser == null)
        {
            await DisplayAlert("Info", "Please login to add favorites.", "OK");
            return;
        }

        var allFavs = _dataService.GetAllFavorites() ?? new List<string>();
        bool isFav = allFavs.Contains($"Food:{foodName}");

        if (isFav)
            _dataService.RemoveFavoriteFood(foodName);
        else
            _dataService.AddFavoriteFood(foodName);

        // 更新当前页面中对应 FoodItemViewModel 的 IsFavorite 状态
        var item = FoodItems.FirstOrDefault(i => i.Name == foodName);
        if (item != null)
            item.IsFavorite = !isFav;
    }
}

public class FoodItemViewModel : INotifyPropertyChanged
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Price { get; set; }
    public string ImageFileName { get; set; }

    private bool _isFavorite;
    public bool IsFavorite
    {
        get => _isFavorite;
        set
        {
            if (_isFavorite != value)
            {
                _isFavorite = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand ToggleFavoriteCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}