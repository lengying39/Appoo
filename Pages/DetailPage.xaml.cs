using Microsoft.Maui.Controls;

namespace Assignment.Pages;

public partial class DetailPage : ContentPage
{
    string currentTitle = "";

    public DetailPage(string title)
    {
        InitializeComponent();
        currentTitle = title;
    }
}