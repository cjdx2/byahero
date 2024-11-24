using SQLite;

namespace BiyaHero
{
    public class Report
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ReportContent { get; set; }
        public DateTime ReportDate { get; set; }
        public string UserEmail { get; set; }  // Assuming you want to track the user's email for the report
    }
}
