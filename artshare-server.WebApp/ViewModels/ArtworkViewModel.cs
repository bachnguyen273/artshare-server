using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [Required]
        public int CreatorId { get; set; }
		[Required(ErrorMessage = "Title is required")]
		[StringLength(50, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 50 characters")]
		public string Title { get; set; }
		[Required(ErrorMessage = "Price is required")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
		public decimal Price { get; set; }
		public required IFormFile OrginalArtworkFile { get; set; }
		[Required(ErrorMessage = "Title is required")]
		[StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters")]
		public string? Description { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public required int GenreId { get; set; }
    }

    public class UpdateArtworkViewModel
    {
        [Required]
        public int ArtworkId { get; set; }
        [Required]
        public int CreatorId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 50 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required]
        public IFormFile OrginalArtworkFile { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters")]
        public string? Description { get; set; }
        public int GenreId { get; internal set; }
    }
}
