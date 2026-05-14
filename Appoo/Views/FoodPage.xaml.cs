using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class FoodPage : ContentPage, INotifyPropertyChanged
{
    private string _spotName;
    public string SpotName
    {
        get => _spotName;
        set { _spotName = value; LoadFoodItems(); }
    }

    private ObservableCollection<FoodItem> _foodItems = new();
    public ObservableCollection<FoodItem> FoodItems
    {
        get => _foodItems;
        set { _foodItems = value; OnPropertyChanged(); }
    }

    public FoodPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void LoadFoodItems()
    {
        FoodItems.Clear();

        // Get current spot data
        var dataService = App.Services.GetRequiredService<IDataService>();
        var allSpots = dataService.GetAllSpots();
        var currentSpot = allSpots.FirstOrDefault(s => s.Name == SpotName);

        if (currentSpot == null) return;

        // Load different food lists per attraction (5 each)
        switch (currentSpot.Name)
        {
            case "Giant Wild Goose Pagoda": // Dayanta
                FoodItems.Add(new FoodItem { Name = "Qin Sheng Xuan Home Cuisine", Description = "Authentic Shaanxi cuisine, highly recommended by locals", Price = "¥69/person", Dish = "Old Shaanxi Hulu Chicken" });
                FoodItems.Add(new FoodItem { Name = "Shi San Chao · Dayanta Impression", Description = "Best view of the pagoda", Price = "¥85/person", Dish = "Red Sleeves Laughing" });
                FoodItems.Add(new FoodItem { Name = "Da Cheng Zhi · Shang Shi Fang", Description = "Food hub at Tang Paradise", Price = "¥50/person", Dish = "Lamb Pao Mo" });
                FoodItems.Add(new FoodItem { Name = "Rain Flower Western Restaurant", Description = "Western dining with pagoda view", Price = "¥121/person", Dish = "Steak Buffet" });
                FoodItems.Add(new FoodItem { Name = "Chang'an Grand Brand Stall", Description = "Popular Shaanxi restaurant inside Dayue City", Price = "¥80/person", Dish = "Chang'an Hulu Chicken" });
                break;

            case "Bell Tower":
                FoodItems.Add(new FoodItem { Name = "Suan La Gu · Shaanxi Style Halal Shabu", Description = "Halal hot pot facing the Drum Tower", Price = "¥99/person", Dish = "Fresh sliced beef & lamb" });
                FoodItems.Add(new FoodItem { Name = "Zui Chang'an (Bell Tower Flagship)", Description = "Asia's 100 best Shaanxi restaurant", Price = "¥95/person", Dish = "Intangible Heritage Hulu Chicken" });
                FoodItems.Add(new FoodItem { Name = "Tong Sheng Xiang · Intangible Heritage (Bell Tower)", Description = "Century-old time-honored brand", Price = "¥52/person", Dish = "Lamb Pao Mo" });
                FoodItems.Add(new FoodItem { Name = "De Fa Chang Dumpling House", Description = "China time-honored dumpling banquet", Price = "¥65/person", Dish = "Dumpling Banquet" });
                FoodItems.Add(new FoodItem { Name = "Ben Pa Ba Shaanxi Cuisine", Description = "High value Shaanxi food", Price = "¥60/person", Dish = "Hulu Chicken" });
                break;

            case "Terra Cotta Warriors":
                FoodItems.Add(new FoodItem { Name = "Qin Feng Lao Pu", Description = "Authentic Pao Mo near the Terracotta Army", Price = "¥40/person", Dish = "Handmade Lamb Pao Mo" });
                FoodItems.Add(new FoodItem { Name = "Wei Jia Liang Pi (Terracotta Army Branch)", Description = "Fast local snack", Price = "¥15/person", Dish = "Secret Liangpi" });
                FoodItems.Add(new FoodItem { Name = "Hou Gong Yuan Lin Restaurant", Description = "Garden style restaurant", Price = "¥70/person", Dish = "Shaanxi Cuisine" });
                FoodItems.Add(new FoodItem { Name = "Cheng Cheng Chong Bin Shui Pen Yang Rou", Description = "Authentic Lintong Shui Pen Lamb", Price = "¥40/person", Dish = "Shui Pen Lamb" });
                FoodItems.Add(new FoodItem { Name = "Tang Wu Da Pan Ji", Description = "Lintong old shop big plate chicken", Price = "¥50/person", Dish = "Xinjiang Big Plate Chicken" });
                break;

            case "Huaqing Palace":
                FoodItems.Add(new FoodItem { Name = "Yu Shan Yuan", Description = "Palace Shaanxi cuisine inside Huaqing Hot Spring Hotel", Price = "¥200/person", Dish = "Palace Snack Banquet" });
                FoodItems.Add(new FoodItem { Name = "Qin Restaurant", Description = "Immersive Qin Dynasty set meal experience", Price = "¥180/person", Dish = "Qin Theme Set" });
                FoodItems.Add(new FoodItem { Name = "Xi'an Xinjiang Big Plate Chicken (Lintong Branch)", Description = "Years-old Xinjiang cuisine shop", Price = "¥28/person", Dish = "Xinjiang Big Plate Chicken" });
                FoodItems.Add(new FoodItem { Name = "Ke Lai Zhou Dao (Lintong Branch)", Description = "Specialty 'Bubble Oil Cake'", Price = "¥30/person", Dish = "Bubble Oil Cake, Jujube Paste" });
                FoodItems.Add(new FoodItem { Name = "Huaqing Eros Palace Yu Shan Ge", Description = "Chinese restaurant inside hot spring hotel", Price = "¥93/person", Dish = "Signature Spicy Fried Abalone" });
                break;

            case "Shaanxi History Museum":
                FoodItems.Add(new FoodItem { Name = "Qing Zhen Gang Gang Roast Meat", Description = "30-year-old 1-yuan BBQ shop", Price = "¥50/person", Dish = "1 yuan skewer" });
                FoodItems.Add(new FoodItem { Name = "Bai Nian Shou Zhua Yang Rou", Description = "Walkable from the museum", Price = "¥100/person", Dish = "Intangible heritage hand-held lamb" });
                FoodItems.Add(new FoodItem { Name = "Da Chu Xiao Guan (Xiaozhai Museum Branch)", Description = "Guanzhong folk Shaanxi cuisine", Price = "¥55/person", Dish = "Shaanxi Cuisine" });
                FoodItems.Add(new FoodItem { Name = "Qin Jiu Wei", Description = "Shaanxi intangible heritage cuisine", Price = "¥40/person", Dish = "Zichang Pancake" });
                FoodItems.Add(new FoodItem { Name = "Chang'an Grand Brand Stall (Shaanxi History Museum Cultural Restaurant)", Description = "Tang style cultural restaurant", Price = "¥80/person", Dish = "Hulu Chicken" });
                break;

            case "Tang Paradise (Datang Furong Garden)":
                FoodItems.Add(new FoodItem { Name = "Yin Tang Chinese Cuisine", Description = "Opposite the main gate of Tang Paradise", Price = "¥120/person", Dish = "Intangible heritage Hulu Head Pao Mo" });
                FoodItems.Add(new FoodItem { Name = "Ma La Shang Xi", Description = "Garden hot pot inside Yu Yan Palace", Price = "¥111/person", Dish = "Thousand layer tripe, ice jelly" });
                FoodItems.Add(new FoodItem { Name = "Fu Tao Museum · Tang Yu Yin", Description = "Courtyard restaurant inside a museum", Price = "¥150/person", Dish = "Qin pepper dried beef" });
                FoodItems.Add(new FoodItem { Name = "Zi Wu Lu Zhang Ji Rou Jia Mo", Description = "West side of Tang Paradise", Price = "¥16/person", Dish = "Rou Jia Mo" });
                FoodItems.Add(new FoodItem { Name = "Yan Ran Ju", Description = "Near South Gate of Tang Paradise", Price = "¥70/person", Dish = "Shaanxi Cuisine" });
                break;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
public class FoodItem
{
    public string Name { get; set; }        // 店名
    public string Description { get; set; } // 一句话描述或招牌菜
    public string Price { get; set; }       // 人均或特色菜价格
    public string Dish { get; set; }        // 主推菜品
    public string ImageFileName { get; set; }
}