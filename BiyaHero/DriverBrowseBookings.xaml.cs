using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BiyaHero.Models;
using System.Diagnostics;
using Microsoft.Maui.Devices.Sensors;
using System.Threading.Tasks;
using Microsoft.Maui.Maps;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace BiyaHero;

public partial class DriverBrowseBookings : ContentPage, INotifyPropertyChanged {
    private readonly FirebaseClient firebaseClient;
    private Location driverLocation;
    public ObservableCollection<BookingData> Bookings { get; set; }

    public ICommand ButtonClickedCommand { get; }

    public DriverBrowseBookings() {
        InitializeComponent();

        firebaseClient = new FirebaseClient("https://biyahero-7d8a3-default-rtdb.firebaseio.com");

        Bookings = new ObservableCollection<BookingData>();
        ButtonClickedCommand = new Command(ViewTrip);

        BindingContext = this;
    }

    protected override async void OnAppearing() {
        base.OnAppearing();

        await TestUpdate();

        await GetUserLocation();
        await LoadBookings();
    }

    private async Task TestUpdate() {
        await firebaseClient.Child("test/zz").PutAsync(new BookingData());
    }

    private async Task GetUserLocation() {
        try {
            // Get the user's current location
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

            if (location != null) {
                // Assign the location to driverLocation
                driverLocation = new Location(location.Latitude, location.Longitude);
            } else {
                await DisplayAlert("Error", "Unable to retrieve current location.", "OK");
            }
        } catch (Exception ex) {
            // Handle any exceptions, such as permission issues or service errors
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async Task LoadBookings() {
        // Reset the content of Bookings collection at the start
        Bookings.Clear();

        // Fetch all bookings from Firebase
        var bookingsSnapshot = await firebaseClient
            .Child("Bookings")
            .OnceAsync<BookingData>();

        // Add the fetched bookings to the Bookings collection
        MainThread.BeginInvokeOnMainThread(() => {
            bool hasActive = false;

            foreach (var booking in bookingsSnapshot) {
                var bookingData = booking.Object;

                if (bookingData.type == "active") {
                    bookingData.uuid = booking.Key;  // Set the UUID from Firebase key
                    bookingData.distance = CalculateDistance(bookingData.originLat, bookingData.originLong);
                    Bookings.Add(bookingData);  // Add the booking to the collection
                    hasActive = true;
                }
            }

            if (!hasActive) {
                BookingsNone.IsVisible = true;
            } else {
                BookingsNone.IsVisible = false;
            }
        });
    }

    private double CalculateDistance(double lat, double lon) {
        var originLocation = new Location(driverLocation.Latitude, driverLocation.Longitude);
        var destinationLocation = new Location(lat, lon);

        // Get the distance between the origin and destination in meters
        double distance = originLocation.CalculateDistance(destinationLocation, DistanceUnits.Kilometers) * 100; 

        double roundedDistance = Math.Round(distance, 2);

        return roundedDistance;
    }

    private void OnRefreshClicked(object sender, EventArgs e) {
        LoadBookings();
    }

    // BOOKING ITEM ON CLICK
   private async void ViewTrip(object obj) {
        var booking = obj as BookingData;

        string type = await firebaseClient
            .Child($"Bookings/{booking.uuid}/type").OnceSingleAsync<string>();


        if (type != null) {
            if (type == "active") {
                await Navigation.PushAsync(new DriverViewTrip(booking));
                return;
            } else {
                await DisplayAlert("Error", "Booking already filled by another driver.", "OK");
                LoadBookings();
            }
        } else {
            await DisplayAlert("Error", "Booking already cancelled by the rider.", "OK");
            LoadBookings();
        }

    }
}
