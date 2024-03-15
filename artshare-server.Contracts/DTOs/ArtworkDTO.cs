using artshare_server.Contracts;
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
        public string Title { get; set; }
        public string OriginalArtUrl { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public string? Status { get; set; }
    }

    public class CreateArtworkDTO : ArtworkDTO
    {

    }
}
