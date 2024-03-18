using artshare_server.WebApp.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace artshare_server.WebApp.Pages
{
    public class ProfileModel : PageModel
    {
		[BindProperty]
		public ProfileViewModel? ProfileViewModel1 { get; set; }

		[BindProperty]
		public int Check {  get; set; }

		private HttpClient _httpClient;

        public ProfileModel()
        {
			_httpClient = new HttpClient();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string username = HttpContext.Request.Query["username"];

            string currentUser = HttpContext.Session.GetString("Username");
			Check = (currentUser == null) ? 0 : ((currentUser.Equals(username)) ? 1 : 0);


			IConfiguration config = new ConfigurationBuilder()
									   .SetBasePath(Directory.GetCurrentDirectory())
									   .AddJsonFile("appsettings.json", true, true)
									   .Build();
			string apiUrl = config["API_URL"];
			var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/GetAccountByUsername/{username}");
			var response = await _httpClient.SendAsync(request);
			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				var acc = JsonConvert.DeserializeObject<ProfileViewModel>(responseContent);
				if(acc.Status != "Inactive")
				{
					ProfileViewModel1 = acc;
					return Page();
				}
			}
			return NotFound();
		}
    }
}
