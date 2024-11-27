using System;
using Microsoft.Maui.Controls;

namespace BiyaHero
{
    public partial class PaymentMethodsPage : ContentPage
    {
        public PaymentMethodsPage()
        {
            InitializeComponent();
        }

        private async void OnCreditCardButtonClicked(object sender, EventArgs e)
        {
            // Handle credit card selection (e.g., navigate to credit card entry form)
            await DisplayAlert("Payment Method", "You selected Credit/Debit Card", "OK");
        }

        private async void OnCashOnDeliveryButtonClicked(object sender, EventArgs e)
        {
            // Handle cash on delivery selection
            await DisplayAlert("Payment Method", "Cash", "OK");
        }

        private async void OnPayPalButtonClicked(object sender, EventArgs e)
        {
            // Handle PayPal selection
            await DisplayAlert("Payment Method", "You selected PayPal", "OK");
        }
    }
}
