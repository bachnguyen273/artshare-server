namespace artshare_server.WebApp.ViewModels
{
    public class ArtworkViewModel
    {
        public int ArtworkId { get; set; }
        public required string Title { get; set; }
        public required string WatermarkedArtUrl { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateArtworkViewModel
    {
        public int CreatorId { get; set; }
		public int WatermarkId { get; set; }
		public required string Title { get; set; }
        public decimal Price { get; set; }
        public required IFormFile OrginalArtworkFile { get; set; }
        public string? Description { get; set; }
        public required string Status { get; set; }
        public required IEnumerable<int> GenreId { get; set; }
    }
}
