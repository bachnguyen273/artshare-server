using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace artshare_server.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<dynamic> Artworks { get; set; }
        public List<dynamic> Genres { get; set; }
        public string SearchString { get; set; }
        public int PageNumber { get; set; }


		public async Task<IActionResult> OnGetAsync() => await LoadData();
		public async Task<IActionResult> OnPostAsync(string searchString, int pageNumber)
		{
			await LoadData(searchString, pageNumber);
			return Page();
		}

        private async Task<IActionResult> LoadData(string? searchString = null, int? pageNumber = null)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            string apiUrl = config["API_URL"];
            try
            {
                pageNumber = pageNumber == null ? 1 : pageNumber;
                string artworkUrl = $"{apiUrl}/Artwork/GetArtworks?PageNumber={pageNumber}&PageSize=1";
                string genreUrl = $"{apiUrl}/Genre/GetGenres";
                using (var httpClient = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        artworkUrl += $"&Title={searchString}";
                    }
                    HttpResponseMessage artworkResponseMessage = await httpClient.GetAsync(artworkUrl);
                    HttpResponseMessage genreResponseMessage = await httpClient.GetAsync(genreUrl);
                    artworkResponseMessage.EnsureSuccessStatusCode();
                    genreResponseMessage.EnsureSuccessStatusCode();
                    string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
                    string genreContent = await genreResponseMessage.Content.ReadAsStringAsync();
                    dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
                    dynamic genreObject = JsonConvert.DeserializeObject(genreContent);
                    Artworks = new List<dynamic>(artworkObject.data.artwork.items);
                    Genres = new List<dynamic>(genreObject.items);

                    return Page();
                }
            }
            catch (HttpRequestException)
            {
                // Handle any errors from the API request
                return NotFound();
            }
        }
    }
}