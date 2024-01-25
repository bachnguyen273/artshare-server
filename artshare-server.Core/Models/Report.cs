using artshare_server.Core.Enums;

namespace artshare_server.Core.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public int AccountId { get; set; }
        public int ArtworkId { get; set; }
        public string Content { get; set; }
        public ReportCategory Category { get; set; }
        public DateTime ReportDate { get; set; }
        public ReportStatus Status { get; set; }
        public Account Account { get; set; }
        public Artwork Artwork { get; set; }
    }
}