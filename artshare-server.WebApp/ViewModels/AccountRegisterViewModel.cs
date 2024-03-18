using System.ComponentModel.DataAnnotations;

namespace artshare_server.WebApp.ViewModels
{
    public class AccountRegisterViewModel
    {
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare(("Password"), ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		public string FullName { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		public string Role { get; set; }
	}
}
