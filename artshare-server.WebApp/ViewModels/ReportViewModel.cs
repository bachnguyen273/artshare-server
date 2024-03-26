namespace artshare_server.WebApp.ViewModels
{
    public class ReportViewModel
    {
        public int AccountId { get; set; }
        public int ArtworkId { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public DateTime ReportDate { get; set; }
        public string Status { get; set; }
    }
}
