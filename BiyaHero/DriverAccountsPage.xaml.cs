using BiyaHero.Services;
using BiyaHero.Models;

namespace BiyaHero
{
    public partial class DriverAccountsPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public DriverAccountsPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LoadDriverAccounts();
        }

        private async void LoadDriverAccounts()
        {
            var drivers = await _databaseService.GetAllDriversAsync();
            DriverListView.ItemsSource = drivers;
        }

        private void OnDriverSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedDriver = e.SelectedItem as Driver;
            }
        }

        private async void OnEditDriverClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedDriver = button?.BindingContext as Driver;

            if (selectedDriver != null)
            {
                var editPage = new EditDriverPage(selectedDriver);
                await Navigation.PushAsync(editPage);
            }
        }

        private async void OnDeleteDriverClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedDriver = button?.BindingContext as Driver;

            if (selectedDriver != null)
            {
                var confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this driver?", "Yes", "No");
                if (confirmDelete)
                {
                    await _databaseService.DeleteDriverAsync(selectedDriver);
                    LoadDriverAccounts();
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
            await DisplayAlert("Driver Accounts Management displayed", "You are currently viewing Driver Accounts", "Close");
        }

        private async void OnUserAccountsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserAccountsPage());
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
