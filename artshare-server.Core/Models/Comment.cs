namespace artshare_server.Core.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int CommenterId { get; set; }
        public int ArtworkId { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public Account Commenter { get; set; }
        public Artwork Artwork { get; set; }
    }
}