using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class TopSaleArtwork
    {
        public int? CreatorId { get; set; }
        public string Title { get; set; }
        public int ArtworkCount { get; set; }
    }
}
