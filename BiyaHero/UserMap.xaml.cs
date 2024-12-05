using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Devices.Sensors;

using Map = Microsoft.Maui.Controls.Maps.Map;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace BiyaHero;

public partial class UserMap : ContentPage {
    public string pinSelection = "origin";
    private Pin originPin;  // Track the origin pin
    private Pin destinationPin;  // Track the destination pin
    public string originAddress;
    public string destinationAddress;
    double currentPrice;
    string paymentMethod;

    public UserMap(string payment) {
        InitializeComponent();

        paymentMethod = payment;

        // Handle tap event to add or move a pin
        map.MapClicked += OnMapClicked;

        // Load the user's current location
        LoadUserLocation(map);
    }

    // Method to load the user's current location
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
                AddOriginPin(map, userPosition);
                SetAddress("origin", userPosition);
            } else {
                // Handle the case when location is not available
                await DisplayAlert("Error", "Unable to retrieve current location.", "OK");
            }
        } catch (Exception ex) {
            // Handle any exceptions (e.g., no permissions or location service issues)
            await DisplayAlert("Error", "An error occurred while retrieving the location: " + ex.Message, "OK");
        }
    }

    // Method to add the origin pin
    private void AddOriginPin(Map map, Location location) {
        originPin = new Pin {
            Label = "Origin",
            Type = PinType.Place,
            Location = location
        };
        map.Pins.Add(originPin);  // Add origin pin to map
    }

    // Method to add the destination pin
    private void AddDestinationPin(Map map, Location location) {
        destinationPin = new Pin {
            Label = "Destination",
            Type = PinType.Place,
            Location = location
        };
        map.Pins.Add(destinationPin);  // Add destination pin to map
    }

    private async Task<string> GetAddressFromLocation(double latitude, double longitude) {
        string url = $"https://nominatim.openstreetmap.org/reverse?lat={latitude}&lon={longitude}&format=json&addressdetails=1";

        using (HttpClient client = new HttpClient()) {
            var response = await client.GetStringAsync(url);
            var json = JObject.Parse(response);

            var address = json["display_name"]?.ToString();
            return address ?? "Unknown Location";
        }
    }

    // Method to handle map click event
    private async void OnMapClicked(object sender, MapClickedEventArgs e) {
        if (pinSelection == "origin") {
            // Move the origin pin if origin is selected
            if (originPin != null) {
                map.Pins.Remove(originPin);  // Remove the old pin
                originPin.Location = e.Location;  // Update the location
                map.Pins.Add(originPin);  // Re-add the updated pin

                SetAddress("origin", e.Location);
            }
        } else if (pinSelection == "destination") {
            // Add the destination pin if destination is selected
            if (destinationPin == null) {
                AddDestinationPin((Map)sender, e.Location);
            } else {
                map.Pins.Remove(destinationPin);  // Remove the old pin
                destinationPin.Location = e.Location;  // Update the location
                map.Pins.Add(destinationPin);  // Re-add the updated pin
            }

            SetAddress("destination", e.Location);
        }

        // Check if both origin and destination pins are set
        if (originPin != null && destinationPin != null) {
            // Call the method to calculate the distance
            CalculateDistance();
        }
    }

    private async void SetAddress(string type, Location location) {
        var address = await GetAddressFromLocation(location.Latitude, location.Longitude);

        if (type == "origin") {
            originButton.Text = address; // Set the button text to the address
            originAddress = address;
        } else {
            destinationButton.Text = address; // Set the button text to the address
            destinationAddress = address;
        }
    }

    // New method to calculate and display the distance
    private void CalculateDistance() {
        var originLocation = new Location(originPin.Location.Latitude, originPin.Location.Longitude);
        var destinationLocation = new Location(destinationPin.Location.Latitude, destinationPin.Location.Longitude);

        // Get the distance between the origin and destination in kilometers (or miles)
        double distance = originLocation.CalculateDistance(destinationLocation, DistanceUnits.Kilometers); // You can change to Miles if needed

        // Calculate the price based on distance
        double pricePerKm = 25;
        double price = distance * pricePerKm;
        double roundedPrice = Math.Round(price, 2);

        currentPrice = roundedPrice;

        // Display the distance and price in the debug console
        Debug.WriteLine($"Distance between Origin and Destination: {distance} km");
        Debug.WriteLine($"Price for the trip: ${price}");

        // Update UI with the calculated distance and price
        distanceLabel.Text = $"Distance: {distance} km";
        priceLabel.Text = $"Price: P{price:F2}"; // Format price to two decimal places
    }

    // Method to handle origin button click
    private void OnOriginButtonClicked(object sender, EventArgs e) {
        // Change the background and text color of the Origin button
        originButton.BackgroundColor = Color.FromHex("#0f9f72");
        originButton.TextColor = Color.FromHex("#fff");

        // Reset the Destination button to its original style
        destinationButton.BackgroundColor = Color.FromHex("#fff");
        destinationButton.TextColor = Color.FromHex("#000");

        pinSelection = "origin";
    }

    // Method to handle destination button click
    private void OnDestinationButtonClicked(object sender, EventArgs e) {
        // Change the background and text color of the Destination button
        destinationButton.BackgroundColor = Color.FromHex("#0f9f72");
        destinationButton.TextColor = Color.FromHex("#fff");

        // Reset the Origin button to its original style
        originButton.BackgroundColor = Color.FromHex("#fff");
        originButton.TextColor = Color.FromHex("#000");

        pinSelection = "destination";
    }

    private async void OnBookClicked(object sender, EventArgs e) {
        if (destinationPin == null) {
            await DisplayAlert("Destination not set.", "Please select your destination location.", "Ok");
            return;
        }

        var originLocation = originPin.Location;
        var destinationLocation = destinationPin.Location;

        // Navigate to WaitingForDriverPage and pass the coordinates
        await Navigation.PushAsync(new WaitingForDriver(originLocation, destinationLocation, originAddress, destinationAddress, currentPrice, paymentMethod));
    }
}
