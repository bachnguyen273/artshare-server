namespace artshare_server.WebApp.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Price { get; set; }
        public int ArtworkId { get; set; }
    }
}
