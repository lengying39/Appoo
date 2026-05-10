using Appoo.Models;
namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class FoodPage : ContentPage
{
    public string SpotName { get; set; }
    private Dictionary<string, List<string>> foodData = new()
    {
        {"大雁塔", new List<string>{"肉夹馍", "凉皮", "羊肉泡馍"}},
        {"钟楼", new List<string>{"德发长饺子", "回民街烤肉", "酸梅汤"}},
        {"兵马俑", new List<string>{"临潼石榴", "柿子饼", "梆梆肉"}},
        {"华清宫", new List<string>{"温泉蛋", "御膳点心", "石榴汁"}}
    };

    public FoodPage() => InitializeComponent();

    protected override void OnAppearing()
    {
        base.OnAppearing();
        TitleLabel.Text = $"🍽️ {SpotName} Nearby Food";
        if (foodData.TryGetValue(SpotName, out var foods))
            foreach (var f in foods)
                FoodList.Children.Add(new Label
                {
                    Text = $"• {f}",
                    FontSize = 16,
                    TextColor = (Color)Application.Current.Resources["DarkBlack"]
                });
    }
}