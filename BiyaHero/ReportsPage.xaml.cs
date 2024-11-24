using Microsoft.Maui.Controls;
using BiyaHero.Services;
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

        // Sample method to load reports from the database
        private async void LoadReports()
        {
            try
            {
                // Fetch reports from the database
                var reportsFromDb = await _databaseService.GetAllReportsAsync();

                // Convert the reports into a format suitable for the UI (you can change ReportType based on your logic)
                var reports = new List<ReportViewModel>();

                foreach (var report in reportsFromDb)
                {
                    reports.Add(new ReportViewModel
                    {
                        ReportTitle = "Report: " + report.ReportContent,
                        ReportType = "User",  // Modify this logic as needed (e.g., User or Driver)
                        ReportDate = report.ReportDate.ToString("yyyy-MM-dd")
                    });
                }

                // Bind the fetched reports to the ListView
                ReportsListView.ItemsSource = reports;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading reports: {ex.Message}");
            }
        }
    }

    // A ViewModel to match the format of your ListView
    public class ReportViewModel
    {
        public string ReportTitle { get; set; }
        public string ReportType { get; set; }
        public string ReportDate { get; set; }
    }
}
