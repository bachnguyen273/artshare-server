using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportservice;
        public ReportController (IReportService reportservice)
        {
            _reportservice = reportservice;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllReport() 
        {
            var listReport = await _reportservice.GetAllReportsAsync();
            if (listReport == null)
            {
                return NotFound();
            }
            return Ok(listReport.OrderBy(r => r.ReportDate).Where(r => r.Status.Equals(ReportStatus.Processing)).ToList());
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateReportAsync(ReportDTO reportDTO)
        {
            var report = _reportservice.GetReportByAccountIdAndArtworkId(reportDTO.AccountId, reportDTO.ArtworkId);
            if (report != null)
            {
                return BadRequest("Already reported");
            }
            bool result = await _reportservice.CreateReportAsync(reportDTO);
            if (result)
            {
                return Ok();
            }return BadRequest("Create faile");
        }
    }
}
