//using artshare_server.WebApp.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json;
//using System.Text;

//namespace artshare_server.WebApp.Pages.Creators;

//public class ArtworkOrdersListModel : PageModel
//{

//	public dynamic Orders { get; set; }
//	public int Total { get; set; }
//	public int Page { get; set; }
//	public int Size { get; set; } = 5;
//	public int ArtId { get; set; }

//	private HttpClient _httpClient = new();
//	public async Task<IActionResult> OnGet(int artworkId)
//	{   ArtId=artworkId;
//		int page;
//		if (HttpContext.Request.Query["page"].IsNullOrEmpty())
//		{
//			page = 1;
//		}
//		else
//		{
//			page = int.Parse(HttpContext.Request.Query["page"]);
//		}


//		Page = page > 0 ? page : 1;

//		IConfiguration config = new ConfigurationBuilder()
//								   .SetBasePath(Directory.GetCurrentDirectory())
//								   .AddJsonFile("appsettings.json", true, true)
//								   .Build();
//		string apiUrl = config["API_URL"];
//		var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Order/GetOrdersByArtId?artId={artworkId}");
//		var response = await _httpClient.SendAsync(request);
//		if (response.IsSuccessStatusCode)
//		{
//			var responseString = await response.Content.ReadAsStringAsync();
//			var obj = JsonConvert.DeserializeObject<SucceededResponseModel>(responseString);
//			Total = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(obj.Data.ToString()).Count();
//			Orders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(obj.Data.ToString()).Skip((Page - 1) * Size).Take(Size);
//		}

//		return Page();
//	}
//}
