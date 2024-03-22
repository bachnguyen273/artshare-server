using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages
{
    public class ArtworkDetailModel : PageModel
    {
        [BindProperty]
        public dynamic Artwork { get; set; }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await LoadData(id);
            return Page();
        }
        private async Task<IActionResult> LoadData(int? id = null)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            string apiUrl = config["API_URL"];
            string artworkUrl = $"{apiUrl}/Artwork/GetArtworkById?id={id}";
            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage artworkResponseMessage = await httpClient.GetAsync(artworkUrl);
                artworkResponseMessage.EnsureSuccessStatusCode();
                string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
                dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
                Artwork = artworkObject.data.artwork;

            }
            return Page();
        }
    }

}
