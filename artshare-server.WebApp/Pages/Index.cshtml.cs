using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;

namespace artshare_server.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;



        public void OnGet()
        {
            
        }
    }
}