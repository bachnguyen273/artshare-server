using artshare_server.Core.Enums;
using artshare_server.Core.Models;
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

        public async Task<IActionResult> OnGetAsync(int artworkId)
		{
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            string apiUrl = config["API_URL"];
            using var client = new HttpClient();
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage artworkResponseMessage = await httpClient.GetAsync(apiUrl + "/Artwork/GetArtworkById?id=" + artworkId);
                artworkResponseMessage.EnsureSuccessStatusCode();
                string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
                dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
                Artwork = artworkObject.data.artwork;

                return Page();
            }
        }

        public IActionResult OnPostAddToCart(int itemId)
        {
            Role = HttpContext.Session.GetString("AccountRole");
            if (AccountRole == "Customer")
            {
                try
                {
                    var cage = _birdCageRepository.GetCageById(itemId);

                    if (cage != null)
                    {
                        var cart = HttpContext.Session.Get<Dictionary<BirdCage, int>>("Cart");
                        if (cart != null)
                        {
                            if (cart.ContainsKey(cage))
                            {
                                cart[cage]++;
                            }
                            else
                            {
                                cart.Add(cage, 1);
                            }
                            HttpContext.Session.Set("Cart", cart);
                        }
                        else
                        {
                            cart = new Dictionary<BirdCage, int> { { cage, 1 } };
                            HttpContext.Session.Set("Cart", cart);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                }
                return RedirectToPage();
            }
            else
            {
                return RedirectToPage("/Users/Login");
            }
        }
    }
}
