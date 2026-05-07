using Microsoft.Maui.Controls;

namespace Assignment.Pages;

public partial class DetailPage : ContentPage
{
    public DetailPage()
    {
        InitializeComponent();
    }

    public DetailPage(string name)
    {
        InitializeComponent();

        if (name == "兵马俑")
        {
            Title = "兵马俑";
            AttractionNameLabel.Text = "兵马俑";
            AddressLabel.Text = "地址：西安市临潼区";
            TimeLabel.Text = "开放时间：8:00-18:00";
            TicketLabel.Text = "门票：120元";
            DescLabel.Text = "世界八大奇迹之一。";
            // 关键：路径改为 Images/xxx.jpg
            AttractionImage.Source = "Images/icon1.jpg";
        }
        else if (name == "大雁塔")
        {
            Title = "大雁塔";
            AttractionNameLabel.Text = "大雁塔";
            AddressLabel.Text = "地址：西安市雁塔区";
            TimeLabel.Text = "开放时间：8:00-18:30";
            TicketLabel.Text = "门票：40元";
            DescLabel.Text = "唐代佛教建筑地标。";
            AttractionImage.Source = "Images/icon2.jpg";
        }
        else if (name == "西安城墙")
        {
            Title = "西安城墙";
            AttractionNameLabel.Text = "西安城墙";
            AddressLabel.Text = "地址：西安市中心";
            TimeLabel.Text = "开放时间：8:00-22:00";
            TicketLabel.Text = "门票：54元";
            DescLabel.Text = "中国最完整古城墙。";
            AttractionImage.Source = "Images/icon3.jpg";
        }
    }
}