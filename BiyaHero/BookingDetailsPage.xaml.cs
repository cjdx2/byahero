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
        }
    }
}
