using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PaypalClientId { get; set; }
        public string? PaypalSercretKey { get; set; }
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
        public IEnumerable<GetArtworkDTO>? Artworks { get; set; }
        public IEnumerable<GetOrderDTO>? Orders { get; set; }
        public IEnumerable<GetLikeDTO>? Likes { get; set; }
        public IEnumerable<GetCommentDTO>? Comments { get; set; }
        public IEnumerable<GetReportDTO>? Reports { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }

    public class ProfileDTO
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? AvatarUrl { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
