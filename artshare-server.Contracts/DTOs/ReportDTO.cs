using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class ReportDTO
    {

        public int AccountId { get; set; }
        public int ArtworkId { get; set; }
        public string Content { get; set; }
        public String Category { get; set; }
    }
}
