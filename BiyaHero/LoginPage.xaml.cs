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

        var user = await _databaseService.GetUserByEmailAsync(email);

        if (user == null)
        {
            await DisplayAlert("Error", "No account found with this email. Please sign up first.", "OK");
        }
        else if (user.Password == password)
        {
            await DisplayAlert("Success", "Login successful!", "OK");
            await Navigation.PushAsync(new MainPage());
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
}
