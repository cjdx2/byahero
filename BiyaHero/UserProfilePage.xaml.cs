using BiyaHero.Services;

namespace BiyaHero;

public partial class UserProfilePage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public UserProfilePage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        LoadUserProfile();
    }

    public async Task RefreshUserProfile()
    {
        // Fetch the updated user details and refresh the displayed data
        await LoadUserProfile();
    }

    private async Task LoadUserProfile()
    {
        string email = UserSession.GetUserEmail();

        if (string.IsNullOrEmpty(email))
        {
            await DisplayAlert("Error", "Unable to load user profile. Please log in again.", "OK");
            await Navigation.PopToRootAsync();
            return;
        }

        var user = await _databaseService.GetUserByEmailAsync(email);
        if (user != null)
        {
            UsernameLabel.Text = user.Username;
            FullNameLabel.Text = $"{user.FirstName} {user.LastName}";
            EmailLabel.Text = user.Email;
        }
        else
        {
            await DisplayAlert("Error", "User not found. Please log in again.", "OK");
            await Navigation.PopToRootAsync();
        }
    }

    private async void OnEditUserProfileClicked(object sender, EventArgs e)
    {
        // Pass reference of the current page to the edit page
        await Navigation.PushAsync(new UserProfileEdit(this));
    }

    private async void OnTransactionHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TransactionHistoryPage());
    }
}
