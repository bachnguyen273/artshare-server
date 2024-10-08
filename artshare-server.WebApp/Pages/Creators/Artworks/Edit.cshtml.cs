using artshare_server.WebApp.Helpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection;

namespace artshare_server.WebApp.Pages.Creators.Artworks
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public UpdateArtworkViewModel UpdateArtworkViewModel { get; set; }
        public List<dynamic> Watermarks { get; set; }
        public List<dynamic> Genres { get; set; }
        private readonly HttpClient _httpClient;
        private string _apiURL;
        private string _jwtToken;
        [BindProperty]
        public int SelectedGenreId { get; set; }
        [BindProperty]
        public string SelectedStatus { get; set; }
        [BindProperty]
        public dynamic Artwork { get; private set; }
        public dynamic _artworkId { get; set; }
 

        public EditModel()
        {
            _httpClient = new HttpClient();
            IConfiguration config = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true, true)
                          .Build();

            _apiURL = config["API_URL"];
        }

        public async Task OnGet(int artworkId)
        {
            TempData["ArtworkId"] = artworkId;
            // Load data
            try
            {
                await LoadArtwork(artworkId);
                await LoadGenresAsync();
                await LoadOldArtworkData(artworkId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task LoadOldArtworkData(int artworkId)
        {
            UpdateArtworkViewModel = new UpdateArtworkViewModel();
            UpdateArtworkViewModel.ArtworkId = artworkId;
            UpdateArtworkViewModel.CreatorId = Int32.Parse(GetAccountIdFromToken());
            UpdateArtworkViewModel.Title = Artwork.title;
            UpdateArtworkViewModel.Description = Artwork.description;
            UpdateArtworkViewModel.Price = Artwork.price;
            SelectedStatus = Artwork.artworkStatus;
            
            SelectedGenreId = Artwork.genreId;
        }

        public async Task<IActionResult> OnPost(int artworkId)
        {
            try
            {
                await LoadGenresAsync();
                await LoadArtwork(artworkId);

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Authorize
                _jwtToken = HttpContext.Session.GetString("JWTToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

                if (UpdateArtworkViewModel.OrginalArtworkFile != null)
                {

                    if (!ImageHelpers.IsImage(UpdateArtworkViewModel.OrginalArtworkFile))
                    {
                        TempData["ErrorMessage"] = "This should be image";
                        return Page();
                    }
                }


                // Create artwork
                var request = new
                {
                    creatorId = GetAccountIdFromToken(),
                    title = UpdateArtworkViewModel.Title,
                    description = UpdateArtworkViewModel.Description,
                    price = UpdateArtworkViewModel.Price,
                    genreId = SelectedGenreId,
                    originalArtUrl = UpdateArtworkViewModel.OrginalArtworkFile == null ? Artwork.originalArtUrl : await ImageHelpers.UploadOriginalArtworkFile(_apiURL, UpdateArtworkViewModel.OrginalArtworkFile),
                    watermarkedArtUrl = UpdateArtworkViewModel.OrginalArtworkFile == null ? Artwork.watermarkedArtUrl : await ImageHelpers.UploadWatermarkArtworkFile(_apiURL, UpdateArtworkViewModel.OrginalArtworkFile)
                };

                var updateUrl = _apiURL + $"/Artwork/UpdateArtwork/{artworkId}?artworkStatus={SelectedStatus}";
                var jsonBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(updateUrl, content);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        private async Task LoadArtwork(int artworkId)
        {
            _jwtToken = HttpContext.Session.GetString("JWTToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            HttpResponseMessage artworkResponseMessage = await _httpClient.GetAsync(_apiURL + "/Artwork/GetArtworkById?id=" + artworkId);
            artworkResponseMessage.EnsureSuccessStatusCode();
            string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
            dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);

            Artwork = artworkObject.data.artwork;
        }
    }
}
