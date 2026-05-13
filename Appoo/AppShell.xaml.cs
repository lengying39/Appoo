using Appoo.Views;

namespace Appoo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(AttractionDetailPage), typeof(AttractionDetailPage));
        Routing.RegisterRoute(nameof(MyFavoritesPage), typeof(MyFavoritesPage));
        Routing.RegisterRoute(nameof(PublicFacilitiesPage), typeof(PublicFacilitiesPage));
        Routing.RegisterRoute(nameof(FoodPage), typeof(FoodPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(TestimonialOptionsPage), typeof(TestimonialOptionsPage));
        Routing.RegisterRoute(nameof(WriteReviewPage), typeof(WriteReviewPage));
        Routing.RegisterRoute(nameof(ViewReviewsPage), typeof(ViewReviewsPage));
        Routing.RegisterRoute(nameof(MyReviewsPage), typeof(MyReviewsPage));
    }
}