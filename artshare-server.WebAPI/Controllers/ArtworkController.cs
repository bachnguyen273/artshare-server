using artshare_server.Contracts.DTOs;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using artshare_server.WebAPI.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using artshare_server.ApiModels.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using artshare_server.Services.CustomExceptions;
using artshare_server.Services.FilterModels;
using artshare_server.Core.Models;
using artshare_server.Core.Enums;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArtworkController : Controller
    {
        private readonly IArtworkService _artworkService;
        private readonly IAzureBlobStorageService _azureBlobStorageService;

        public ArtworkController(IArtworkService artworkService,
                                    IAzureBlobStorageService azureBlobStorageService)
        {
            _artworkService = artworkService;
            _azureBlobStorageService = azureBlobStorageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtworks([FromQuery] ArtworkFilters artworkFilter)
        {
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = new
                {
                    Artwork = await _artworkService.GetAllArtworksAsync<Artwork>(artworkFilter)
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetArtworkById(int id)
        {
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = new
                {
                    Artwork = await _artworkService.GetArtworkByIdAsync(id)
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetArtworkIdsByAccountId(int accountId)
        {
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = new
                {
                    ArtworkIds = await _artworkService.GetArtworkIdsByAccountIdAsync(accountId)
                }
            });
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> CreateArtwork([FromQuery] ArtworkStatus artworkStatus ,[FromBody] CreateArtworkDTO createArtworkDTO)
        {
            try
            {
                createArtworkDTO.Status = artworkStatus.ToString();
                var requestResult = await _artworkService.CreateArtworkAsync(createArtworkDTO);
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
                    Message = "Success"
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
