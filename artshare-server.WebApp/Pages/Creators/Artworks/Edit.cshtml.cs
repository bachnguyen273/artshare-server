using artshare_server.WebApp.Helpers;
using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

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
        public int SelectedWatermarkId { get; set; }
        public int SelectedGenreId { get; set; }
        [BindProperty]
        public dynamic Artwork { get; private set; }

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
            UpdateArtworkViewModel = new UpdateArtworkViewModel();
            // Load data
            try
            {
                await LoadWatermarkByCreatorIdAsync(GetAccountIdFromToken());
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
            _jwtToken = HttpContext.Session.GetString("JWTToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            HttpResponseMessage artworkResponseMessage = await _httpClient.GetAsync(_apiURL + "/Artwork/GetArtworkById?id=" + artworkId);
            artworkResponseMessage.EnsureSuccessStatusCode();
            string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
            dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);

            Artwork = artworkObject.data.artwork;

            UpdateArtworkViewModel.ArtworkId = Artwork.artworkId;
            UpdateArtworkViewModel.GenreId = Artwork.genreId;
            UpdateArtworkViewModel.WatermarkId = Artwork.watermarkId;
            UpdateArtworkViewModel.Title = Artwork.title;
            UpdateArtworkViewModel.Description = Artwork.description;
            UpdateArtworkViewModel.Price = Artwork.price;
            UpdateArtworkViewModel.OriginalArtUrl = Artwork.originalArtUrl;
            UpdateArtworkViewModel.WatermarkedArtUrl = Artwork.watermarkedArtUrl;
            UpdateArtworkViewModel.Status = Artwork.artworkStatus;

            SelectedWatermarkId = Artwork.watermarkId;
            SelectedGenreId = Artwork.genreId;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Authorize
            _jwtToken = HttpContext.Session.GetString("JWTToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

            // Load data
            await LoadWatermarkByCreatorIdAsync(GetAccountIdFromToken());
            await LoadGenresAsync();

            // Create artwork
            var request = new
            {
                creatorId = GetAccountIdFromToken(),
                watermarkId = SelectedWatermarkId,
                title = UpdateArtworkViewModel.Title,
                description = UpdateArtworkViewModel.Description,
                price = UpdateArtworkViewModel.Price,
                genreId = UpdateArtworkViewModel.GenreId,
                originalArtUrl = await ImageHelpers.UploadOriginalArtworkFile(_apiURL, UpdateArtworkViewModel.OrginalArtworkFile),
                watermarkedArtUrl = await ImageHelpers.UploadWatermarkArtworkFile(_apiURL, UpdateArtworkViewModel.OrginalArtworkFile, GetWatermarkUrl(SelectedWatermarkId))
            };

            var createUrl = _apiURL + "/Artwork/UpdateArtwork?artworkStatus=" + UpdateArtworkViewModel.Status;
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

        private async Task LoadWatermarkByCreatorIdAsync(string creatorId)
        {
            string watermarkUrl = _apiURL + "/Watermark/GetWatermakrs?CreatorId=" + creatorId;
            var request = new HttpRequestMessage(HttpMethod.Get, watermarkUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            HttpResponseMessage watermarkResponseMessage = await _httpClient.SendAsync(request);
            watermarkResponseMessage.EnsureSuccessStatusCode();
            string watermarkContent = await watermarkResponseMessage.Content.ReadAsStringAsync();
            dynamic watermarkObject = JsonConvert.DeserializeObject(watermarkContent);
            Watermarks = new List<dynamic>(watermarkObject.items);
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

        private string GetWatermarkUrl(int watermarkId)
        {
            foreach (var watermark in Watermarks)
            {
                if (watermark.watermarkId == watermarkId)
                {
                    return watermark.watermarkUrl;
                }
            }
            return null;
        }
    }
}
