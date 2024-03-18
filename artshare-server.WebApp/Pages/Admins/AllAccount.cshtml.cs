using artshare_server.WebAPI.ResponseModels;
using artshare_server.WebApp.ViewModels;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace artshare_server.WebApp.Pages.Admins
{
    public class AllAccountModel : PageModel
    {
        private HttpClient _httpClient;

        [BindProperty]
        public IEnumerable<ProfileViewModel> AccList { get; set; }

        [BindProperty]
        public int Check { get; set; }

        [BindProperty]
        public IEnumerable<ProfileViewModel> UserName { get; set; }

        [BindProperty]
        public Pager Pager { get; set; }

        public AllAccountModel()
        {
            _httpClient = new HttpClient();
        }
        public async Task OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == null)
            {
                TempData["Alert1"] = " You're not signed in...!";
                Check = 0;
            }
            else if (!role.Equals("Admin"))
            {
                TempData["Alert1"] = " You're not allow to access...!";
                Check = -1;
            }
            else
            {
                Check = 1;
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
            string username = HttpContext.Request.Query["searchValue"];
            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }
            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];
            if (username == null || String.IsNullOrEmpty(username))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/GetAllAccount");
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var acc = JsonConvert.DeserializeObject<IEnumerable<ProfileViewModel>>(responseContent);
                    int recs = acc.Count();
                    Pager = new Pager(recs, page, pageSize);
                    AccList = acc.Skip((page - 1) * pageSize).Take(Pager.PageSize);
                }
            }
            else
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/SearchByUsername/{username}");
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var acc = JsonConvert.DeserializeObject<IEnumerable<ProfileViewModel>>(responseContent);
                    int recs = acc.Count();
                    Pager = new Pager(recs, page, pageSize);
                    UserName = acc.Skip((page - 1) * pageSize).Take(Pager.PageSize);
                    TempData["SearchValue"] = username;
                }
            }

        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiUrl}/Account/DeleteAccount/{id}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                TempData["Alert"] = " Delete Success...!";
                return RedirectToPage("../Admins/AllAccount", new { page = 1 });
            }
            TempData["Alert1"] = " Delete Fail...!";
            return Page();
        }

        public async Task<IActionResult> OnPostSearch(int page = 1)
        {
            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }
            IConfiguration config = new ConfigurationBuilder()
                                       .SetBasePath(Directory.GetCurrentDirectory())
                                       .AddJsonFile("appsettings.json", true, true)
                                       .Build();
            string apiUrl = config["API_URL"];
            string username = Request.Form["SearchValue"];
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/SearchByUsername/{username}");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var acc = JsonConvert.DeserializeObject<IEnumerable<ProfileViewModel>>(responseContent);
                int recs = acc.Count();
                Pager = new Pager(recs, page, pageSize);
                UserName = acc.Skip((page - 1) * pageSize).Take(Pager.PageSize);
                TempData["Alert"] = " Find Success...!";
                Check = 1;
                TempData["SearchValue"] = username;
                return Page();
            }
            UserName = null;
            TempData["Alert1"] = $" Can't find account has Username: {username}...!";
            return Page();
        }
    }
}
