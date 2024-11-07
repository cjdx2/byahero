namespace BiyaHero;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        // Disable the navigation bar for LoginPage
        NavigationPage.SetHasNavigationBar(this, false);
    }
}