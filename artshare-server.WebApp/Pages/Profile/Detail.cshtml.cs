using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages.Profile
{
    public class DetailModel : PageModel
    {
        [BindProperty]
        public ProfileViewModel? ProfileViewModel1 { get; set; }
        [BindProperty]
        public int ProfileId { get; set; }

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
        }
    }
}
