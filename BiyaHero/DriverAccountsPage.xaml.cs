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

        // Load driver accounts from SQLite
        private async void LoadDriverAccounts()
        {
            var drivers = await _databaseService.GetAllDriversAsync();
            DriverListView.ItemsSource = drivers;
        }

        // Handler for driver selection (optional)
        private void OnDriverSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedDriver = e.SelectedItem as Driver;
                // Handle selection (e.g., navigate to a detail page or show options)
            }
        }

        // Edit driver button handler
        private async void OnEditDriverClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedDriver = button?.BindingContext as Driver;

            if (selectedDriver != null)
            {
                // Navigate to an edit page or display a dialog to edit driver info
                var editPage = new EditDriverPage(selectedDriver);
                await Navigation.PushAsync(editPage);
            }
        }

        // Delete driver button handler
        private async void OnDeleteDriverClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedDriver = button?.BindingContext as Driver;

            if (selectedDriver != null)
            {
                var confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this driver?", "Yes", "No");
                if (confirmDelete)
                {
                    // Call the Delete method to remove the driver from the database
                    await _databaseService.DeleteDriverAsync(selectedDriver);
                    LoadDriverAccounts(); // Refresh the list after deletion
                }
            }
        }
    }
}
