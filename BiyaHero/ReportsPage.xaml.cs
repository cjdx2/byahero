using Microsoft.Maui.Controls;
using BiyaHero.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiyaHero
{
    public partial class ReportsPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public ReportsPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LoadReports();
        }

        private async void LoadReports()
        {
            try
            {
                // Fetch reports from the database
                var allReports = await _databaseService.GetAllReportsAsync();

                // Categorize reports
                var pendingReports = new List<ReportViewModel>();
                var resolvedReports = new List<ReportViewModel>();
                var spamReports = new List<ReportViewModel>();

                foreach (var report in allReports)
                {
                    var reportVm = new ReportViewModel
                    {
                        ReportTitle = "Report: " + report.ReportContent,
                        ReportType = "User", // Adjust as necessary
                        ReportDate = report.ReportDate.ToString("yyyy-MM-dd"),
                        UserEmail = report.UserEmail,
                        ResolveCommand = new Command(async (reportVm) => await MarkReportAsResolved(reportVm as ReportViewModel)),
                        SpamCommand = new Command(async (reportVm) => await MarkReportAsSpam(reportVm as ReportViewModel))
                    };

                    if (report.Status == "Resolved")
                        resolvedReports.Add(reportVm);
                    else if (report.Status == "Spam")
                        spamReports.Add(reportVm);
                    else
                        pendingReports.Add(reportVm);
                }

                // Bind categorized reports to ListViews
                PendingReportsListView.ItemsSource = pendingReports;
                ResolvedReportsListView.ItemsSource = resolvedReports;
                SpamReportsListView.ItemsSource = spamReports;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading reports: {ex.Message}");
            }
        }

        private async Task MarkReportAsResolved(ReportViewModel report)
        {
            if (report == null) return;

            try
            {
                // Update the report's status using the UserEmail
                await _databaseService.UpdateReportStatusAsync(report.UserEmail, "Resolved");
                Console.WriteLine($"Report(s) by {report.UserEmail} marked as Resolved");

                // Reload the reports to reflect the changes
                LoadReports();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking report as resolved: {ex.Message}");
            }
        }

        private async Task MarkReportAsSpam(ReportViewModel report)
        {
            if (report == null) return;

            try
            {
                // Update the report's status using the UserEmail
                await _databaseService.UpdateReportStatusAsync(report.UserEmail, "Spam");
                Console.WriteLine($"Report(s) by {report.UserEmail} marked as Spam");

                // Reload the reports to reflect the changes
                LoadReports();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking report as spam: {ex.Message}");
            }
        }

    }

    // ViewModel for binding to the ListView
    public class ReportViewModel
    {
        public string Id { get; set; }
        public string ReportTitle { get; set; }
        public string ReportType { get; set; }
        public string ReportDate { get; set; }
        public string UserEmail { get; set; }

        public Command ResolveCommand { get; set; }
        public Command SpamCommand { get; set; }
        public Command ViewDetailsCommand { get; set; }

        public ReportViewModel()
        {
            ViewDetailsCommand = new Command(async () => await ViewDetails());
        }

        private async Task ViewDetails()
        {
            // Navigate to the detailed report page
            await Application.Current.MainPage.Navigation.PushAsync(new ReportDetailsPage(this));
        }
    }
}
