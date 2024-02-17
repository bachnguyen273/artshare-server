using artshare_server.Contracts.DTOs;
using artshare_server.ResponseModels;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArtworkController : Controller
    {
        private readonly IArtworkService _artworkService;

        public ArtworkController(IArtworkService artworkService)
        {
            _artworkService = artworkService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtworks()
        {
            return Ok(new SucceededResponseModel()
            {
                Status = Ok().StatusCode,
                Message = "Success",
                Data = new
                {
                    Artwork = await _artworkService.GetAllArtworksAsync()
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
    }
}
