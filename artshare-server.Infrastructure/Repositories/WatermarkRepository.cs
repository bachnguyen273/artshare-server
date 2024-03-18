using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class WatermarkRepository : GenericRepository<Watermark>, IWatermarkRepository
    {
        private readonly IMapper _mapper;
        public WatermarkRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public WatermarkRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }
        public async Task<Watermark?> GetByCreatorIdAsync(int id)
        {
            return await _dbContext.Watermarks.Where(t => t.CreatorId == id).FirstOrDefaultAsync();
        }
    }
}