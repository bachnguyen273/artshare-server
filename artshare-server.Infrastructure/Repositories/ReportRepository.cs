using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace artshare_server.Infrastructure.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        private readonly IMapper _mapper;
        public ReportRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public ReportRepository(AppDbContext dbContext, IMapper maper) : base(dbContext)
        {
            _mapper = maper;
        }

        public async Task<List<GetReportDTO>> GetAll()
        {
            var list = _dbContext.Reports
                            .Include(x => x.Artwork)
                            .Include(x => x.Account)
                            .ToList();
            return _mapper.Map<List<GetReportDTO>>(list);
        }

        public async Task<Report?> GetReportByAccountIdAndArtworkId(int accountId, int artworkId)
        {
            return _dbContext.Reports.FirstOrDefault(r => r.AccountId == accountId && r.ArtworkId == artworkId);
        }
    }
}