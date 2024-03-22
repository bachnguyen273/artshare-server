using artshare_server.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace artshare_server.WebApp.Pages.Audiences.Artworks
{
    public class IndexModel : PageModel
    {
        public IEnumerable<dynamic> Artworks { get; set; }
        public List<dynamic> Genres { get; set; }
        public Pager Pager { get; set; }
        public int? GenreID { get; set; }
        public string Search { get; set; }

        private const int pageSize = 5;
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
            var jwtToken = HttpContext.Session.GetString("JWTToken");
            // Decode the token
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;
            var accountId = tokenS.Claims.First(claim => claim.Type == "AccountId").Value;

            string artworkUrl = $"{_apiURL}/Account/GetAccountById/{accountId}";

            if (!string.IsNullOrEmpty(id.ToString()))
            {
                artworkUrl += $"&GenreId={id}";
            }
            else if (!HttpContext.Request.Query["genre"].IsNullOrEmpty())
            {
                artworkUrl += $"&GenreId={HttpContext.Request.Query["genre"]}";
            }
            else if (!Search.IsNullOrEmpty())
            {
                artworkUrl += $"&Title={Search}";
            }
            else if (!HttpContext.Request.Query["searchVal"].IsNullOrEmpty())
            {
                artworkUrl += $"&Title={HttpContext.Request.Query["searchVal"]}";
            }
            HttpResponseMessage accountResponseMessage = await _httpClient.GetAsync(artworkUrl);
            accountResponseMessage.EnsureSuccessStatusCode();
            string accountContent = await accountResponseMessage.Content.ReadAsStringAsync();
            dynamic accountObject = JsonConvert.DeserializeObject(accountContent);
            if (accountObject.orders != null)
            {
                List<dynamic> artworks = new List<dynamic>();
                var orders = new List<dynamic>(accountObject.orders);
                foreach (var order in orders)
                {
                    artworks.Add(order.artwork);
                }
                Artworks = artworks;
            }
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
            int recs = Artworks.Count();
            Pager = new Pager(recs, page, pageSize);
            Artworks = Artworks.Skip((page - 1) * pageSize).Take(Pager.PageSize);

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
