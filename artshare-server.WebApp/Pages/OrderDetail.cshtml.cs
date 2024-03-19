using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;
using artshare_server.WebAPI.ResponseModels;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages
{
    public class OrderDetailModel : PageModel
    {
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
       

        private HttpClient _httpClient = new();
        public async Task<IActionResult> OnGet(int id)
        {
            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/OrderDetail/GetOrderDetailsByOrderId?orderId={id}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<SucceededResponseModel>(responseString);
                
                OrderDetails = JsonConvert.DeserializeObject<IEnumerable<OrderDetailViewModel>>(obj.Data.ToString());
            }

            return Page();
        }
    }
}
