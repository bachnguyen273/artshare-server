using artshare_server.WebApp.Helpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace artshare_server.WebApp.Pages.Creators.Artworks
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateArtworkViewModel CreateArtworkViewModel { get; set; }
		public List<dynamic> Genres { get; set; }
		private readonly HttpClient _httpClient;
		private string _apiURL;
		private string _jwtToken;
        [BindProperty]
        public int SelectedWatermarkId { get; set; }
        [BindProperty]
        public int SelectedGenreId { get; set; }

        public CreateModel()
		{
			_httpClient = new HttpClient();
			IConfiguration config = new ConfigurationBuilder()
						  .SetBasePath(Directory.GetCurrentDirectory())
						  .AddJsonFile("appsettings.json", true, true)
						  .Build();

			_apiURL = config["API_URL"];
		}

		public async Task OnGet()
		{
            // Load data
            try
            {
                await LoadGenresAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }		
		}

        public async Task<IActionResult> OnPost()
        {
            // Authorize
            _jwtToken = HttpContext.Session.GetString("JWTToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            // Load data
            await LoadGenresAsync();


            if (!ImageHelpers.IsImage(CreateArtworkViewModel.OrginalArtworkFile))
            {
                TempData["ErrorMessage"] = "This should be image";
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            // Create artwork
            var request = new
            {
                creatorId = GetAccountIdFromToken(),
                watermarkId = SelectedWatermarkId,
                title = CreateArtworkViewModel.Title,
                description = CreateArtworkViewModel.Description,
                price = CreateArtworkViewModel.Price,
                genreId = CreateArtworkViewModel.GenreId,
                originalArtUrl = await ImageHelpers.UploadOriginalArtworkFile(_apiURL, CreateArtworkViewModel.OrginalArtworkFile),
                watermarkedArtUrl = await ImageHelpers.UploadWatermarkArtworkFile(_apiURL, CreateArtworkViewModel.OrginalArtworkFile)
            };
            var createUrl = _apiURL + "/Artwork/CreateArtwork?artworkStatus=" + CreateArtworkViewModel.Status;
            var jsonBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(createUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BadRequest(errorMessage);
            }
        }

		private string GetAccountIdFromToken()
		{
            _jwtToken = HttpContext.Session.GetString("JWTToken");
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(_jwtToken) as JwtSecurityToken;
            return tokenS.Claims.First(claim => claim.Type == "AccountId").Value;
        }

		private async Task LoadGenresAsync()
		{
            string genreUrl = _apiURL + "/Genre/GetGenres";
            HttpResponseMessage genreResponseMessage = await _httpClient.GetAsync(genreUrl);
            genreResponseMessage.EnsureSuccessStatusCode();
            string genreContent = await genreResponseMessage.Content.ReadAsStringAsync();
            dynamic genreObject = JsonConvert.DeserializeObject(genreContent);
            Genres = new List<dynamic>(genreObject.items);
        }
    }
}
