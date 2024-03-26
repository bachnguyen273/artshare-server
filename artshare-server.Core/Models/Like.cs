namespace artshare_server.Core.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public int? AccountId { get; set; }
        public int? ArtworkId { get; set; }
        public bool? IsLike { get; set; }
        public virtual Account? Account { get; set; }
        public virtual Artwork? Artwork { get; set; }
    }
}