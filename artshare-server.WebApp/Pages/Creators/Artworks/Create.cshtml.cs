using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public List<dynamic> Watermarks { get; set; }
		public List<dynamic> Genres { get; set; }
		private readonly HttpClient _httpClient;
		private string _apiURL;
		private string _jwtToken;

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
			await LoadWatermarkByCreatorIdAsync(GetAccountIdFromToken());
			await LoadGenresAsync();
		}

        public async Task<string> UploadArtwork(IFormFile artworkFile)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(artworkFile.OpenReadStream()), "file", artworkFile.FileName);

                var response = await _httpClient.PostAsync(_apiURL + "/Auth/UploadFile?containerName=1", content);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic responseData = JsonConvert.DeserializeObject(responseContent);
                    return responseData.data.fileUri;
                }
                else
                {
                    // Handle non-successful response (e.g., log error or throw exception)
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // You can log the error or handle it as per your application's requirements
                    throw new Exception($"Failed to upload artwork: {response.StatusCode} - {responseContent}");
                }
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

            // Create artwork
            var request = new
            {
                creatorId = GetAccountIdFromToken(),
                watermarkId = CreateArtworkViewModel.WatermarkId,
                title = CreateArtworkViewModel.Title,
                description = CreateArtworkViewModel.Description,
                price = CreateArtworkViewModel.Price,
                genreId = CreateArtworkViewModel.GenreId,
                originalArtUrl = await UploadArtwork(CreateArtworkViewModel.OrginalArtworkFile)
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
            // Decode the token
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
