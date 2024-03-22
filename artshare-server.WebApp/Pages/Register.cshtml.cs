using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace artshare_server.WebApp.Pages
{
    public class RegisterModel : PageModel
    {
		[BindProperty]
		public AccountRegisterViewModel AccountRegisterViewModel { get; set; }
		private HttpClient _httpClient;

		public RegisterModel()
		{
			_httpClient = new HttpClient();
		}
		public async Task<IActionResult> OnPost()
		{
			IConfiguration config = new ConfigurationBuilder()
									   .SetBasePath(Directory.GetCurrentDirectory())
									   .AddJsonFile("appsettings.json", true, true)
									   .Build();
			string apiUrl = config["API_URL"];
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var requestBody = new
			{
				Email = AccountRegisterViewModel.Email,
				Password = AccountRegisterViewModel.Password,
				UserName = AccountRegisterViewModel.UserName,
				FullName = AccountRegisterViewModel.FullName,
				PhoneNumber = AccountRegisterViewModel.PhoneNumber,
				Role = AccountRegisterViewModel.Role
			};

			var jsonBody = JsonConvert.SerializeObject(requestBody);
			var request = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/Auth/Register");
			request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

			// Send the request
			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				return RedirectToPage("./Login");
			}
			else
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				dynamic responseObject = JObject.Parse(responseContent);
				if (responseObject != null && responseObject.ContainsKey("message"))
				{
					TempData["ErrorMessage"] = responseObject["message"];
				}
			}

			return Page();
		}
	}
}
