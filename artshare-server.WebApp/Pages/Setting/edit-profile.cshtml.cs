using artshare_server.WebApp.ViewModels;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

// ?? truy c?p vào trang này RedirectToPage("../Setting/edit-profile", new { id = id })
// Truy c?p b?i c? 3 role

namespace artshare_server.WebApp.Pages.Setting
{
	public class edit_profileModel : PageModel
	{
		[BindProperty]
		public ProfileViewModel ProfileViewModel1 { get; set; }
		[BindProperty]
		public int ProfileId { get; set; }
		
		private HttpClient _httpClient;

		public edit_profileModel()
		{
			_httpClient = new HttpClient();
		}

		public async Task OnGetAsync()
		{
			TempData["Role"] = HttpContext.Session.GetString("Role");
			IConfiguration config = new ConfigurationBuilder()
									   .SetBasePath(Directory.GetCurrentDirectory())
									   .AddJsonFile("appsettings.json", true, true)
									   .Build();
			string apiUrl = config["API_URL"];
			int id = int.Parse(HttpContext.Request.Query["id"].ToString());			
			var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUrl}/Account/GetAccountById/{id}");
			var response = await _httpClient.SendAsync(request);
			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				ProfileViewModel1 = JsonConvert.DeserializeObject<ProfileViewModel>(responseContent);
				ProfileId = id;
			}
		}

		[BindProperty]
		public IFormFile? UploadAvatar { get; set; }

		public async Task<IActionResult> OnPostProccessRequest(int id)
		{
            ProfileId = id;
            IConfiguration config = new ConfigurationBuilder()
									   .SetBasePath(Directory.GetCurrentDirectory())
									   .AddJsonFile("appsettings.json", true, true)
									   .Build();
			string apiUrl = config["API_URL"];
			if (!ModelState.IsValid)
			{
			  return Page();
			}
			ProfileId = id;
			bool check = false;
			string passwordHash = ProfileViewModel1.PasswordHash;
			string avatarRes = null;
			if(UploadAvatar != null)
			{
				using (var content = new MultipartFormDataContent())
				{
					using (var fileStream = UploadAvatar.OpenReadStream())
					{
						using (var streamContent = new StreamContent(fileStream))
						{
							using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
							{
								fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(UploadAvatar.ContentType);

								content.Add(fileContent, "file", UploadAvatar.FileName);
								var response2 = await _httpClient.PostAsync($"{apiUrl}/Account/UploadFileAvatar", content);
								if (response2.IsSuccessStatusCode)
								{
									var responseContent1 = await response2.Content.ReadAsStringAsync();
									var point1 = JsonConvert.DeserializeObject<SuccesResponse>(responseContent1);
									//"fileUri": "https://artsharing.blob.core.windows.net/apifile/palette.png"
									var mess = point1.Data.ToString();
									string start = "\"fileUri\": \"";
									string end = "\"";
									int Start = mess.IndexOf(start, 0) + start.Length;
									int End = mess.IndexOf(end, Start);
									avatarRes = mess.Substring(Start, End - Start);
								}
							}
						}
					}
				}
			
			}
			if (!string.IsNullOrEmpty(Request.Form["newPassword"]) && !string.IsNullOrEmpty(Request.Form["confirmPassword"]))
			{
				if (Request.Form["newPassword"].Equals(Request.Form["confirmPassword"]))
				{
					check = true;					
				}
				else
				{
					TempData["AlertMessage1"] = " New Password and Confirm Password isn't the same...!";
					return Page();
				}
			}
			else
			{
				passwordHash = null;
			}			
			var requestBody = new ProfileViewModel
			{
				AccountId = id,
				Email = Request.Form["Email"],
				PasswordHash = (check == true) ? Request.Form["confirmPassword"] : passwordHash,
				AvatarUrl = (avatarRes != null)? avatarRes : null,
				UserName = Request.Form["UserName"],
				FullName = Request.Form["FullName"],
				PhoneNumber = Request.Form["PhoneNumber"],
				JoinDate = DateTime.Parse(Request.Form["JoinDate"]),
				Role = Request.Form["Role"],
				Status = Request.Form["Status"]
			};

			var jsonBody = JsonConvert.SerializeObject(requestBody);
			var requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");


			// Send the request
			var response = await _httpClient.PutAsync($"{apiUrl}/Account/UpdateProfile/{id}", requestContent);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				TempData["AlertMessage"] = " Update Account Successfully...!";
				return RedirectToPage("../Setting/edit-profile", new { id = id });  
			}
			TempData["AlertMessage1"] = " Update Account Fail...!";
			return Page();
		}
	}
}
