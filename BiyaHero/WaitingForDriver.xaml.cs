using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using Map = Microsoft.Maui.Controls.Maps.Map;
using BiyaHero.Services;
using BiyaHero.Models;
using System.CodeDom;
using Newtonsoft.Json;

namespace BiyaHero;

public partial class WaitingForDriver : ContentPage
{
    public Location origin;
    public Location destination;
    public Pin originPin = new Pin();
    public Pin destinationPin = new Pin();
    public Pin driverPin = new Pin();

    public string originAddress;
    public string destinationAddress;
    FirebaseClient firebaseClient;
    public string userName;
    public double price;
    public string paymentMethod;
    private readonly DatabaseService _databaseService;
    public string tripID;

    public WaitingForDriver(Location originL, Location destinationL, string originA, string destinationA,double tripPrice, string payment)
	{
		InitializeComponent();

        firebaseClient = new FirebaseClient("https://biyahero-7d8a3-default-rtdb.firebaseio.com");

        origin = originL;
        destination = destinationL;
        originAddress = originA;
        destinationAddress = destinationA;
        price = tripPrice;
        paymentMethod = payment;

        originButton.Text = originAddress;
        destinationButton.Text = destinationAddress;

        userName = UserSession.GetUserFullName();

        ListBooking();

        LoadUserLocation(map);
    }

    protected override async void OnAppearing() {
        base.OnAppearing();

    }

    private async Task GetUserName() {
        string email = UserSession.GetUserEmail();
        var user = await _databaseService.GetUserByEmailAsync(email);
        userName = $"{user.FirstName} {user.LastName}";
    }

    private async void ListBooking() {
        Guid uuid = Guid.NewGuid();
        tripID = uuid.ToString();
        var data = new BookingData {
            originLat = origin.Latitude,
            originLong = origin.Longitude,
            destinationLat = destination.Latitude,
            destinationLong = destination.Longitude,
            originAddress = originAddress,
            destinationAddress = destinationAddress,
            riderName = userName,
            type = "active",
            price = price,
            paymentMethod = paymentMethod
        };

        await firebaseClient.Child("Bookings").Child(uuid.ToString()).PutAsync(data);
        ListenToBooking(uuid.ToString());
    }

    public void ListenToBooking(string uuid) {
        firebaseClient
            .Child($"Bookings/{uuid}")
            .AsObservable<object>()
            .Subscribe(data => {
                if (data.Object != null) {
                    MainThread.BeginInvokeOnMainThread(async () => {
                        if (data.Key == "type") {
                            string type = data.Object.ToString();

                            if (type == "filled") {
                                cancelBookingButton.IsVisible = false;
                                chatButton.IsVisible = true;
                                await DisplayAlert("Booking filled", "Your driver is on the way.", "Ok");
                            } else if (type == "picked up") {
                                await DisplayAlert("Picked up", "Sit back, relax, and enjoy a safe ride to your destination!", "Ok");

                                Application.Current.MainPage = new NavigationPage(new UserHomePage());
                            }
                        } else if (data.Key == "driver") {
                            var objSerialized = data.Object.ToString();

                            var driverData = JsonConvert.DeserializeObject<DriverData>(objSerialized);

                            var driverLocation = new Location(driverData.currentLat, driverData.currentLong);

                            map.Pins.Clear();
                            AddPin(map, origin, "Origin");
                            AddPin(map, destination, "Destination");
                            AddPin(map, driverLocation, "Driver");
                        }
                    });
                }
            });
    }

    private async void LoadUserLocation(Map map) {
        try {
            // Get the user's current location
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

            if (location != null) {
                // Create a Position for the user's current location
                var userPosition = new Location(location.Latitude, location.Longitude);

                // Move the map to the user's current location
                map.MoveToRegion(MapSpan.FromCenterAndRadius(userPosition, Distance.FromMiles(1)));

                // Add the origin pin at the user's location
                AddPin(map, origin, "Origin");
                AddPin(map, destination, "Destination");
            } else {
                // Handle the case when location is not available
                await DisplayAlert("Error", "Unable to retrieve current location.", "OK");
            }
        } catch (Exception ex) {
            // Handle any exceptions (e.g., no permissions or location service issues)
            await DisplayAlert("Error", "An error occurred while retrieving the location: " + ex.Message, "OK");
        }
    }

    private void AddPin(Map map, Location location, string type) {
        Pin pin;

        if (type == "Origin") {
            pin = originPin;
        } else if (type == "Destination") {
            pin = destinationPin;
        } else if (type == "Driver") {
            pin = driverPin;
        }

        pin = new Pin {
            Label = type,
            Type = PinType.Place,
            Location = location
        };

        map.Pins.Add(pin);  // Add origin pin to map
    }    

    private async void OnCancelBookingClicked(object sender, EventArgs e) {
        await firebaseClient.Child($"Bookings/{tripID}").DeleteAsync();
        await Navigation.PopAsync();
    }

    private async void OnChatClicked(object sender, EventArgs e) {
        await Navigation.PushAsync(new UserChat(tripID));
    }
}