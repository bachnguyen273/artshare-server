using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;

namespace artshare_server.Infrastructure.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Report?> GetReportByAccountIdAndArtworkId(int accountId, int artworkId)
        {
            return _dbContext.Reports.FirstOrDefault(r => r.AccountId == accountId && r.ArtworkId == artworkId);
        }
    }
}