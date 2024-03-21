using artshare_server.WebApp.SessionHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace artshare_server.WebApp.Pages.Audiences;

public class CartModel : PageModel
{
    public List<dynamic> Cart { get; set; }
    private HttpClient _httpClient;

    public CartModel()
    {
        _httpClient = new HttpClient();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = HttpContext.Session.Get<List<dynamic>>("Cart");
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveFromCartAsync(int itemId)
    {
        var cart = HttpContext.Session.Get<List<dynamic>>("Cart");
        if (cart != null)
        {
            cart.RemoveAll(item => item.artworkId == itemId);
            HttpContext.Session.Set("Cart", cart);
        }
        return RedirectToPage();
    }
}