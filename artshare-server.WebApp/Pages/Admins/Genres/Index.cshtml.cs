using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace artshare_server.WebApp.Pages.Admins.Genres
{
    public class IndexModel : PageModel
    {
		private readonly HttpClient _httpCLient;
		private string _apiURL;
		public List<dynamic> Genres { get; set; }

		public IndexModel()
		{
			_httpCLient = new HttpClient();
			IConfiguration config = new ConfigurationBuilder()
						  .SetBasePath(Directory.GetCurrentDirectory())
						  .AddJsonFile("appsettings.json", true, true)
						  .Build();

			_apiURL = config["API_URL"];
		}

		public async Task OnGet()
        {
			using (HttpResponseMessage response = await _httpCLient.GetAsync(_apiURL + "/Genre/GetGenres"))
			{
				response.EnsureSuccessStatusCode();

				string jsonResponse = await response.Content.ReadAsStringAsync();
				Genres = JsonSerializer.Deserialize<List<dynamic>>(jsonResponse);
			}
		}
    }
}
