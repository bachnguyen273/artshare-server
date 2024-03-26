using System.ComponentModel.DataAnnotations;

namespace artshare_server.WebApp.ViewModels
{
    public class AccountRegisterViewModel
    {
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[MinLength(3, ErrorMessage = "Password must be at least 3 characters long")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm password is required")]
		[Compare(("Password"), ErrorMessage = "Passwords do not match")]
		[MinLength(3, ErrorMessage = "Password must be at least 3 characters long")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Username is required")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Full Name is required")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Phone number is required")]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Phone Number")]
		public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Role is required")]
		public string Role { get; set; }
	}
}
