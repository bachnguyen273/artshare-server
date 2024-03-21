using artshare_server.WebApp.Helpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace artshare_server.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IEnumerable<dynamic> Artworks { get; set; }
        public List<dynamic> Genres { get; set; }
        public Pager Pager { get; set; }
        public int? GenreID { get; set; }
        public string Search { get; set; }

        private const int pageSize = 5;

        //public async Task<IActionResult> OnGetAsync() => await LoadData();
        //public async Task<IActionResult> OnPostAsync(string searchString, int pageNumber, string? selectedGenreId)
        //{
        //	await LoadData(searchString, pageNumber, selectedGenreId);
        //	return Page();
        //}


        //private async Task<IActionResult> LoadData(string? searchString = null, int? pageNumber = null, string? selectedGenreId = null)
        //{
        //    IConfiguration config = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", true, true)
        //        .Build();

        //    string apiUrl = config["API_URL"];
        //    try
        //    {
        //        pageNumber = pageNumber == null ? 1 : pageNumber;
        //        string artworkUrl = $"{apiUrl}/Artwork/GetArtworks?PageNumber={pageNumber}&PageSize=8&ArtworkStatus=0";
        //        string genreUrl = $"{apiUrl}/Genre/GetGenres";
        //        using (var httpClient = new HttpClient())
        //        {
        //            if (!string.IsNullOrEmpty(searchString))
        //            {
        //                artworkUrl += $"&Title={searchString}";
        //            }
        //            if (!string.IsNullOrEmpty(selectedGenreId))
        //            {
        //                artworkUrl += $"&GenreId={selectedGenreId}";
        //            }
        //            HttpResponseMessage artworkResponseMessage = await httpClient.GetAsync(artworkUrl);
        //            HttpResponseMessage genreResponseMessage = await httpClient.GetAsync(genreUrl);
        //            artworkResponseMessage.EnsureSuccessStatusCode();
        //            genreResponseMessage.EnsureSuccessStatusCode();
        //            string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
        //            string genreContent = await genreResponseMessage.Content.ReadAsStringAsync();
        //            dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
        //            dynamic genreObject = JsonConvert.DeserializeObject(genreContent);
        //            Artworks = new List<dynamic>(artworkObject.data.artwork.items);
        //            Genres = new List<dynamic>(genreObject.items);

        //            return Page();
        //        }
        //    }
        //    catch (HttpRequestException)
        //    {
        //        // Handle any errors from the API request
        //        return NotFound();
        //    }
        //}

        public async Task<IActionResult> OnGetAsync() => await LoadData();
        public async Task<IActionResult> OnPostAsync(int? id = null)
        {
            await LoadData(id);
            GenreID = id;            
            return Page();
        }
        public async Task<IActionResult> OnPostSearch()
        {
            Search = Request.Form["SearchValue"];
            await LoadData();
            return Page();
        }

        private async Task<IActionResult> LoadData(int? id = null)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            string apiUrl = config["API_URL"];
            string artworkUrl = $"{apiUrl}/Artwork/GetArtworks";
            string genreUrl = $"{apiUrl}/Genre/GetGenres";
            string createrUrl = $"{apiUrl}/Account/";
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    artworkUrl += $"?GenreId={id}";
                }
                else if (!HttpContext.Request.Query["genre"].IsNullOrEmpty())
                {
                    artworkUrl += $"?GenreId={HttpContext.Request.Query["genre"]}";
                }
                else if (!Search.IsNullOrEmpty())
                {
                    artworkUrl += $"?Title={Search}";
                }
                else if (!HttpContext.Request.Query["searchVal"].IsNullOrEmpty())
                {
                    artworkUrl += $"?Title={HttpContext.Request.Query["searchVal"]}";
                }
                HttpResponseMessage artworkResponseMessage = await httpClient.GetAsync(artworkUrl);
                artworkResponseMessage.EnsureSuccessStatusCode();
                string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
                dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
                IEnumerable<dynamic> listArtwork = new List<dynamic>(artworkObject.data.artwork);
                //paging artwork
                int page;
                if (HttpContext.Request.Query["page"].IsNullOrEmpty())
                {
                    page = 1;
                }
                else
                {
                    page = int.Parse(HttpContext.Request.Query["page"]);
                }
                if (page < 1)
                {
                    page = 1;
                }
                int recs = listArtwork.Count();
                Pager = new Pager(recs, page, pageSize);
                Artworks = listArtwork.Skip((page - 1) * pageSize).Take(Pager.PageSize);


                HttpResponseMessage genreResponseMessage = await httpClient.GetAsync(genreUrl);
                genreResponseMessage.EnsureSuccessStatusCode();
                string genreContent = await genreResponseMessage.Content.ReadAsStringAsync();
                dynamic genreObject = JsonConvert.DeserializeObject(genreContent);
                Genres = new List<dynamic>(genreObject.items);
            }
            return Page();
        }
    }
}
