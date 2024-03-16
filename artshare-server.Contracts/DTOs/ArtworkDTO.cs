using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace artshare_server.ApiModels.DTOs
{
    public class ArtworkDTO
    {
        public int CreatorId { get; set; }
        public int? WatermarkId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public string? Status { get; set; }
 
    }

    public class CreateArtworkDTO : ArtworkDTO
    {
        public required string OriginalArtUrl { get; set; }
        public required IEnumerable<int> GenreId { get; set; }
    }

    public class GetArtworkDTO : ArtworkDTO
    {
        public int ArtworkId { get; set; }
        public string? OriginalArtUrl { get; set; }
        public string? WatermarkedArtUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int CommentCount { get; set; }
        public IEnumerable<GetOrderDetailDTO>? OrderDetails { get; set; }
        public IEnumerable<GetLikeDTO>? Likes { get; set; }
        public IEnumerable<GetCommentDTO> Comments { get; set; }
        public IEnumerable<GetReportDTO> Reports { get; set; }
        public required IEnumerable<GetGenreDTO> Genres { get; set; }
    }

    public class UpdateArtworkDTO : ArtworkDTO
    {

    }
}

