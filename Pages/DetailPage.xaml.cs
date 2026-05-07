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

        if (name == "Terracotta Army")
        {
            Title = "Terracotta Army";
            AttractionNameLabel.Text = "Terracotta Army";
            AddressLabel.Text = "Address: Lintong District, Xi'an";
            TimeLabel.Text = "Open Time: 8:00 - 18:00";
            TicketLabel.Text = "Ticket: 120 RMB";
            DescLabel.Text = "One of the Eight Wonders of the World.";
            AttractionImage.Source = "icon1.jpg";
        }
        else if (name == "Giant Wild Goose Pagoda")
        {
            Title = "Giant Wild Goose Pagoda";
            AttractionNameLabel.Text = "Giant Wild Goose Pagoda";
            AddressLabel.Text = "Address: Yanta District, Xi'an";
            TimeLabel.Text = "Open Time: 8:00 - 18:30";
            TicketLabel.Text = "Ticket: 40 RMB";
            DescLabel.Text = "A famous Buddhist pagoda in Tang Dynasty.";
            AttractionImage.Source = "icon2.jpg";
        }
        else if (name == "Xi'an City Wall")
        {
            Title = "Xi'an City Wall";
            AttractionNameLabel.Text = "Xi'an City Wall";
            AddressLabel.Text = "Address: Downtown Xi'an";
            TimeLabel.Text = "Open Time: 8:00 - 22:00";
            TicketLabel.Text = "Ticket: 54 RMB";
            DescLabel.Text = "The most complete ancient city wall in China.";
            AttractionImage.Source = "icon3.jpg";
        }
    }
}