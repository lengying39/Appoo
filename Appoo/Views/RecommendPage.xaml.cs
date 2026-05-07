namespace App00.Views;

public partial class RecommendPage : ContentPage
{
    public RecommendPage()
    {
        InitializeComponent();
    }

    private async void OnSpotClicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            string name = btn.ClassId;
            string description = name switch
            {
                "大雁塔" => "唐代古塔，位于大慈恩寺内。",
                "钟楼" => "西安市中心标志建筑。",
                "兵马俑" => "世界八大奇迹之一，秦始皇陵陪葬坑。",
                "华清宫" => "唐代皇家温泉行宫，杨贵妃故事发生地。",
                _ => "暂无介绍"
            };
            await DisplayAlert(name, description, "关闭");
        }
    }
}