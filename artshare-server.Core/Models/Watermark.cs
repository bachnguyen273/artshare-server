using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Core.Models
{
    public class Watermark
    {
        public int WatermarkId { get; set; }
        public int CreatorId { get; set; }
        public string ImageUrl { get; set; }
        public Account Creator { get; set; }
        public ICollection<Artwork> Artworks { get; set; }
    }
}
