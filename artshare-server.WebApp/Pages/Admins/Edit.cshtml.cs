using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace artshare_server.WebApp.Pages.Admins
{
    public class EditModel : PageModel
    {

        [BindProperty]
        public ProfileViewModel? ProfileViewModel1 { get; set; }
        [BindProperty]
        public int ProfileId { get; set; }

        private HttpClient _httpClient;

        public EditModel()
        {
            _httpClient = new HttpClient();
        }

        public async Task OnGet()
        {
            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];
            int id = int.Parse(HttpContext.Request.Query["id"].ToString());
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/GetAccountById/{id}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                ProfileViewModel1 = JsonConvert.DeserializeObject<ProfileViewModel>(responseContent);
                ProfileId = id;
            }
        }

        public async Task<IActionResult> OnPostProccessRequest(int id)
        {
            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];
            //if (!ModelState.IsValid)
            //{
            //	return Page();
            //}
            ProfileId = id;
            var requestBody = new ProfileViewModel
            {
                AccountId = id,
                Email = Request.Form["Email"],
                Password = "123",
                AvatarUrl = null,
                UserName = Request.Form["UserName"],
                FullName = Request.Form["FullName"],
                PhoneNumber = Request.Form["PhoneNumber"],
                JoinDate = DateTime.Parse(Request.Form["JoinDate"]),
                Role = Request.Form["Role"],
                Status = Request.Form["Status-Up"]
            };

            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");


            // Send the request
            var response = await _httpClient.PutAsync($"{apiUrl}/Account/UpdateProfile/{id}", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["AlertMessage"] = " Update Account Successfully...!";
                return RedirectToPage("/Admins/Edit", new { id });
            }
            TempData["AlertMessage1"] = " Update Account Fail...!";
            return Page();
        }
    }
}

