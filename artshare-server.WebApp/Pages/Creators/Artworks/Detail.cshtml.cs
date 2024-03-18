using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

namespace artshare_server.WebApp.Pages.Creators.Artworks
{
    public class DetailModel : PageModel
    {
        public dynamic Artwork { get; set; }
        public string Role { get; set; }
        private string _apiURL;
        private string _jwtToken;
        private HttpClient _httpClient;

        public DetailModel()
        {
            _httpClient = new HttpClient();
            IConfiguration config = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true, true)
                          .Build();

            _apiURL = config["API_URL"];
        }

        public async Task<IActionResult> OnGetAsync(int artworkId)
        {
            _jwtToken = HttpContext.Session.GetString("JWTToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            HttpResponseMessage artworkResponseMessage = await _httpClient.GetAsync(_apiURL + "/Artwork/GetArtworkById?id=" + artworkId);
            artworkResponseMessage.EnsureSuccessStatusCode();
            string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
            dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
            Artwork = artworkObject.data.artwork;
            return Page();
        }
    }
}
