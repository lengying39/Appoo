using Microsoft.Maui.Controls;

namespace Assignment.Pages;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    private async void GoToBingMaYong(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("兵马俑"));
    }

    private async void GoToDaYanTa(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("大雁塔"));
    }

    private async void GoToXiAnChengQiang(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("西安城墙"));
    }
}