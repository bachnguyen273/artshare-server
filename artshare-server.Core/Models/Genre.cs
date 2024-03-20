namespace artshare_server.Core.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public ICollection<Artwork>? Artworks { get; set; }
    }
}