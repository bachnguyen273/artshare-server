using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages.Admins.Orders
{
    public class IndexModel : PageModel
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Size { get; set; } = 5;
        public dynamic orders { get; set; }

        private HttpClient _httpClient = new();
        public async Task<IActionResult> OnGet()
        {
            int page;
            if (HttpContext.Request.Query["page"].IsNullOrEmpty())
            {
                page = 1;
            }
            else
            {
                page = int.Parse(HttpContext.Request.Query["page"]);
            }


            Page = page > 0 ? page : 1;

            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Order/GetAllOrders");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {

            }

            return Page();
        }
    }
}
