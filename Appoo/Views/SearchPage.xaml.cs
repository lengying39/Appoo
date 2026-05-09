using Appoo.Services;

namespace Appoo.Views;

public partial class SearchPage : ContentPage
{
    private readonly IDataService _dataService;
    public SearchPage(IDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ResultsStack.Children.Clear();
        var keyword = e.NewTextValue?.ToLower() ?? "";
        if (string.IsNullOrWhiteSpace(keyword)) return;

        foreach (var spot in _dataService.GetAllSpots().Where(s => s.Name.ToLower().Contains(keyword)))
            ResultsStack.Children.Add(new Label { Text = spot.Name, FontSize = 16 });

        foreach (var food in _dataService.GetAllFoods().Where(f => f.ToLower().Contains(keyword)))
            ResultsStack.Children.Add(new Label { Text = food, FontSize = 16 });

        foreach (var fac in _dataService.GetAllFacilities().Where(f => f.ToLower().Contains(keyword)))
            ResultsStack.Children.Add(new Label { Text = fac, FontSize = 16 });
    }
}