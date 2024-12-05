namespace BiyaHero;

using BiyaHero.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.ObjectModel;
using Map = Microsoft.Maui.Controls.Maps.Map;

public partial class DriverViewTrip : ContentPage
{
    BookingData bookingData;
    FirebaseClient firebaseClient;
    Location origin;
    Location destination;
    bool pickedUp = false;
    public ObservableCollection<ChatData> ChatLog { get; set; }


    public DriverViewTrip(BookingData booking)
	{
		InitializeComponent();

        firebaseClient = new FirebaseClient("https://biyahero-7d8a3-default-rtdb.firebaseio.com");

        bookingData = booking;
        riderName.Text = booking.riderName;
        ChatLog = new ObservableCollection<ChatData>();
        BindingContext = this;

        LoadUserLocation(map);
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

                // Add pin at the drivers's location
                AddPin(map, userPosition, "You");

                // Add pins
                origin = new Location(bookingData.originLat, bookingData.originLong);
                destination = new Location(bookingData.destinationLat, bookingData.destinationLong);

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

    private async Task<Location> GetUserLocation() {
        try {
            // Get the user's current location
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

            if (location != null) {
                // Return the user's current location as a Location object
                return new Location(location.Latitude, location.Longitude);
            } else {
                // Handle the case when location is not available
                await DisplayAlert("Error", "Unable to retrieve current location.", "OK");
                return null;
            }
        } catch (Exception ex) {
            // Handle any exceptions (e.g., no permissions or location service issues)
            await DisplayAlert("Error", "An error occurred while retrieving the location: " + ex.Message, "OK");
            return null;
        }
    }


    // Method to add the origin pin
    private void AddDriverPin(Map map, Location location) {
        Pin pin = new Pin {
            Label = "Origin",
            Type = PinType.SavedPin,
            Location = location
        };
        map.Pins.Add(pin);  // Add origin pin to map
    }

    private void AddPin(Map map, Location location, string label) {
        // label: Origin, Destination, You (for user position) 

        Pin pin = new Pin {
            Label = label,
            Type = PinType.SavedPin,
            Location = location
        };

        map.Pins.Add(pin);
    }

    private async void OnBrowseBookingsClicked(object sender, EventArgs e) {
        await Navigation.PopAsync();
    }

    private async void OnAcceptClicked(object sender, EventArgs e) {
        acceptBookingButton.IsVisible = false;
        pickedUpButton.IsVisible = true;
        openChatButton.IsVisible = true;

        bookingData.type = "filled";

        var data = bookingData;
        DriverData driverData = new DriverData();
        Location driverLocation = await GetUserLocation();
        driverData.currentLat = driverLocation.Latitude;
        driverData.currentLong = driverLocation.Longitude;

        await firebaseClient.Child($"Bookings/{bookingData.uuid}").PutAsync(data);
        await firebaseClient.Child($"Bookings/{bookingData.uuid}/driver").PutAsync(driverData);

        await DisplayAlert("Success", "Booking accepted.", "Ok");

        MonitorLocation();
        ListenToChat();
    }

    private async void OnPickedUpClicked(object sender, EventArgs e) {
        pickedUpButton.IsVisible = false;
        droppedOffButton.IsVisible = true;
        openChatButton.IsVisible = false;
        pickedUp = true;

        bookingData.type = "picked up";

        // Remove origin pin
        map.Pins.Clear();
        Location driverLocation = await GetUserLocation();
        AddPin(map, driverLocation, "You");
        AddPin(map, destination, "Destination");

        var data = bookingData;
        DriverData driverData = new DriverData();
        driverData.currentLat = driverLocation.Latitude;
        driverData.currentLong = driverLocation.Longitude;

        await firebaseClient.Child($"Bookings/{bookingData.uuid}").PutAsync(data);
        await firebaseClient.Child($"Bookings/{bookingData.uuid}/driver").PutAsync(driverData);

        await DisplayAlert("Success", "Rider picked up.", "Ok");
    }

    private async void OnDroppedOffClicked(object sender, EventArgs e) {
        droppedOffButton.IsVisible = false;

        bookingData.type = "dropped off";

        var data = bookingData;
        DriverData driverData = new DriverData();
        Location driverLocation = await GetUserLocation();
        driverData.currentLat = driverLocation.Latitude;
        driverData.currentLong = driverLocation.Longitude;

        await firebaseClient.Child($"Bookings/{bookingData.uuid}").PutAsync(data);
        await firebaseClient.Child($"Bookings/{bookingData.uuid}/driver").PutAsync(driverData);

        await DisplayAlert("Success", "Rider dropped off.", "Ok");

        Application.Current.MainPage = new NavigationPage(new DriverHomepage());
    }

    private async void MonitorLocation() {
        while (true) {
            await UpdateLocation();
            await Task.Delay(10000);
        }
    }

    private async Task UpdateLocation() {
        DriverData driverData = new DriverData();
        Location driverLocation = await GetUserLocation();
        driverData.currentLat = driverLocation.Latitude;
        driverData.currentLong = driverLocation.Longitude;

        map.Pins.Clear();
        AddPin(map, driverLocation, "You");
        AddPin(map, destination, "Destination");

        if (!pickedUp) {
            AddPin(map, origin, "Origin");
        }

        await firebaseClient.Child($"Bookings/{bookingData.uuid}/driver").PutAsync(driverData);
    }

    private async void OnOpenChatClicked(object sender, EventArgs e) {
        ChatFrame.IsVisible = true;
    }

    private async void OnCloseChatClicked(object sender, EventArgs e) {
        ChatFrame.IsVisible = false;
    }

    public void ListenToChat() {
        firebaseClient
            .Child($"Chats/{bookingData.uuid}")
            .AsObservable<ChatData>()
            .Subscribe(item => {
                if (item.Object != null) {
                    MainThread.BeginInvokeOnMainThread(async () => {
                        // Add the new chat to the ChatLog collection
                        var chatData = item.Object as ChatData;

                        if (chatData.sender == "driver") {
                            chatData.textColor = Color.FromHex("#000");
                            chatData.textAlign = TextAlignment.End;
                            chatData.backgroundColor = Color.FromHex("#d3d3d3");
                            chatData.horizontalOptions = LayoutOptions.End;
                            chatData.margin = new Thickness(40, 5, 10, 5);
                        } else if (chatData.sender == "rider") {
                            chatData.textColor = Color.FromHex("#fff");
                            chatData.textAlign = TextAlignment.Start;
                            chatData.backgroundColor = Color.FromHex("#000");
                            chatData.horizontalOptions = LayoutOptions.Start;
                            chatData.margin = new Thickness(10, 5, 40, 5);
                        }

                        ChatLog.Add(chatData);
                    });
                }
            });
    }

    private async void OnSendClicked(object sender, EventArgs e) {
        string dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

        var data = new ChatData {
            sender = "driver",
            message = MessageEntry.Text
        };

        await firebaseClient.Child("Chats").Child(bookingData.uuid).Child(dateString).PutAsync(data);
    }
}