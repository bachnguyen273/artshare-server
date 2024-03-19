using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Text.Json;

namespace artshare_server.WebApp.Pages.Admins.Genres
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpCLient;
        private string _apiURL;

        [BindProperty]
        public CreateGenreViewModel CreateGenreViewModel { get; set; }
        public CreateModel()
        {
            _httpCLient = new HttpClient();
            IConfiguration config = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true, true)
                          .Build();

            _apiURL = config["API_URL"];
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var requestBody = new
            {
                name = CreateGenreViewModel.Name
            };

            var jsonRequestBody = JsonSerializer.Serialize(requestBody);
            try
            {
                var content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpCLient.PostAsync(_apiURL + "/Genre/CreateGenre", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }
            }
            catch (Exception ex)
            {
                
            }

            return Page();
        }
    }
}
