using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportservice;
        public ReportController(IReportService reportservice)
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
            return Ok(listReport.OrderBy(r => r.ReportDate));
        }

        [HttpPost]
        public async Task<IActionResult> CreateReportAsync(ReportDTO reportDTO)
        {
            var report = _reportservice.GetReportByAccountIdAndArtworkId(reportDTO.AccountId, reportDTO.ArtworkId).Result;
            if (report != null)
            {
                return BadRequest("Already reported");
            }
            bool result = await _reportservice.CreateReportAsync(reportDTO);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Create faile");
        }
        [HttpPut]
        public async Task<IActionResult> DenyReport(int id)
        {

            bool result = await _reportservice.DenyReport(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Update faile");
        }
        [HttpPut]
        public async Task<IActionResult> AcceptReport(int id)
        {

            bool result = await _reportservice.AcceptReport(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Update faile");
        }
    }
}
