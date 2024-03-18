using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WatermarkController : ControllerBase
    {
        private readonly IWatermarkService _watermarkService;

        public WatermarkController(IWatermarkService watermarkService)
        {
            _watermarkService = watermarkService;
        }

        [HttpGet]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> GetWatermakrs([FromQuery] WatermarkFilters filters)
        {
            try
            {
                var waterMarkList = await _watermarkService.GetAllWatermarksAsync(filters);
                if (waterMarkList == null)
                {
                    return BadRequest("List is null");
                }
                return Ok(waterMarkList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{creatorId}")]
        public async Task<IActionResult> GetByCreatorIdAsync(int creatorId)
        {
            try
            {
                var watermark = await _watermarkService.GetByCreatorIdAsync(creatorId);
                if (watermark == null)
                {
                    return NotFound();
                }
                return Ok(watermark);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWatermarkByIdAsync(int watermarkId)
        {
            try
            {
                var watermark = await _watermarkService.GetWatermarkByIdAsync(watermarkId);
                if (watermark == null)
                {
                    return NotFound();
                }
                return Ok(watermark);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CreateWatermarkAsync(CreateWatermarkDTO createWatermarkDTO)
        {         
            var watermark = await _watermarkService.CreateWatermarkAsync(createWatermarkDTO);
            return watermark;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatermarkAsync(int id, UpdateWatermarkDTO updateWatermarkDTO)
        {
            await _watermarkService.UpdateWatermarkAsync(id, updateWatermarkDTO);
            return Ok("Update SUCCESS!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatermark(int id)
        {
            try
            {
                var existedldWatermark = await _watermarkService.GetWatermarkByIdAsync(id);
                if (existedldWatermark == null)
                {
                    return NotFound(); ;
                }
                _watermarkService.DeleteWatermarkAsync(id);
                return Ok("Delete Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
