using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace artshare_server.WebApp.ViewModels
{
	public class AccountViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
