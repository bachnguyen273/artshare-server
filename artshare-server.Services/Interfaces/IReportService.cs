using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<GetReportDTO>> GetAllReportsAsync();

        Task<GetReportDTO?> GetReportByIdAsync(int reportId);

        Task<bool> CreateReportAsync(Report report);

        Task<bool> UpdateReportAsync(Report report);

        Task<bool> DeleteReportAsync(int reportId);
    }
}