using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class WatermarkCreateDTO
    {
        public int WatermarkId { get; set; }
        public int CreatorId { get; set; }
        public string WatermarkUrl { get; set; }
    }
}
