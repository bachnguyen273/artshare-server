using System.ComponentModel.DataAnnotations;

namespace artshare_server.WebApp.ViewModels
{
    public class ProfileViewModel
    {
		public int AccountId { get; set; }

        [Required(ErrorMessage ="Please Enter email..!")]
        [EmailAddress]
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
