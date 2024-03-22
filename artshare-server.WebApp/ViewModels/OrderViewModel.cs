namespace artshare_server.WebApp.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
    }
}
