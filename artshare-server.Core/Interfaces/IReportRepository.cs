using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<Report?> GetReportByAccountIdAndArtworkId(int accountId, int artworkId);
        Task<List<GetReportDTO>> GetAll();
    }
}