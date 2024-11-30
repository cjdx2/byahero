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
                    VehiclePlateNumberLabel.Text = driver.VehiclePlateNumber;
                    VehicleTypeLabel.Text = driver.VehicleType;

                    // Use the default profile photo from the Resources/Images folder
                    DriverImage.Source = "icon_profilebtn2.png";
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

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        try
        {
            string driverEmail = UserSession.GetUserEmail();
            if (!string.IsNullOrEmpty(driverEmail))
            {
                string newPassword = await DisplayPromptAsync(
                    "Change Password",
                    "Enter your new password:",
                    "OK",
                    "Cancel",
                    keyboard: Keyboard.Text);

                if (!string.IsNullOrEmpty(newPassword))
                {
                    Driver driver = await _databaseService.GetDriverByEmailAsync(driverEmail);
                    if (driver != null)
                    {
                        // Update the password
                        driver.Password = newPassword;
                        await _databaseService.UpdateDriverAsync(driver);

                        await DisplayAlert("Success", "Password changed successfully.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Cancelled", "Password change cancelled.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to change password: {ex.Message}", "OK");
        }
    }
}
