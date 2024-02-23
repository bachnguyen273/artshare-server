using artshare_server.Contracts.DTOs;
using artshare_server.WebAPI.ResponseModels;
using artshare_server.Services.CustomExceptions;
using artshare_server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        public AuthController(IAuthService authService, IAccountService accountService)
        {
            _authService = authService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginData)
        {
            try
            {
                var loginRequest = await _authService.LoginAsync(loginData);
                return Ok(new SucceededResponseModel()
                {
                    Status = Ok().StatusCode,
                    Message = "Success",
                    Data = new
                    {
                        Token = loginRequest,
                        Account = await _accountService.GetAccountByEmailAsync(loginData.Email)
                    }
                });
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
        public async Task<IActionResult> Register(RegisterDTO registerRequest)
        {
            try
            {
                var requestResult = await _authService.RegisterAsync(registerRequest);
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
    }
}
