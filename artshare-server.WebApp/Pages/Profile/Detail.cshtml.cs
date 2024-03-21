using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace artshare_server.WebApp.Pages.Profile
{
    public class DetailModel : PageModel
    {
        [BindProperty]
        public ProfileViewModel? ProfileViewModel1 { get; set; }
        [BindProperty]
        public int ProfileId { get; set; }
        public List<string> OriginalArtUrl { get; set; }
        public List<string> ArtworkUrls { get; set; } = new List<string>();

        private HttpClient _httpClient;

        public DetailModel()
        {
            _httpClient = new HttpClient();
        }

        public async Task OnGet()
        {
            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];
            int id = (int) HttpContext.Session.GetInt32("AccountId");
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/GetAccountById/{id}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                ProfileViewModel1 = JsonConvert.DeserializeObject<ProfileViewModel>(responseContent);
                ProfileId = id;
            }

            //var idsResponse = await _httpClient.GetAsync("http://localhost:5292/api/Artwork/GetArtworkIdsByAccountId?accountId=7");

            //if (!idsResponse.IsSuccessStatusCode)
            //{
            //    ModelState.AddModelError(string.Empty, "Failed to retrieve artwork IDs.");
            //}
            //var idsContent = await idsResponse.Content.ReadAsStringAsync();
            //var artworkIdsObject = JObject.Parse(idsContent);
            //var artworkIdsArray = artworkIdsObject["data"]["artworkIds"].ToObject<int[]>();

            //foreach (var aid in artworkIdsArray)
            //{
            //    var artworkResponse = await _httpClient.GetAsync($"http://localhost:5292/api/Artwork/GetArtworkById?id={aid}");
            //    if (artworkResponse.IsSuccessStatusCode)
            //    {
            //        var artworkContent = await artworkResponse.Content.ReadAsStringAsync();
            //        dynamic artworkData = JObject.Parse(artworkContent);
            //        var artworkUrlsArray = artworkData.data.artwork.originalArtUrl;
            //        foreach (var url in artworkUrlsArray)
            //        {
            //            ArtworkUrls.Add(url.ToString());
            //        }
            //    }
            //    else
            //    {
            //        ModelState.AddModelError(string.Empty, $"Failed to retrieve artwork {id}.");
            //    }
            //}
        }
    }
}
