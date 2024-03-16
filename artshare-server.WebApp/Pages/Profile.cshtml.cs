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

		private HttpClient _httpClient;

        public ProfileModel()
        {
			_httpClient = new HttpClient();
        }
        public async Task OnGetAsync()
        {
            string username = HttpContext.Request.Query["username"];
            //string currentUser = HttpContext.Session.GetString("Username");


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
				ProfileViewModel1 = JsonConvert.DeserializeObject<ProfileViewModel>(responseContent);
			}
		}
    }
}
