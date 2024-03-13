using artshare_server.Contracts.DTOs;
using artshare_server.WebAPI.ResponseModels;
using artshare_server.Services.CustomExceptions;
using artshare_server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using artshare_server.Core.Enums;

namespace artshare_server.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        public AuthController(IAuthService authService, IAccountService accountService, IAzureBlobStorageService azureBlobStorageService)
        {
            _authService = authService;
            _accountService = accountService;
            _azureBlobStorageService = azureBlobStorageService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginData)
        {
            try
            {
                var loginRequest = await _authService.LoginAsync(loginData);
                //return Ok(new SucceededResponseModel()
                //{
                //    Status = Ok().StatusCode,
                //    Message = "Success",
                //    Data = new
                //    {
                //        Token = loginRequest,
                //        Account = await _accountService.GetAccountByEmailAsync(loginData.Email)
                //    }
                //});
                return Ok(loginRequest);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new FailedResponseModel()
                {
                    Status = NotFound().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromQuery] AccountRole accounrRole, [FromBody] RegisterDTO registerRequest)
        {
            try
            {
                var requestResult = await _authService.RegisterAsync(accounrRole, registerRequest);
                if (!requestResult)
                {
                    return StatusCode(500, new FailedResponseModel
                    {
                        Status = 500,
                        Message = "Register failed."
                    });
                }
                return Ok(new SucceededResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Success",
                    Data = new
                    {
                        Account = await _accountService.GetAccountByEmailAsync(registerRequest.Email)
                    }
                });

            }
            catch (RegistrationException ex)
            {
                return Conflict(new FailedResponseModel()
                {
                    Status = Conflict().StatusCode,
                    Message = ex.Message
                });
            }
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


    }
}
