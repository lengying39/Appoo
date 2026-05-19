using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Appoo.Models;
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
    private readonly DatabaseService _dbService;

    public FoodPage()
    {
        InitializeComponent();
        _dataService = App.Services.GetRequiredService<IDataService>();
        _dbService = App.Services.GetRequiredService<DatabaseService>();
        BindingContext = this;
    }

    private async void LoadFoodItems()
    {
        FoodItems.Clear();
        if (string.IsNullOrEmpty(SpotName)) return;

        // 从数据库获取美食列表
        var foods = await _dbService.GetFoodItemsBySpotNameAsync(SpotName);
        if (foods == null || foods.Count == 0) return;

        // 获取当前用户已收藏的美食名称（带 "Food:" 前缀）
        var favoriteFoods = (_dataService.GetAllFavorites() ?? new List<string>())
            .Where(f => f.StartsWith("Food:"))
            .Select(f => f.Substring(5))
            .ToList();

        foreach (var food in foods)
        {
            var vm = new FoodItemViewModel
            {
                Name = food.Name,
                Description = food.Description,
                Price = food.Price,
                ImageFileName = food.ImageFileName,
                IsFavorite = favoriteFoods.Contains(food.Name),
                ToggleFavoriteCommand = new Command(() => ToggleFavorite(food.Name))
            };
            FoodItems.Add(vm);
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