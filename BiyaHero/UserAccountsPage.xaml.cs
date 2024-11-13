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
            var button = (Button)sender; // Cast sender to Button
            var user = button.CommandParameter as User; // Get the user from CommandParameter
            if (user != null)
            {
                // Navigate to edit page, passing the user data
                await Navigation.PushAsync(new EditUserPage(user));
            }
        }

        // Delete user action
        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var button = (Button)sender; // Cast sender to Button
            var user = button.CommandParameter as User; // Get the user from CommandParameter
            if (user != null)
            {
                var result = await DisplayAlert("Delete User", "Are you sure you want to delete this user?", "Yes", "No");
                if (result)
                {
                    await _databaseService.DeleteUserAsync(user);
                    LoadUserAccounts(); // Reload the users after deletion
                }
            }
        }

        // Event handler for when a user is selected in the ListView
        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return; // Exit if no item is selected

            var selectedUser = e.SelectedItem as User;

            if (selectedUser != null)
            {
                // Navigate to the EditUserPage with the selected user
                await Navigation.PushAsync(new EditUserPage(selectedUser));
            }

            // Deselect the item after navigating
            UserListView.SelectedItem = null;
        }
    }
}
