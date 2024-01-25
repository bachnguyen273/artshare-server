using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Core.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public int AccountId { get; set; }
        public int ArtworkId { get; set; }
        public bool IsLike { get; set; }
        public Account Account { get; set; }
        public Artwork Artwork { get; set; }
    }
}
