namespace BiyaHero;

public partial class UserProfilePage : ContentPage
{
    public UserProfilePage()
    {
        InitializeComponent();
    }
    private async void OnEditUserProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserProfileEdit()); // Navigate to UserEditProfilePage
    }
    private async void OnTransactionHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TransactionHistoryPage()); // Navigate to UserEditProfilePage
    }
}