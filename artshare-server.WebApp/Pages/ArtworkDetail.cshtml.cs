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
