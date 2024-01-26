using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;

namespace artshare_server.Infrastructure.Repositories
{
    public class WatermarkRepository : GenericRepository<Watermark>, IWatermarkRepository
    {
        public WatermarkRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}