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

    private async void OnSearch(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        string input = searchBar.Text?.ToLower() ?? "";

        if (input.Contains("qin") || input.Contains("emperor") || input.Contains("mausoleum"))
        {
            await Navigation.PushAsync(new DetailPage("Emperor Qinshihuang's Mausoleum Site Museum"));
            searchBar.Text = "";
        }
        else if (input.Contains("dayan") || input.Contains("pagoda"))
        {
            await Navigation.PushAsync(new DetailPage("Dayan Pagoda"));
            searchBar.Text = "";
        }
        else if (input.Contains("xi'an") || input.Contains("circumvallation") || input.Contains("wall"))
        {
            await Navigation.PushAsync(new DetailPage("The Xi'an Circumvallation"));
            searchBar.Text = "";
        }
    }
}