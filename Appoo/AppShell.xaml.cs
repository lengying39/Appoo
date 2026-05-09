using Appoo.Views;
namespace Appoo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(FoodPage), typeof(FoodPage));
    }
}