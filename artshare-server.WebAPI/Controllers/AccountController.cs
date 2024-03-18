using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Interfaces;
using artshare_server.Services.Interfaces;
using artshare_server.WebAPI.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAzureBlobStorageService _azureBlobStorageService;

        public AccountController(IAccountService accountService, IAzureBlobStorageService azureBlobStorageService)
        {
            _accountService = accountService;
            _azureBlobStorageService = azureBlobStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccount()
        {
            try
            {
                var accList = await _accountService.GetAllAccountsAsync();
                if(accList == null)
                {
                    return BadRequest("List is null");
                }
                return Ok(accList);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetAccountByUsername(string username)
        {
            try
            {
                var account = await _accountService.GetAccountByUsernameAsync(username);
                if (account == null || account.Status.Equals(AccountStatus.Inactive))
                {
                    return NotFound();
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById (int id)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound();
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile (int id, ProfileDTO profileDTO)
        {
            try
            {
                var check = await _accountService.GetAccountByIdAsync (id);
                if (check == null)
                {
                    return NotFound();
                }
                var up = await _accountService.UpdateAccountAsync(id,profileDTO);
                if (up)
                {
                    return Ok("Update SUCCESS!");
                }
                return BadRequest("Update FAIL");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var check = await _accountService.UpdateAccountStatuslAsync(id);
                if (!check)
                {
                    return BadRequest("Delete Fail");
                }
                return Ok("Delete Success");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileAvatar(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new FailedResponseModel()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "File is not selected or empty."
                    });

                var containerName = "avatar"; // replace with your container name
                var uri = await _azureBlobStorageService.UploadFileAsync(containerName, file);

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

        [HttpGet("{username}")]
        public async Task<IActionResult> SearchByUsername(string username)
        {
            try
            {
                var acc = await _accountService.GetAccountByUsernameAsync(username);
                if(acc == null)
                {
                    return NotFound();
                }
                return Ok(acc);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
