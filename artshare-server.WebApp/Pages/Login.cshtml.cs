using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace artshare_server.WebApp.Pages
{
    public class LoginModel : PageModel
    {
		[BindProperty]
        public AccountViewModel AccountViewModel { get; set; }
        private HttpClient _httpClient;

		public LoginModel()
		{
			_httpClient = new HttpClient();
		}
		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var requestBody = new
			{
				Email = AccountViewModel.Email,
				Password = AccountViewModel.Password
			};

			var jsonBody = JsonConvert.SerializeObject(requestBody);
			var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5292/api/Auth/Login");
			request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

			// Send the request
			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				return RedirectToPage("./Creators/Index");
			}
			return Page();
		}
	}
}
