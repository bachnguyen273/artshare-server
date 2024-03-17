using artshare_server.WebApp.SessionHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace artshare_server.WebApp.Pages.Audiences
{
    public class CheckoutModel : PageModel
    {
        public List<dynamic> Cart { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Cart = HttpContext.Session.Get<List<dynamic>>("Cart");
            return Page();
        }
    }
}
