using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages.Audiences.Orders
{
    public class IndexModel : PageModel
    {
        public IEnumerable<OrderDashBoardViewModel> Orders { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Size { get; set; } = 5;
        public int? CustomerId { get; set; }

        private HttpClient _httpClient = new();
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetInt32("AccountId").HasValue)
            {
                CustomerId = HttpContext.Session.GetInt32("AccountId");
            }
            else
            {
                return NotFound("Account not found");
            }
            if (HttpContext.Session.GetInt32("AccountId").HasValue)
            {
                CustomerId = HttpContext.Session.GetInt32("AccountId");
            }
            else
            {
                return NotFound("Account not found");
            }
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Order/GetOrdersByCusId?customerId={CustomerId}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                SuccesResponse obj = JsonConvert.DeserializeObject<SuccesResponse>(responseString);
                Total = JsonConvert.DeserializeObject<IEnumerable<OrderDashBoardViewModel>>(obj.Data.ToString()).Count();
                Orders = JsonConvert.DeserializeObject<IEnumerable<OrderDashBoardViewModel>>(obj.Data.ToString()).Take(Size).Skip((Page - 1) * Size);
            }

            return Page();
        }
    }
}
