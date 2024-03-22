using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public dynamic Accounts { get; set; }
        public List<dynamic> Artworks { get; private set; }

        private HttpClient _httpClient;
        private string _apiUrl;

        public DetailModel()
        {
            _httpClient = new HttpClient();
            IConfiguration config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", true, true)
                           .Build();
            _apiUrl = config["API_URL"];
        }

        public async Task OnGet()
        {
            int id = (int) HttpContext.Session.GetInt32("AccountId");
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/Account/GetAccountById/{id}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                ProfileViewModel1 = JsonConvert.DeserializeObject<ProfileViewModel>(responseContent);
                ProfileId = id;
            }

            var role = HttpContext.Session.GetString("Role");
            if (role == "Creator")
            {

            }
            else if (role == "Audience")
            {
                // Hiện những artwkrk đã mua
                GetArtworkFromOrderByAudienceId(id);
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

        private async void GetArtworkFromOrderByAudienceId(int audienceId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_apiUrl}/Account/GetAccountById/{audienceId}");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Accounts = JsonConvert.DeserializeObject<dynamic>(responseData);
                    if (Accounts != null && Accounts.orders != null)
                    {
                        // Extract artwork information from orders
                        Artworks = new List<dynamic>();
                        foreach (var order in Accounts.orders)
                        {
                            Artworks.Add(order.artwork);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
