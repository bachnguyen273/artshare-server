using artshare_server.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace artshare_server.WebApp.Pages
{
    public class ArtworkDetailModel : PageModel
    {
        [BindProperty]
        public dynamic Artwork { get; set; }
        public dynamic Comments { get; set; }

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


            return Page();
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
                    var request2 = new LikeCountViewModel
                    {
                        LikeCount = 0,
                        DislikeCount = 0,
                        CommentCount = int.Parse(Request.Form["CommentCount"]) + 1
                    };
                    using (var httpClient1 = new HttpClient())
                    {
                        var createUrl1 = $"{apiUrl}/Artwork/UpdateCountByArtworkId?artworkId={id}";
                        var jsonBody1 = JsonConvert.SerializeObject(request2);
                        var content1 = new StringContent(jsonBody1, System.Text.Encoding.UTF8, "application/json");
                        var response1 = await httpClient.PutAsync(createUrl1, content1);
                        if (response1.IsSuccessStatusCode)
                        {
                            await LoadData(id);
                            return Page();
                        }
                        else
                        {
                            var errorMessage1 = await response.Content.ReadAsStringAsync();
                            return BadRequest(errorMessage1);
                        }
                    }
                }

                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(errorMessage);
                }
            }
        }

        public async Task<IActionResult> OnPostReport(int id)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            string apiUrl = config["API_URL"];
            var request = new ReportViewModel
            {
                AccountId = (int)HttpContext.Session.GetInt32("AccountId"),
                ArtworkId = id,
                Content = Request.Form["reportContent"],
                Category = Request.Form["reportCate"],
                ReportDate = DateTime.Now,
                Status = "Processing"
            };
            using (var httpClient = new HttpClient())
            {
                var createUrl = $"{apiUrl}/Report/CreateReport";
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
