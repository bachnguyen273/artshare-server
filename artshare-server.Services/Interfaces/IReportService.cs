using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<GetReportDTO>> GetAllReportsAsync();

        Task<Report?> GetReportByIdAsync(int reportId);
        Task<Report?> GetReportByAccountIdAndArtworkId(int accountId, int artworkId);

        Task<bool> CreateReportAsync(ReportDTO reportDTO);

        Task<bool> UpdateReportAsync(Report report);

        Task<bool> DeleteReportAsync(int reportId);
        Task<bool> DenyReport(int reportId);
        Task<bool> AcceptReport(int reportId);
    }
}