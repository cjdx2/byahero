using BiyaHero.Services;
using BiyaHero.Models;

namespace BiyaHero
{
    public partial class UserAccountsPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public UserAccountsPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LoadUserAccounts();
        }

        // Load user accounts from the database
        private async void LoadUserAccounts()
        {
            var users = await _databaseService.GetAllUsersAsync();
            UserListView.ItemsSource = users;
        }

        // Edit user action (navigate to EditUserPage or open a dialog)
        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedUser = button?.BindingContext as User;

            if (selectedUser != null)
            {
                // Navigate to edit page, passing the user data
                var editPage = new EditUserPage(selectedUser);
                await Navigation.PushAsync(editPage);
            }
        }

        // Delete user action
        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedUser = button?.BindingContext as User;

            if (selectedUser != null)
            {
                var result = await DisplayAlert("Delete User", "Are you sure you want to delete this user?", "Yes", "No");
                if (result)
                {
                    await _databaseService.DeleteUserAsync(selectedUser);
                    LoadUserAccounts(); // Reload the users after deletion
                }
            }
        }

        // Event handler for when a user is selected in the ListView
        private void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedUser = e.SelectedItem as User;
                // Handle selection if needed (e.g., navigate to details)
            }
        }

        // Show the menu popup
        private void OnMenuClicked(object sender, EventArgs e)
        {
            MenuPopup.IsVisible = true;
        }

        // Hide the menu popup
        private void OnCloseMenuClicked(object sender, EventArgs e)
        {
            MenuPopup.IsVisible = false;
        }

        // Handle navigation or actions for each menu item
        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminHomePage());
        }

        private async void OnDriverAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverAccountsPage());
        }

        private async void OnUserAccountsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("User Accounts Management displayed", "You are currently viewing User Accounts", "Close");
        }

        private async void OnDriverHistoryClicked(object sender, EventArgs e)
        {
            await DisplayAlert("User Ride History", "Viewing user ride history.", "OK");
        }
    }
}
