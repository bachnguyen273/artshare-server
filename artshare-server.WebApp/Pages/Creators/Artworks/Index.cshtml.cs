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
        public List<dynamic> Artworks { get; set; }
        public List<dynamic> Genres { get; set; }
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

        public async Task<IActionResult> OnPostAsync(int? id = null)
        {
            await LoadData(id);
            return Page();
        }

        private async Task<IActionResult> LoadData(int? id = null)
        {
            var jwtToken = HttpContext.Session.GetString("JWTToken");
            // Decode the token
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;
            var accountId = tokenS.Claims.First(claim => claim.Type == "AccountId").Value;

            string artworkUrl = $"{_apiURL}/Artwork/GetArtworks?CreatorId={accountId}";

            if (!string.IsNullOrEmpty(id.ToString()))
            {
                artworkUrl += $"&GenreId={id}";
            }
            HttpResponseMessage artworkResponseMessage = await _httpClient.GetAsync(artworkUrl);
            artworkResponseMessage.EnsureSuccessStatusCode();
            string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
            dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
            Artworks = new List<dynamic>(artworkObject.data.artwork.items);

            string genreUrl = $"{_apiURL}/Genre/GetGenres";
            HttpResponseMessage genreResponseMessage = await _httpClient.GetAsync(genreUrl);
            genreResponseMessage.EnsureSuccessStatusCode();
            string genreContent = await genreResponseMessage.Content.ReadAsStringAsync();
            dynamic genreObject = JsonConvert.DeserializeObject(genreContent);
            Genres = new List<dynamic>(genreObject.items);

            return Page();
        }
    }
}