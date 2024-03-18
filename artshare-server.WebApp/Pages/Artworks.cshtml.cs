using artshare_server.WebApp.SessionHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;

namespace artshare_server.WebApp.Pages
{
    public class ArtworksModel : PageModel
    {
        public dynamic Artwork { get; set; }
        public string Role { get; set; }
        private HttpClient _httpClient;
        public bool IsBought { get; set; }


        public ArtworksModel()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> OnGetAsync(int artworkId)
        {
            if (artworkId == 0)
            {
                artworkId = (int)TempData["artworkId"]; // Store artworkId in TempData
            }
            Artwork = await GetArtworkById(artworkId);
            IsBought = await IsBoughtArtwork(int.Parse(HttpContext.Session.GetString("UserId")), Convert.ToInt32(Artwork.artworkId));
            return Page();
        }


        public async Task<IActionResult> OnPostAddToCartAsync(int itemId)
        {
            Role = HttpContext.Session.GetString("Role");
            if (Role == "Audience")
            {
                dynamic artwork = await GetArtworkById(itemId);
                var cart = HttpContext.Session.Get<List<dynamic>>("Cart");
                if (cart != null)
                {
                    cart.Add(artwork);
                }
                else
                {
                    cart = new List<dynamic> { artwork };
                }
                HttpContext.Session.Set("Cart", cart);
                TempData["artworkId"] = itemId;
                return RedirectToPage();
            }
            else
            {
                return RedirectToPage("./Login");
            }
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(int itemId)
        {
            var cart = HttpContext.Session.Get<List<dynamic>>("Cart");
            if (cart != null)
            {
                cart.RemoveAll(item => item.artworkId == itemId);
                HttpContext.Session.Set("Cart", cart);
            }
            TempData["artworkId"] = itemId;
            return RedirectToPage();
        }



        private async Task<dynamic> GetArtworkById(int artworkId)
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();
            string apiUrl = config["API_URL"];

            HttpResponseMessage artworkResponseMessage = await _httpClient.GetAsync($"{apiUrl}/Artwork/GetArtworkById?id={artworkId}");
            artworkResponseMessage.EnsureSuccessStatusCode();
            string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
            dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
            return artworkObject.data.artwork;
        }


        public bool IsProductInCart(dynamic artwork)
        {
            var cart = HttpContext.Session.Get<List<dynamic>>("Cart");
            return cart != null && cart.Any(item => item.artworkId == artwork.artworkId);
        }

        public async Task<bool> IsBoughtArtwork(int accountId, int artworkId)
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();
            string apiUrl = config["API_URL"];

            HttpResponseMessage artworkIdsResponseMessage = await _httpClient.GetAsync($"{apiUrl}/Artwork/GetArtworkIdsByAccountId?accountId={accountId}");
            artworkIdsResponseMessage.EnsureSuccessStatusCode();
            string artworkIdsContent = await artworkIdsResponseMessage.Content.ReadAsStringAsync();
            dynamic artworkIdsObject = JsonConvert.DeserializeObject(artworkIdsContent);
            List<int> artworkIds = artworkIdsObject.data.artworkIds.ToObject<List<int>>();

            return artworkIds.Contains(artworkId);
        }


    }
}