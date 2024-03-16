using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace artshare_server.WebApp.Pages.Creators.Artworks
{
    public class IndexModel : PageModel
    {
        [BindProperty]
		public List<dynamic> Artworks { get; set; }
		public List<dynamic> Genres { get; set; }
        public List<dynamic> Watermarks { get; set; }
		public string SearchString { get; set; }
		public int PageNumber { get; set; }
        private readonly HttpClient _httpClient;
        private readonly string _apiURL;

		public IndexModel()
		{
			_httpClient = new HttpClient();
			IConfiguration config = new ConfigurationBuilder()
						  .SetBasePath(Directory.GetCurrentDirectory())
						  .AddJsonFile("appsettings.json", true, true)
		                  .Build();

			_apiURL = config["API_URL"];
		}

		public async Task<IActionResult> OnGetAsync() => await LoadData();
        public async Task<IActionResult> OnPostAsync(string searchString)
        {
            await LoadData(searchString);
            return Page();
        }

        private async Task<IActionResult> LoadData(string? searchString = null, string? pageNumber = null)
        {
           
            try
            {
                var jwtToken = HttpContext.Session.GetString("JWTToken");
                // Decode the token
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;
                var accountId = tokenS.Claims.First(claim => claim.Type == "AccountId").Value;
                await LoadArtworkByCreatorIAsyncd(accountId);
                await LoadGenresAsync();
                return Page();
            }
            catch (HttpRequestException)
            {
                // Handle any errors from the API request
                return NotFound();
            }
        }

        private async Task LoadArtworkByCreatorIAsyncd(string creatorId, string searchString = null)
        {
			string artworkUrl = _apiURL + "/Artwork/GetArtworks?CreatorId=" + creatorId;
			if (searchString != null)
            {
                
            }
			HttpResponseMessage artworkResponseMessage = await _httpClient.GetAsync(artworkUrl);
			artworkResponseMessage.EnsureSuccessStatusCode();
			string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
			dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
			Artworks = new List<dynamic>(artworkObject.data.artwork.items);
		}

        private async Task LoadGenresAsync()
        {
			string genreUrl = _apiURL + "/Genre/GetGenres";
			HttpResponseMessage genreResponseMessage = await _httpClient.GetAsync(genreUrl);
			genreResponseMessage.EnsureSuccessStatusCode();
			string genreContent = await genreResponseMessage.Content.ReadAsStringAsync();
			dynamic genreObject = JsonConvert.DeserializeObject(genreContent);
			Genres = new List<dynamic>(genreObject.items);
		}
    }
}
