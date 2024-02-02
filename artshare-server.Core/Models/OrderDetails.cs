namespace artshare_server.Core.Models
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public int ArtworkId { get; set; }
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; }
        public Artwork Artwork { get; set; }
    }
}