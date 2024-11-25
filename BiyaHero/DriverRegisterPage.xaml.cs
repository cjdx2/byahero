using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using BiyaHero.Models;
using BiyaHero.Services;

namespace BiyaHero
{
    public partial class DriverRegisterPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private string _photoFilePath; // Path to the selected photo

        public DriverRegisterPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        private byte[] _photoBytes; // To store the photo as a byte array

        private async Task SelectPhotoButton_Clicked()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    _photoBytes = memoryStream.ToArray();

                    // Display the selected photo
                    SelectedPhoto.Source = ImageSource.FromStream(() => new MemoryStream(_photoBytes));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while selecting the photo: {ex.Message}", "OK");
            }
        }

        private async Task RegisterButton_Clicked()
        {
            if (!AgreeCheckBox.IsChecked)
            {
                await DisplayAlert("Error", "Please agree to the terms and conditions.", "OK");
                return;
            }

            // Get User Input
            string email = EmailEntry.Text?.Trim() ?? string.Empty;
            string password = PasswordEntry.Text ?? string.Empty;
            string username = UsernameEntry.Text ?? string.Empty;
            string firstName = FnameEntry.Text ?? string.Empty;
            string lastName = LnameEntry.Text ?? string.Empty;
            string vehicleType = VehicleTypeEntry.Text ?? string.Empty;
            string vehiclePlateNumber = VehiclePlateNumberEntry.Text ?? string.Empty;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(vehicleType) || string.IsNullOrEmpty(vehiclePlateNumber) || _photoBytes == null)
            {
                await DisplayAlert("Error", "All fields, including a photo, are required.", "OK");
                return;
            }

            try
            {
                // Check if email already exists
                var existingUser = await _databaseService.GetUserByEmailAsync(email);
                if (existingUser != null)
                {
                    await DisplayAlert("Error", "Email already exists!", "OK");
                    return;
                }

                // Create new driver
                var newDriver = new Driver
                {
                    Username = username,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password,
                    VehicleType = vehicleType,
                    VehiclePlateNumber = vehiclePlateNumber,
                    Photo = _photoBytes,
                    CreatedAt = DateTime.UtcNow
                };

                await _databaseService.AddDriverAsync(newDriver);
                await DisplayAlert("Success", "Driver registered successfully!", "OK");

                ClearForm();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void ClearForm()
        {
            UsernameEntry.Text = string.Empty;
            FnameEntry.Text = string.Empty;
            LnameEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            VehicleTypeEntry.Text = string.Empty;
            VehiclePlateNumberEntry.Text = string.Empty;
            AgreeCheckBox.IsChecked = false;
            SelectedPhoto.Source = null; // Clear the photo preview
            _photoFilePath = null;
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await RegisterButton_Clicked();
        }

        private async void OnSelectPhotoButtonClicked(object sender, EventArgs e)
        {
            await SelectPhotoButton_Clicked();
        }
    }
}
