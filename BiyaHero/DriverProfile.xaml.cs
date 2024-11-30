using BiyaHero.Models;
using BiyaHero.Services;

namespace BiyaHero;

public partial class DriverProfile : ContentPage
{
    private readonly DatabaseService _databaseService;

    public DriverProfile()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        LoadDriverProfile();
    }

    private async void LoadDriverProfile()
    {
        try
        {
            string driverEmail = UserSession.GetUserEmail(); // Fetch email from session
            if (!string.IsNullOrEmpty(driverEmail))
            {
                Driver driver = await _databaseService.GetDriverByEmailAsync(driverEmail);
                if (driver != null)
                {
                    DriverNameLabel.Text = $"{driver.FirstName} {driver.LastName}";
                    DriverEmailLabel.Text = driver.Email;
                    LicenseIdLabel.Text = driver.VehiclePlateNumber;
                    VehicleTypeLabel.Text = driver.VehicleType;

                    if (driver.Photo != null)
                    {
                        DriverImage.Source = ImageSource.FromStream(() => new MemoryStream(driver.Photo));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to load driver profile: {ex.Message}", "OK");
        }
    }

    private async void OnBookingHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DriverBookingHistory());
    }
}
