namespace artshare_server.Core.Models
{
    public class Watermark
    {
        public int WatermarkId { get; set; }
        public int CreatorId { get; set; }
        public string WatermarkUrl { get; set; }
        public Account Creator { get; set; }
        public ICollection<Artwork> Artworks { get; set; }
    }
}