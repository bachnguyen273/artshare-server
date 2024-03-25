using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace artshare_server.WebApp.Pages
{
    public class ArtworkDetailModel : PageModel
    {
        [BindProperty]
        public dynamic Artwork { get; set; }

        public dynamic Comments { get; set; }
        public bool IsBought { get; set; }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await LoadData(id);
            return Page();
        }

        private async Task<IActionResult> LoadData(int? id = null)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            string apiUrl = config["API_URL"];
            string artworkUrl = $"{apiUrl}/Artwork/GetArtworkById?id={id}";
            string commentUrl = $"{apiUrl}/Comment/GetAllCommentByArtWorkId?artwordId={id}";
            using (var httpClient = new HttpClient())
            {
                //GetArtWork
                HttpResponseMessage artworkResponseMessage = await httpClient.GetAsync(artworkUrl);
                artworkResponseMessage.EnsureSuccessStatusCode();
                string artworkContent = await artworkResponseMessage.Content.ReadAsStringAsync();
                dynamic artworkObject = JsonConvert.DeserializeObject(artworkContent);
                Artwork = artworkObject.data.artwork;

                //GetComment
                HttpResponseMessage commentResponseMessage = await httpClient.GetAsync(commentUrl);
                commentResponseMessage.EnsureSuccessStatusCode();
                string commentContent = await commentResponseMessage.Content.ReadAsStringAsync();
                dynamic commentObject = JsonConvert.DeserializeObject(commentContent);
                Comments = commentObject;
            }

            if(HttpContext.Session.GetString("UserId") != null)
            {
                IsBought = await IsBoughtArtwork(int.Parse(HttpContext.Session.GetString("UserId")), Convert.ToInt32(Artwork.artworkId));
            }

            return Page();
        }

        public async Task<bool> IsBoughtArtwork(int accountId, int artworkId)
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json", true, true)
                                        .Build();
            string apiUrl = config["API_URL"];

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage artworkIdsResponseMessage = await httpClient.GetAsync($"{apiUrl}/Artwork/GetArtworkIdsByAccountId?accountId={accountId}");
                artworkIdsResponseMessage.EnsureSuccessStatusCode();
                string artworkIdsContent = await artworkIdsResponseMessage.Content.ReadAsStringAsync();
                dynamic artworkIdsObject = JsonConvert.DeserializeObject(artworkIdsContent);
                List<int> artworkIds = artworkIdsObject.data.artworkIds.ToObject<List<int>>();

                return artworkIds.Contains(artworkId);
            }
        }

        public async Task<FileResult> OnGetDownloadImageAsync(string imageUrl, string title)
        {
            using (var httpClient = new HttpClient())
            {
                var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                var contentType = GetContentType(imageUrl);
                return File(imageBytes, contentType, title + Path.GetExtension(imageUrl));
            }
        }

        private string GetContentType(string url)
        {
            var extension = Path.GetExtension(url).ToLowerInvariant();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".tiff":
                    return "image/tiff";
                case ".bmp":
                    return "image/bmp";
                case ".svg":
                    return "image/svg+xml";
                default:
                    throw new NotSupportedException($"File extension '{extension}' is not supported.");
            }
        }



        public async Task<IActionResult> OnPostComment(int id)
        {
            //Create Comment
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            string apiUrl = config["API_URL"];
            var request = new CommentViewModel
            {
                CommenterId = (int)HttpContext.Session.GetInt32("AccountId"),
                ArtworkId = id,
                Content = Request.Form["Content"],
                PostDate = DateTime.Now
            };
            using (var httpClient = new HttpClient())
            {
                var createUrl = $"{apiUrl}/Comment/CreateComment";
                var jsonBody = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(createUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    await LoadData(id);
                    return Page();
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(errorMessage);
                }
            }
        }
    }
}