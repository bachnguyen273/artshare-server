using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Models;
using artshare_server.Services.CustomExceptions;
using artshare_server.Services.FilterModels;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using artshare_server.WebAPI.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenre([FromQuery] GenreFilters genreFilters)
        {
            try
            {
                var accList = await _genreService.GetAllGenresAsync<Genre>(genreFilters);
                if (accList == null)
                {
                    return BadRequest("List is null");
                }
                return Ok(accList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetGenreById(int id)
        {
            try
            {
                var account = await _genreService.GetGenreByIdAsync(id);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] UpdateGenreDTO updateGenreDTO)
        {
            try
            {
                var check = await _genreService.GetGenreByIdAsync(id);
                if (check == null)
                {
                    return NotFound();
                }
                var up = await _genreService.UpdateGenreAsync(id, updateGenreDTO);
                if (up)
                {
                    return Ok("Update SUCCESS!");
                }
                return BadRequest("Update FAIL");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var check = await _genreService.DeleteGenreAsync(id);
                if (!check)
                {
                    return BadRequest("Delete Fail");
                }
                return Ok("Delete Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDTO createArtworkDTO)
        {
            try
            {
                var requestResult = await _genreService.CreateGenreAsync(createArtworkDTO);
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