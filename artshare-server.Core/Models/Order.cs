namespace artshare_server.Core.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Account Customer { get; set; }
        public ICollection<Artwork> Artworks { get; set; }
    }
}