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
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int GenreId { get; set; }
        public required string OriginalArtUrl { get; set; }
        public required string WatermarkedArtUrl { get; set; }
    }

    public class CreateArtworkDTO : ArtworkDTO
    {
        [JsonIgnore]
        public string? Status { get; set; }
    }

    public class GetArtworkDTO : ArtworkDTO
    {
        public int ArtworkId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int CommentCount { get; set; }
        public string? ArtworkStatus { get; set; }
        public IEnumerable<GetLikeDTO>? Likes { get; set; }
        public IEnumerable<GetCommentDTO> Comments { get; set; }
        public IEnumerable<GetReportDTO> Reports { get; set; }
        public GetAccountDTO Creator { get; set; }
    }

    public class UpdateArtworkDTO : ArtworkDTO
    {
        [JsonIgnore]
        public string? Status { get; set; }
    }
}

