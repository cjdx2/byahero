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

        private async void LoadUserAccounts()
        {
            var users = await _databaseService.GetAllUsersAsync();
            UserListView.ItemsSource = users;
        }

        private void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedUser = e.SelectedItem as User;
            }
        }

        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedUser = button?.BindingContext as User;

            if (selectedUser != null)
            {
                var editPage = new EditUserPage(selectedUser);
                await Navigation.PushAsync(editPage);
            }
        }

        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedUser = button?.BindingContext as User;

            if (selectedUser != null)
            {
                var confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this user?", "Yes", "No");
                if (confirmDelete)
                {
                    await _databaseService.DeleteUserAsync(selectedUser);
                    LoadUserAccounts();
                }
            }
        }

        private void OnMenuClicked(object sender, EventArgs e)
        {
            MenuPopup.IsVisible = true;
        }

        private void OnCloseMenuClicked(object sender, EventArgs e)
        {
            MenuPopup.IsVisible = false;
        }

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

        private async void OnReportsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportsPage());
        }
        
        private async void OnAdminLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
