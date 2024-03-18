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
		public async Task<IActionResult> OnPostAsync(string searchString, int pageNumber, string? selectedGenreId)
		{
			await LoadData(searchString, pageNumber, selectedGenreId);
			return Page();
		}

		private async Task<IActionResult> LoadData(string? searchString = null, int? pageNumber = null, string? selectedGenreId = null)
        {
           
            try
            {
                var jwtToken = HttpContext.Session.GetString("JWTToken");
                // Decode the token
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;
                var accountId = tokenS.Claims.First(claim => claim.Type == "AccountId").Value;

				pageNumber = pageNumber == null ? 1 : pageNumber;
				string artworkUrl = _apiURL + $"/Artwork/GetArtworks?CreatorId={accountId}&PageNumber={pageNumber}&PageSize=8";
				if (!string.IsNullOrEmpty(searchString))
				{
					artworkUrl += $"&Title={searchString}";
				}
				if (!string.IsNullOrEmpty(selectedGenreId))
				{
					artworkUrl += $"&GenreId={selectedGenreId}";
				}
				HttpResponseMessage artworkResponseMessage = await _httpClient.GetAsync(artworkUrl);
				artworkResponseMessage.EnsureSuccessStatusCode();
				string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
				dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
				Artworks = new List<dynamic>(artworkObject.data.artwork.items);

				string genreUrl = _apiURL + "/Genre/GetGenres";
				HttpResponseMessage genreResponseMessage = await _httpClient.GetAsync(genreUrl);
				genreResponseMessage.EnsureSuccessStatusCode();
				string genreContent = await genreResponseMessage.Content.ReadAsStringAsync();
				dynamic genreObject = JsonConvert.DeserializeObject(genreContent);
				Genres = new List<dynamic>(genreObject.items);
				return Page();
            }
            catch (HttpRequestException)
            {
                // Handle any errors from the API request
                return NotFound();
            }
        }
    }
}
