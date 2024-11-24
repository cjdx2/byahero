namespace BiyaHero;

public partial class DriverHomepage : ContentPage
{
	public DriverHomepage()
	{
		InitializeComponent();
	}

    private async void OnMapButtonClicked(object sender, EventArgs e)
    {
        // Navigate to the Target Page
        await Navigation.PushAsync(new MainPage());
    }

}