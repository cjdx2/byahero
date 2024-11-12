using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using BiyaHero.Models;
using BiyaHero.Services;

namespace BiyaHero
{
    public partial class RegisterPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public RegisterPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
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

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
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

                // Create new user
                var newUser = new User
                {
                    Username = username,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password,  // Ideally, you should hash the password before storing it
                    CreatedAt = DateTime.UtcNow
                };

                await _databaseService.AddUserAsync(newUser);
                await DisplayAlert("Success", "User registered successfully!", "OK");

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
            AgreeCheckBox.IsChecked = false;
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            await RegisterButton_Clicked();
        }
    }
}
