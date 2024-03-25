using artshare_server.ApiModels.DTOs;
using artshare_server.WebApp.Helpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace artshare_server.WebApp.Pages.Creators
{
    public class DashboardModel : PageModel
    {
        public IEnumerable<TopSaleArtwork> TopSaleArtworks { get; set; }
        public Pager Pager { get; set; }
        public int? AccountID { get; set; }
        public string Search { get; set; }
        public IEnumerable<OrderDashBoardViewModel> Orders { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Size { get; set; } = 7;
        public decimal EarningMonthly { get; set; }
        public float GrowthRate { get; set; }
        public int ProductSaleInYear { get; set; }
        public int GrowthRateByYear { get; set; }
        public int NumberOfCustomer { get; set; }

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
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Order/GetOrderDashboard?id={AccountID}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                SuccesResponse obj = JsonConvert.DeserializeObject<SuccesResponse>(responseString);
                if (obj != null && obj.Data != null)
                {
                    Total = JsonConvert.DeserializeObject<IEnumerable<OrderDashBoardViewModel>>(obj.Data.ToString()).Count();
                    Orders = JsonConvert.DeserializeObject<IEnumerable<OrderDashBoardViewModel>>(obj.Data.ToString()); ;
                    var currentMonthOrders = Orders.Where(order => order.CreateDate.Year == DateTime.Now.Year
                                      && order.CreateDate.Month == DateTime.Now.Month);

                    // Calculate earnings for the current month
                    EarningMonthly = currentMonthOrders.Sum(order => order.Price);

                    // Filter orders for the previous month
                    var previousMonthOrders = Orders.Where(order => order.CreateDate.Year == DateTime.Now.Year
                                                       && order.CreateDate.Month == DateTime.Now.Month - 1);

                    // Calculate earnings for the previous month
                    var previousMonthRevenue = previousMonthOrders.Sum(order => order.Price);

                    // Calculate growth rate
                    GrowthRate = (float)Math.Round((previousMonthRevenue != 0 ? ((EarningMonthly - previousMonthRevenue) / previousMonthRevenue) * 100 : 0),1);
                    ProductSaleInYear = Orders.Where(order => order.CreateDate.Year == DateTime.Now.Year).Count();
                    var productSaleLastYear = Orders.Where(order => order.CreateDate.Year == DateTime.Now.Year).Count();
                    GrowthRateByYear = (ProductSaleInYear - productSaleLastYear) / productSaleLastYear;
                    IEnumerable<IGrouping<string, OrderDashBoardViewModel>> numberCustomer = Orders.GroupBy(x => x.FullName);
                    NumberOfCustomer = numberCustomer.Count();

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

