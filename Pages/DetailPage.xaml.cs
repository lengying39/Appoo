using Microsoft.Maui.Controls;

namespace Assignment.Pages;

public partial class DetailPage : ContentPage
{
    string currentTitle = "";

    public DetailPage(string title)
    {
        InitializeComponent();
        currentTitle = title;
        AttractionName.Text = title;

        if (title == "Emperor Qinshihuang's Mausoleum Site Museum")
        {
            AttractionInfo.Text = "The Terracotta Army is a collection of terracotta sculptures depicting the armies of Qin Shi Huang.";
            OpeningHours.Text = "Open: 8:00 - 18:00";
            Location.Text = "Location: Lintong District, Xi'an";
            AttractionImage.Source = "icon1.jpg";
        }
        else if (title == "Dayan Pagoda")
        {
            AttractionInfo.Text = "Dayan Pagoda is a Buddhist pagoda built in the Tang Dynasty.";
            OpeningHours.Text = "Open: 9:00 - 17:30";
            Location.Text = "Location: Yanta District, Xi'an";
            AttractionImage.Source = "icon2.jpg";
        }
        else if (title == "The Xi'an Circumvallation")
        {
            AttractionInfo.Text = "The Xi'an Circumvallation is the most complete ancient city wall in China.";
            OpeningHours.Text = "Open: 8:00 - 22:00";
            Location.Text = "Location: Downtown Xi'an";
            AttractionImage.Source = "icon3.jpg";
        }
    }

    private void AddToFavorite(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(currentTitle))
        {
            FavoriteManager.AddFavorite(currentTitle);
            DisplayAlert("Success", "Added to favorites!", "OK");
        }
    }
}