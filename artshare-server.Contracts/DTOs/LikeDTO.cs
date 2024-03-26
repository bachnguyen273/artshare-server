using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class LikeDTO
    {
        public int AccountId { get; set; }
        public int ArtworkId { get; set; }
        public bool? IsLike { get; set; }
    }

    public class CreateLikeDTO : LikeDTO
    {

    }

    public class GetLikeDTO : LikeDTO
    {
        public int LikeId { get; set; }
    }

    public class UpdateLikeDTO : LikeDTO
    {

    }
}
