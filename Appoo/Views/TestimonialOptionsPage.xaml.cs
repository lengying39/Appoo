using Appoo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appoo.Views;

[QueryProperty(nameof(SpotName), "spotName")]
public partial class TestimonialOptionsPage : ContentPage
{
    public string SpotName { get; set; }

    public TestimonialOptionsPage()
    {
        InitializeComponent();
    }

    private async void OnWriteReviewClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(WriteReviewPage)}?spotName={SpotName}");
    }

    private async void OnViewReviewsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(ViewReviewsPage)}?spotName={SpotName}");
    }
}