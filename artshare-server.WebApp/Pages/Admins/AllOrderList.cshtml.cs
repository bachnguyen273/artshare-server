using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages.Admins
{
    public class AllOrdersListModel : PageModel
    {

        public IEnumerable<OrderViewModel> Orders { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Size { get; set; } = 5;

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
                var responseString = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<SuccesResponse>(responseString);
                Total = JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(obj.Data.ToString()).Count();
                Orders = JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(obj.Data.ToString()).Skip((Page - 1) * Size).Take(Size);
            }
            return Page();
        }
    }
}
