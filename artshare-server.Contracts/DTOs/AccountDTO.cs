using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace artshare_server.ApiModels.DTOs
{
    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class AccountDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public required string Status { get; set; }
        public required string UserName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class CreateAccountDTO : AccountDTO
    {
       
    }

    public class UpdateAccountDTO : AccountDTO
    {
        public int AccountId { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class GetAccountDTO : AccountDTO
    {
        public int AccountId { get; set; }
        public DateTime JoinDate { get; set; }
        public IEnumerable<GetWatermarkDTO>? Watermarks { get; set; }
        public IEnumerable<GetArtworkDTO>? Artworks { get; set; }
        public IEnumerable<GetOrderDTO>? Orders { get; set; }
        public IEnumerable<GetLikeDTO>? Likes { get; set; }
        public IEnumerable<GetCommentDTO>? Comments { get; set; }
        public IEnumerable<GetReportDTO>? Reports { get; set; }
        public string? AvatarUrl { get; set; }
    }


}
