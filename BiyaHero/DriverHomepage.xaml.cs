using System;
using Microsoft.Maui.Controls;
using BiyaHero.Services;

namespace BiyaHero
{
    public partial class DriverHomepage : ContentPage
    {
        // Declare _databaseService as a private field
        private DatabaseService _databaseService;

        public DriverHomepage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            // Initialize the _databaseService in the constructor
            _databaseService = new DatabaseService();
        }

        private async void OnMapButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the Target Page
            await Navigation.PushAsync(new MainPage());
        }

        // Toggle sidebar visibility when Menu button is clicked
        private void OnNavigationButtonClicked(object sender, EventArgs e)
        {
            DriverMenuPopup.IsVisible = true;
        }

        private void OnCloseMenuClicked(object sender, EventArgs e)
        {
            DriverMenuPopup.IsVisible = false;
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Home", "Navigating to Home.", "OK");
            // Logic for navigating to the Home content can go here
        }

        private async void OnTransactionHistoryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverTransactionHistoryPage()); // Navigate to TransactionHistoryPage
        }
        private async void OnDriverProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DriverProfile());
            // Logic for logout action
        }
        private async void OnDriverLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
            // Logic for logout action
        }
        private async void OnLookForBookingsClicked(object sender, EventArgs e)
        {
            try
            {
                // Fetch trips from the database
                var trips = await _databaseService.GetTripsAsync();

                // You can now navigate to a new page or update a list view on the current page
                // For example, push a new page that shows the bookings

                await Navigation.PushAsync(new BookingDetailsPage(trips)); // pass trips to the new page
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load bookings.", "OK");
            }
        }
    }
}
