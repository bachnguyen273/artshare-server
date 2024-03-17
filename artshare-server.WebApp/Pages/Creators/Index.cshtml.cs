using artshare_server.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages.Creators
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }
        public ActionResult OnGetAsync()
        {
            return RedirectToPage("../Creators/CreatorDashboard");
        }
    }
}
