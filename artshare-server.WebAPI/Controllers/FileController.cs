using artshare_server.Core.Enums;
using artshare_server.Services.Interfaces;
using artshare_server.WebAPI.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class FileController : Controller
	{
		private readonly IAzureBlobStorageService _azureBlobStorageService;
		public FileController(IAuthService authService, IAccountService accountService, IAzureBlobStorageService azureBlobStorageService)
		{
			_azureBlobStorageService = azureBlobStorageService;
		}
		[HttpPost]
		public async Task<IActionResult> UploadFile(IFormFile file, ContainerEnum containerName)
		{
			try
			{
				if (file == null || file.Length == 0)
					return BadRequest(new FailedResponseModel()
					{
						Status = BadRequest().StatusCode,
						Message = "File is not selected or empty."
					});

				//var containerName = "apifile"; // replace with your container name
				var uri = await _azureBlobStorageService.UploadFileAsync(containerName.ToString(), file);

				return Ok(new SucceededResponseModel()
				{
					Status = Ok().StatusCode,
					Message = "File uploaded successfully",
					Data = new
					{
						FileName = file.FileName,
						FileUri = uri
					}
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new FailedResponseModel
				{
					Status = 500,
					Message = $"An error occurred while uploading the file: {ex.Message}"
				});
			}
		}

		[HttpPost]
        public async Task<IActionResult> UploadArtAndWatermark(IFormFile artwork, IFormFile watermark)
		{
			var combinedImage = await FileHelper.CombineImages(artwork, watermark);
			try
			{
				var uri = await _azureBlobStorageService.UploadFileAsync("watermarkartwork", combinedImage);

				return Ok(new SucceededResponseModel()
				{
					Status = Ok().StatusCode,
					Message = "File uploaded successfully",
					Data = new
					{
						FileName = combinedImage.FileName,
						FileUri = uri
					}
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, new FailedResponseModel
				{
					Status = 500,
					Message = $"An error occurred while uploading the file: {ex.Message}"
				});
			}
		}
	}
}
