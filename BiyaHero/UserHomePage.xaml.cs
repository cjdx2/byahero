
namespace BiyaHero;

public partial class UserHomePage : ContentPage
{
	public UserHomePage()
	{
        InitializeComponent();
        // Disable the navigation bar for LoginPage
        NavigationPage.SetHasNavigationBar(this, false);
    }

}