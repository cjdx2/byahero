using BiyaHero.Services;
using BiyaHero.Models;

namespace BiyaHero;

public partial class UserProfileEdit : ContentPage
{
    private readonly DatabaseService _databaseService;
    private readonly UserProfilePage _parentPage;
    private User _currentUser;

    public UserProfileEdit(UserProfilePage parentPage)
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        _parentPage = parentPage; // Pass parent page reference
        LoadUserDetails();
    }

    private async void LoadUserDetails()
    {
        string email = UserSession.GetUserEmail(); // Retrieve the logged-in user's email
        if (string.IsNullOrEmpty(email))
        {
            await DisplayAlert("Error", "Unable to load user details. Please log in again.", "OK");
            await Navigation.PopToRootAsync();
            return;
        }

        _currentUser = await _databaseService.GetUserByEmailAsync(email);

        if (_currentUser != null)
        {
            UsernameEntry.Text = _currentUser.Username;
            FirstNameEntry.Text = _currentUser.FirstName;
            LastNameEntry.Text = _currentUser.LastName;
            EmailEntry.Text = _currentUser.Email;
        }
        else
        {
            await DisplayAlert("Notification", "Please log in again.", "OK");
            await Navigation.PopToRootAsync();
        }
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (_currentUser != null)
        {
            // Update user details
            _currentUser.Username = UsernameEntry.Text?.Trim();
            _currentUser.FirstName = FirstNameEntry.Text?.Trim();
            _currentUser.LastName = LastNameEntry.Text?.Trim();
            _currentUser.Email = EmailEntry.Text?.Trim();

            // Save to database
            int result = await _databaseService.UpdateUserAsync(_currentUser);
            if (result > 0)
            {
                // Update the session email if the email is changed
                if (_currentUser.Email != UserSession.GetUserEmail())
                {
                    UserSession.UpdateUserEmail(_currentUser.Email);  // This assumes you have a method to update the session email
                }

                await DisplayAlert("Success", "Profile updated successfully. Please log in again to apply the changes.", "OK");

                // Notify the parent page of changes
                await _parentPage.RefreshUserProfile(); // Explicitly refresh parent page

                // Clear the user session (force the user to log in again)
                UserSession.ClearSession();

                // Navigate back to the login page
                await Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to update profile. Please try again.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Unable to save changes. User not found.", "OK");
        }
    }
}
