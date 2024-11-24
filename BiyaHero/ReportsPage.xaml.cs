using Microsoft.Maui.Controls;

namespace BiyaHero
{
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
            LoadReports();
        }

        // Sample data model for reports
        public class Report
        {
            public string ReportTitle { get; set; }
            public string ReportType { get; set; } // Can be "User" or "Driver"
            public string ReportDate { get; set; }
        }

        // Sample method to load reports
        private void LoadReports()
        {
            var reports = new List<Report>
            {
                new Report { ReportTitle = "User Feedback on Ride", ReportType = "User", ReportDate = "2024-11-24" },
                new Report { ReportTitle = "Driver Performance Report", ReportType = "Driver", ReportDate = "2024-11-23" },
                new Report { ReportTitle = "User Ride History Report", ReportType = "User", ReportDate = "2024-11-22" },
                new Report { ReportTitle = "Driver Ride Completion Rate", ReportType = "Driver", ReportDate = "2024-11-21" }
            };

            // Bind the sample reports to the ListView
            ReportsListView.ItemsSource = reports;
        }
    }
}
