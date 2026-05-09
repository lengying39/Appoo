using Appoo.Services;

namespace Appoo.Views;

public partial class PublicFacilitiesPage : ContentPage
{
    private readonly IDataService _dataService;
    public PublicFacilitiesPage(IDataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        LoadFacilities();
    }

    private void LoadFacilities()
    {
        foreach (var fac in _dataService.GetAllFacilities())
            FacilitiesStack.Children.Add(new Label { Text = fac, FontSize = 16 });
    }
}