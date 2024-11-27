using System;
using Microsoft.Maui.Controls;

namespace BiyaHero
{
    public partial class DriverHomepage : ContentPage
    {
        public DriverHomepage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
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
            await Navigation.PushAsync(new TransactionHistoryPage()); // Navigate to TransactionHistoryPage
        }

        private async void OnDriverLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
            // Logic for logout action
        }

    
    }
}
