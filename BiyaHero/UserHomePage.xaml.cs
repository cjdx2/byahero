﻿using System;
using Microsoft.Maui.Controls;

namespace BiyaHero
{
    public partial class UserHomePage : ContentPage
    {
        public UserHomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new MainPageViewModel();

        }

        // Button click event handler
        private void OnGetPriceClicked(object sender, EventArgs e)
        {
            var viewModel = (MainPageViewModel)BindingContext;

            // Trigger the price calculation in ViewModel
            viewModel.UpdateDistanceAndPrice();
        }

        private async void OnMapButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the Target Page
            await Navigation.PushAsync(new MainPage());
        }

        // Toggle sidebar visibility when Menu button is clicked
        private void OnNavigationButtonClicked(object sender, EventArgs e)
        {
            UserMenuPopup.IsVisible = true;
        }

        private void OnCloseMenuClicked(object sender, EventArgs e)
        {
            UserMenuPopup.IsVisible = false;
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Home", "Navigating to Home.", "OK");
            // Logic for navigating to the Home content can go here
        }

        private async void OnUserProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserProfilePage()); // Navigate to UserProfilePage
        }

        private async void OnUserReportClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserReportPage()); // Navigate to UserReportPage
        }

        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserProfilePage()); // Navigate to UserReportPage
        }

        private async void OnUserLogoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
            // Logic for navigating to Driver Ride History content can go here
        }

        private async void OnPaymentMethodsButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the PaymentMethodsPage
            await Navigation.PushAsync(new PaymentMethodsPage());
        }

        private async void OnSaveTripClicked(object sender, EventArgs e)
        {
            var viewModel = (MainPageViewModel)BindingContext;

            if (!string.IsNullOrEmpty(viewModel.SelectedFrom) &&
                !string.IsNullOrEmpty(viewModel.SelectedTo) &&
                !string.IsNullOrEmpty(viewModel.DistanceResult) &&
                !string.IsNullOrEmpty(viewModel.PriceResult))
            {
                var distance = double.Parse(viewModel.DistanceResult.Replace("Distance: ", "").Replace(" kilometer.", ""));
                var price = double.Parse(viewModel.PriceResult.Replace("Price: ₱", ""));

                var trip = new Trip
                {
                    From = viewModel.SelectedFrom,
                    To = viewModel.SelectedTo,
                    Distance = distance,
                    Price = price
                };

                await DisplayAlert("Success", "Book successfully!", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Please select both places and calculate the price first.", "OK");
            }

        }

    }
}
