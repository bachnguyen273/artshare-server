using artshare_server.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Core.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinDate { get; set; }
        public AccountRole Role { get; set; }
        public AccountStatus Status { get; set; }
        public ICollection<Watermark> Watermarks { get; set; }
        public ICollection<Artwork> Artworks { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
