namespace BiyaHero;

public partial class DriverProfile : ContentPage
{
	public DriverProfile()
	{
		InitializeComponent();
	}

    private async void OnBookingHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DriverBookingHistory());
    }
}