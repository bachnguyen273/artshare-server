using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.ApiModels.DTOs
{
    public class CommentDTO
    {
        public int CommenterId { get; set; }
        public int ArtworkId { get; set; }
        public required string Content { get; set; }
        public DateTime PostDate { get; set; }
    }

    public class CreateCommentDTO : CommentDTO
    {

    }

    public class GetCommentDTO : CommentDTO
    {
        public int CommentId { get; set; }
        public GetAccountDTO Commenter { get; set; }
    }

    public class UpdateCommentDTO : CommentDTO
    {

    }
}
