namespace artshare_server.Core.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? ArtworkId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Account? Customer { get; set; }
        public virtual Artwork? Artwork { get; set; }
    }
}