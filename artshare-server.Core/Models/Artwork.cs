using artshare_server.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Core.Models
{
    public class Artwork
    {
        public int ArtworkId { get; set; }
        public int CreatorId { get; set; }
        public int WatermarkId { get; set; }
        public string Title { get; set; }
        public string OriginalArtUrl { get; set; }
        public string WatermarkedArtUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int CommentCount { get; set; }
        public ArtworkStatus Status { get; set; }
        public Account Creator { get; set; }
        public Watermark Watermark { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
