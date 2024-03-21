using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace artshare_server.WebApp.Pages
{
    public class ArtworkDetailModel : PageModel
    {
        public dynamic ArtWorkDetail { get; set; }

        private HttpClient _httpClient;
        public ArtworkDetailModel() {
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> OnGet(int id)
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();
            string apiUrl = config["API_URL"];

            HttpResponseMessage artworkResponseMessage = await _httpClient.GetAsync($"{apiUrl}/Artwork/GetArtworkById?id=1");
            artworkResponseMessage.EnsureSuccessStatusCode();
            string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
            dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
            ArtWorkDetail = artworkObject.data.artwork;
            return Page();
        }
    }
}
