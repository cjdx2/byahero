using BiyaHero.Services;
using BiyaHero.Models;

namespace BiyaHero;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public LoginPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Please enter both email and password.", "OK");
            return;
        }

        // Check if the email and password match the admin credentials
        if (email == "byahero2024@gmail.com" && password == "By@Her0")
        {
            await DisplayAlert("Admin Login", "Welcome, Admin!", "OK");
            await Navigation.PushAsync(new AdminHomePage()); // Navigate to AdminHomePage
            return;
        }

        // Check in the User table
        var user = await _databaseService.GetUserByEmailAsync(email);

        // Check in the Driver table if no match is found in the User table
        var driver = user == null ? await _databaseService.GetDriverByEmailAsync(email) : null;

        if (user == null && driver == null)
        {
            await DisplayAlert("Error", "No account found with this email. Please sign up first.", "OK");
            return;
        }

        if (user != null && user.Password == password)
        {
            UserSession.SaveUserEmail(user.Email); // Save user email in the session
            await DisplayAlert("Success", "Login successful!", "OK");
            await Navigation.PushAsync(new UserHomePage()); // Navigate to UserHomePage
        }
        else if (driver != null && driver.Password == password)
        {
            UserSession.SaveUserEmail(driver.Email); // Save driver email in the session
            await DisplayAlert("Success", "Driver login successful!", "OK");
            await Navigation.PushAsync(new DriverHomepage()); // Navigate to DriverHomePage
        }
        else
        {
            await DisplayAlert("Error", "Incorrect password. Please try again.", "OK");
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    private async void OnDriverRegisterButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DriverRegisterPage());
    }

    private async void OnForgotButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserHomePage());
    }

}
