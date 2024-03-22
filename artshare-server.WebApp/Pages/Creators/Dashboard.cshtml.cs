using artshare_server.WebApp.Helpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace artshare_server.WebApp.Pages.Creators
{
    public class DashboardModel : PageModel
    {
        private HttpClient _httpClient;
        [BindProperty]
        public IEnumerable<OrderViewModel> Orders { get; set; }
        [BindProperty]
        public int Check { get; set; }

        [BindProperty]
        public Pager Pager { get; set; }
        public DashboardModel()
        {
            _httpClient = new HttpClient();
        }
        public void OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == null)
            {
                TempData["Alert1"] = " You're not signed in...!";
                Check = 0;
            }
            else if (!role.Equals("Creator"))
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
        }
    }
}
