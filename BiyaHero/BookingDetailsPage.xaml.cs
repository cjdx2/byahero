using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using BiyaHero.Services;

namespace BiyaHero
{
    public partial class BookingDetailsPage : ContentPage
    {
        public BookingDetailsPage(List<Trip> trips)
        {
            InitializeComponent();
            BookingsCollectionView.ItemsSource = trips;

            // Initialize the command
            AcceptBookingCommand = new Command<Trip>((trip) => AcceptBooking(trip));
        }

        // Define the AcceptBookingCommand
        public Command<Trip> AcceptBookingCommand { get; }

        // Method to handle the booking acceptance logic
        private async void AcceptBooking(Trip trip)
        {
            try
            {
                // Logic to accept the booking (e.g., update database, mark as accepted, etc.)
                // For now, we will just display a message and remove the trip from the list
                await DisplayAlert("Booking Accepted", $"Booking for {trip.FirstName} {trip.LastName} accepted.", "OK");

                // Optionally, remove the trip from the list or mark it as accepted
                // Assuming `BookingsCollectionView.ItemsSource` is a list of trips:
                var trips = (List<Trip>)BookingsCollectionView.ItemsSource;
                trips.Remove(trip);

                // Refresh the CollectionView to show the updated list
                BookingsCollectionView.ItemsSource = null;
                BookingsCollectionView.ItemsSource = trips;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to accept booking.", "OK");
            }
        }
    }
}
