using artshare_server.WebApp.Helpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages.Admins
{
    public class IndexModel : PageModel
    {
        private HttpClient _httpClient;

        [BindProperty]
        public IEnumerable<dynamic> AccList { get; set; }

        [BindProperty]
        public int Check { get; set; }

        [BindProperty]
        public IEnumerable<ProfileViewModel> UserName { get; set; }

        [BindProperty]
        public Pager Pager { get; set; }

        public IndexModel()
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
            if (username == null || string.IsNullOrEmpty(username))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/GetAllAccount");
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    //var responseContent = await response.Content.ReadAsStringAsync();
                    // var acc = JsonConvert.DeserializeObject<IEnumerable<ProfileViewModel>>(responseContent);
                    //int recs = acc.Count();
                    //Pager = new Pager(recs, page, pageSize);
                    //AccList = acc.Skip((page - 1) * pageSize).Take(Pager.PageSize);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic accs = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    List<dynamic> accList = new List<dynamic>();
                    foreach (var acc in accs.items)
                    {
                        dynamic temp = new
                        {
                            AccountId = acc.accountId,
                            JoinDate = acc.joinDate,
                            AvatarUrl = acc.avatarUrl,
                            Email = acc.email,
                            Role = acc.role,
                            Status = acc.status,
                            UserName = acc.userName,
                            FullName = acc.fullName,
                            PhoneNumber = acc.phoneNumber
                        };
                        accList.Add(temp);
                    }
                    int recs = accList.Count();
                    Pager = new Pager(recs, page, pageSize);
                    AccList = accList.Skip((page - 1) * pageSize).Take(Pager.PageSize);
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
                return RedirectToPage("../Admins/Index", new { page = 1 });
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/SearchByUsernameToList/{username}");
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

