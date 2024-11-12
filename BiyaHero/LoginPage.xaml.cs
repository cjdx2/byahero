using BiyaHero.Services;
using BiyaHero.Models;

namespace BiyaHero;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public LoginPage()
    {
        InitializeComponent();
        // Initialize the database service
        _databaseService = new DatabaseService();

        // Disable the navigation bar for LoginPage
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            // Show an error message if either field is empty
            await DisplayAlert("Error", "Please enter both email and password.", "OK");
            return;
        }

        // Query the database to check if the user exists
        var user = await _databaseService.GetUserByEmailAsync(email);

        if (user == null)
        {
            // If the user doesn't exist, show an error message
            await DisplayAlert("Error", "No account found with this email. Please sign up first.", "OK");
        }
        else
        {
            // Check if the password matches
            if (user.Password == password)  // Make sure you're storing passwords securely (hashing them is recommended)
            {
                // Successful login, navigate to the next page (MainPage)
                await DisplayAlert("Success", "Login successful!", "OK");

                // Navigate to MainPage after successful login
                await Navigation.PushAsync(new MainPage()); // Assuming MainPage is the main screen
            }
            else
            {
                // If the password is incorrect
                await DisplayAlert("Error", "Incorrect password. Please try again.", "OK");
            }
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        // Navigate to the registration page
        await Navigation.PushAsync(new RegisterPage());
    }
}
