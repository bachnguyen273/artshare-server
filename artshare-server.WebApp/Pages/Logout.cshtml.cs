using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace artshare_server.WebApp.Pages
{
    public class LogoutModel : PageModel
    {

        public IActionResult OnPost()
        {
            // Clear session
            HttpContext.Session.Clear();

            return RedirectToPage("/Index"); // Redirect to home page after logout
        }
    }
}
