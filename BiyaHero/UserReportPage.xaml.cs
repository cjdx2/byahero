using Microsoft.Maui.Controls;
using System;
using BiyaHero.Models;
using BiyaHero.Services;

namespace BiyaHero
{
    public partial class UserReportPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private string _userEmail;  // Store the user email

        public UserReportPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LoadUserEmailAsync();  // Load the email when the page is created
        }

        // Load the user's email from the database
        private async void LoadUserEmailAsync()
        {
            var user = await _databaseService.GetCurrentUserAsync();  // Fetch the current user (assuming it's the first user or you adjust the logic)

            if (user != null)
            {
                _userEmail = user.Email;  // Set the email from the database
            }
            else
            {
                // Handle the case where no user is found (e.g., redirect to login)
                await DisplayAlert("Error", "No user found. Please log in again.", "OK");
            }
        }

        // Event handler for the Submit button click
        private async void OnSubmitReportClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_userEmail))
            {
                await DisplayAlert("Error", "User email not found. Please log in again.", "OK");
                return;
            }

            string reportContent = ReportEditor.Text?.Trim();

            if (string.IsNullOrEmpty(reportContent))
            {
                await DisplayAlert("Error", "Please enter a report before submitting.", "OK");
                return;
            }

            // Get the Philippine Standard Time (PST) TimeZoneInfo object
            var philippinesTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");

            // Convert the current UTC time to Philippine time
            DateTime philippinesTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, philippinesTimeZone);

            var report = new Report
            {
                ReportContent = reportContent,
                ReportDate = philippinesTime, // Use the Philippine time
                UserEmail = _userEmail // Use the fetched email
            };

            // Debugging step: Log the report details before saving
            Console.WriteLine($"Submitting Report: {reportContent}");

            // Save the report in the SQLite database
            var result = await _databaseService.AddReportAsync(report);

            if (result > 0)
            {
                // Clear the text editor after submission
                ReportEditor.Text = string.Empty;

                // Notify the user that the report has been submitted
                await DisplayAlert("Success", "Your report has been submitted successfully.", "OK");
            }
            else
            {
                // If the insertion failed
                await DisplayAlert("Error", "There was an issue submitting your report. Please try again.", "OK");
            }
        }

    }
}

