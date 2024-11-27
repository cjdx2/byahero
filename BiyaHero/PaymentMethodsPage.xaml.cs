using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui; // This is where Launcher is now included

namespace BiyaHero
{
    public partial class PaymentMethodsPage : ContentPage
    {
        private string currentPaymentMethod = "Cash"; // Default payment method

        public PaymentMethodsPage()
        {
            InitializeComponent();
            UpdatePaymentMethodLabel(); // Update the UI to show the default method
        }

        private void UpdatePaymentMethodLabel()
        {
            // You can update a label or button text to show the current payment method
            if (currentPaymentMethod == "Cash")
            {
                // Update button text or UI to show Cash as default
                CashButton.Text = "Cash (Default)";
            }
            else
            {
                CashButton.Text = "Cash";
            }
        }

        private async void OnCreditCardButtonClicked(object sender, EventArgs e)
        {
            // Handle switching to Stripe (Credit/Debit Card)
            currentPaymentMethod = "Stripe";
            await DisplayAlert("Payment Method", "You selected Credit/Debit Card (via Stripe)", "OK");
            UpdatePaymentMethodLabel(); // Update UI

            // Open Stripe app or go to PlayStore if app is not installed
            await OpenPaymentApp("com.stripe.android");
        }

        private async void OnCashOnDeliveryButtonClicked(object sender, EventArgs e)
        {
            // Handle cash on delivery selection
            currentPaymentMethod = "Cash";
            await DisplayAlert("Payment Method", "You selected Cash (Default)", "OK");
            UpdatePaymentMethodLabel(); // Update UI
        }

        private async void OnPayPalButtonClicked(object sender, EventArgs e)
        {
            // Handle PayPal selection
            currentPaymentMethod = "PayPal";
            await DisplayAlert("Payment Method", "You selected PayPal", "OK");
            UpdatePaymentMethodLabel(); // Update UI

            // Open PayPal app or go to PlayStore if app is not installed
            await OpenPaymentApp("com.paypal.android.p2pmobile");
        }

        private async Task OpenPaymentApp(string packageName)
        {
            try
            {
                // Try to open the app using its package name
                var uri = new Uri($"intent://#Intent;package={packageName};end");
                await Launcher.OpenAsync(uri);
            }
            catch (Exception ex)
            {
                // If the app is not installed, it will throw an exception, so we open Play Store
                var playStoreUri = new Uri($"https://play.google.com/store/apps/details?id={packageName}");
                await Launcher.OpenAsync(playStoreUri);
            }
        }
    }
}