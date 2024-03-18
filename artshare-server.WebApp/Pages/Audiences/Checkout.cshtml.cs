using artshare_server.WebApp.SessionHelpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Nodes;

namespace artshare_server.WebApp.Pages.Audiences
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class CheckoutModel : PageModel
    {
        public string PaypalClientId { get; set; }
        public string PaypalSecret { get; set; }
        public string PaypalUrl { get; set; }
        public List<dynamic> Cart { get; set; }
        private HttpClient _httpClient;

        public CheckoutModel(IConfiguration configuration)
        {
            PaypalClientId = configuration["Paypal:ClientId"];
            PaypalSecret = configuration["Paypal:Secret"];
            PaypalUrl = configuration["Paypal:Url"];
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = HttpContext.Session.Get<List<dynamic>>("Cart");
            return Page();
        }

        public JsonResult OnPostCreateOrder()
        {
            Cart = HttpContext.Session.Get<List<dynamic>>("Cart");
            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", Cart.Sum(item => item.price).ToString());

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);

            string accesToken = GetPaypalAccessToken();

            string url = PaypalUrl + "/v2/checkout/orders";

            string orderId = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accesToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    var strResponse = readTask.Result;
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        orderId = jsonResponse["id"]?.ToString() ?? "";
                    }
                }
            }
            var response = new
            {
                Id = orderId
            };
            return new JsonResult(response);
        }

        public async Task<JsonResult> OnPostCompleteOrder([FromBody] JsonObject data)
        {
            if (data == null || data["orderID"] == null) return new JsonResult("");
            var orderID = data["orderID"]!.ToString();
            Cart = HttpContext.Session.Get<List<dynamic>>("Cart");

            string accessToken = GetPaypalAccessToken();

            string url = PaypalUrl + $"/v2/checkout/orders/{orderID}/capture";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;

                var readTask = result.Content.ReadAsStringAsync();
                readTask.Wait();

                var strResponse = readTask.Result;

                var jsonResponse = JsonNode.Parse(strResponse);
                await CreateOrder();

                return new JsonResult("success");
            }
        }


        private async Task CreateOrder()
        {
            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];

            var userId = HttpContext.Session.GetString("UserId");
            var requestBody = new
            {
                CustomerId = int.Parse(userId),
                TotalPrice = Cart.Sum(item => item.price),
                OrderDetails = new List<object>()
            };

            foreach (var item in Cart)
            {
                requestBody.OrderDetails.Add(new
                {
                    ArtworkId = item.artworkId,
                    UnitPrice = item.price
                });
            }

            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/Order/CreateOrderWithOrderDetails");
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Send the request
            var response = await _httpClient.SendAsync(request);
        }

        public JsonResult OnPostCancelOrder([FromBody] JsonObject data)
        {
            if (data == null || data["orderID"] == null) return new JsonResult("");
            var orderID = data["orderID"]!.ToString();
            return new JsonResult("");
        }

        private string GetPaypalAccessToken()
        {
            string accessToken = "";
            string url = PaypalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials64 =
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null,
                    "application/x-www-form-urlencoded");

                var responseTask = client.SendAsync(requestMessage);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    var strResponse = readTask.Result;

                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }

            return accessToken;
        }
    }
}