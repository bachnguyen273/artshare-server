using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class WatermarkDTO
    {      
        public int CreatorId { get; set; }
    }

    public class CreateWatermarkDTO : WatermarkDTO
    {
        public required IFormFile WatermarkFile { get; set; }
    }

    public class GetWatermarkDTO : WatermarkDTO
    {
        public int WatermarkId { get; set; }
        public required string WatermarkUrl { get; set; }
    }

    public class UpdateWatermarkDTO : WatermarkDTO
    {
        public required IFormFile WatermarkFile { get; set; }
    }
}
