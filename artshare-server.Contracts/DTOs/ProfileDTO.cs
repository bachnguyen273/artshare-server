using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace artshare_server.ApiModels.DTOs
{
    public class ProfileDTO
    {
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
