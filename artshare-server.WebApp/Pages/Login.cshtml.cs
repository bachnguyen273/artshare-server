using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace artshare_server.WebApp.Pages
{
    public class LoginModel : PageModel
    {
		[BindProperty]
        public AccountViewModel AccountViewModel { get; set; }
        private HttpClient _httpClient;

		public LoginModel()
		{
			_httpClient = new HttpClient();
		}
		public async Task<IActionResult> OnPost()
		{
			 IConfiguration config = new ConfigurationBuilder()
										.SetBasePath(Directory.GetCurrentDirectory())
										.AddJsonFile("appsettings.json", true, true)
										.Build();
			string apiUrl = config["API_URL"];
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var requestBody = new
			{
				Email = AccountViewModel.Email,
				Password = AccountViewModel.Password
			};

			var jsonBody = JsonConvert.SerializeObject(requestBody);
			var request = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/Auth/Login");
			request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

			// Send the request
			var response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
                var jwtToken = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JWTToken", jwtToken);

                // Decode the token
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(jwtToken) as JwtSecurityToken;

                // Extract the role claim
                var role = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
                int id = int.Parse(tokenS.Claims.First(claim => claim.Type == "AccountId").Value);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetInt32("AccountId", id);

                // Redirect users based on their roles
                switch (role)
                {
                    case "Audience":
                        return RedirectToPage("./Index");
                    case "Creator":
                        return RedirectToPage("./Creators/CreatorDashboard");
                    case "Admin":
                        return RedirectToPage("./Admin/Index");
                    default:
                        return Page();
                }
            }
			return Page();
		}
	}
}
