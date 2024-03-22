namespace artshare_server.WebApp.ViewModels
{
    public class CommentViewModel
    {
        public int CommenterId { get; set; }
        public int ArtworkId { get; set; }
        public required string Content { get; set; }
        public DateTime PostDate { get; set; }
    }
}
