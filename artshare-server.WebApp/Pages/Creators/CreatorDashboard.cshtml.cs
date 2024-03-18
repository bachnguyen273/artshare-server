using artshare_server.Core.Models;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages.Creators
{
    public class CreatorDashboardModel : PageModel
    {
        public Account Creator { get; set; }
        public List<Artwork> NumberOfArtwork { get; set; }
        private HttpClient _httpClient;
        public CreatorDashboardModel()
        {
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            int id = (int)HttpContext.Session.GetInt32("AccountId");
           

            Creator = await GetAccountById(id);
           // NumberOfArtwork = await GetArtworks();


            if (Creator.Role.Equals("Creator"))
            {
                return RedirectToPage("./Login");
            }
            return Page();
        }
        private async Task<Account> GetAccountById(int id)
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();
            string apiUrl = config["API_URL"];
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/GetAccountById/{id}");
            var response = await _httpClient.SendAsync(request);
            Account account = null;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                account = JsonConvert.DeserializeObject<Account>(responseContent);
            }
            return account;
        }
        private async Task<List<Artwork>> GetArtworks()
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();
            string apiUrl = config["API_URL"];
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Artwork/GetArtworks");
            var response = await _httpClient.SendAsync(request);
            List<Artwork> artwork = null;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                artwork = JsonConvert.DeserializeObject<List<Artwork>>(responseContent);
            }
            return artwork;
        }
    }
}
