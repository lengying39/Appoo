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
        // 从你的 DataService 获取美食列表（目前是 GetAllFoods 返回字符串列表）
        var dataService = App.Services.GetRequiredService<IDataService>();
        var allFoods = dataService.GetAllFoods();
        // 这里根据 SpotName 可以过滤关联美食，简单起见演示全部
        // 你可以根据景点不同展示不同美食（需修改 DataService 增加按景点获取美食的方法）
        foreach (var food in allFoods)
        {
            FoodItems.Add(new FoodItem
            {
                Name = food,
                Description = "特色美食，值得品尝",
                Price = "¥??"  // 价格留空
            });
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class FoodItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Price { get; set; }
}