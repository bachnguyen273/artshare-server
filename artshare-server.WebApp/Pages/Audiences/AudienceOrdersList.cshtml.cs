using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;
using artshare_server.WebAPI.ResponseModels;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace artshare_server.WebApp.Pages.Audiences;

public class AudienceOrdersListModel : PageModel
{

	public IEnumerable<OrderDTO> Orders { get; set; }
	public int Total { get; set; }
	public int Page { get; set; }
	public int Size { get; set; } = 5;
	public int CustomerId { get; set; }

	private HttpClient _httpClient = new();
	public async Task<IActionResult> OnGet(int customerId)
	{   CustomerId= customerId;
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
			var obj = JsonConvert.DeserializeObject<SucceededResponseModel>(responseString);
			Total = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(obj.Data.ToString()).Count();
			Orders = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(obj.Data.ToString()).Skip((Page - 1) * Size).Take(Size);
		}

		return Page();
	}
}
