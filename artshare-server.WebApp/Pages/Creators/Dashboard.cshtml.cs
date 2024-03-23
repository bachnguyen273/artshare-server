using artshare_server.ApiModels.DTOs;
using artshare_server.WebApp.Helpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;

namespace artshare_server.WebApp.Pages.Creators
{
    public class DashboardModel : PageModel
    {
        public IEnumerable<TopSaleArtwork> TopSaleArtworks { get; set; }
        public Pager Pager { get; set; }
        public int? AccountID { get; set; }
        public string Search { get; set; }
        public IEnumerable<OrderViewModel> Orders { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Size { get; set; } = 7;

        private HttpClient _httpClient = new();
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetInt32("AccountId").HasValue)
            {
                AccountID = HttpContext.Session.GetInt32("AccountId");
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Order/GetAllOrdersOfCreator?id={AccountID}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                SuccesResponse obj = JsonConvert.DeserializeObject<SuccesResponse>(responseString);
                if (obj != null && obj.Data != null)
                {
                    Total = JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(obj.Data.ToString()).Count();
                    Orders = JsonConvert.DeserializeObject<IEnumerable<OrderViewModel>>(obj.Data.ToString()).Take(Size).Skip((Page - 1) * Size);
                }
                else
                {
                    Total = 0;
                }
            }
            var requestObj = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Artwork/GetTopSaleArtwork?creatorId={AccountID}");
            var responseObj = await _httpClient.SendAsync(requestObj);
            if (responseObj.IsSuccessStatusCode)
            {
                var responseString = await responseObj.Content.ReadAsStringAsync();
                SuccesResponse obj = JsonConvert.DeserializeObject<SuccesResponse>(responseString);
                if (obj != null && obj.Data != null)
                {
                    TopSaleArtworks = JsonConvert.DeserializeObject<IEnumerable<TopSaleArtwork>>(obj.Data.ToString());                }
            }
            return Page();
        }
    }
}

