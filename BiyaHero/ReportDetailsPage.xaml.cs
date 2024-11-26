using Microsoft.Maui.Controls;

namespace BiyaHero
{
    public partial class ReportDetailsPage : ContentPage
    {
        public ReportDetailsPage(ReportViewModel report)
        {
            InitializeComponent();
            BindingContext = report;
        }
    }
}
