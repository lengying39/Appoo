namespace Assignment.Pages;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    private async void GoToDetail1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("Emperor Qinshihuang's Mausoleum Site Museum"));
    }

    private async void GoToDetail2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("Dayan Pagoda"));
    }

    private async void GoToDetail3(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DetailPage("The Xi'an Circumvallation"));
    }
}