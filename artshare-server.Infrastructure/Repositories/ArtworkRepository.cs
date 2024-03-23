using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class ArtworkRepository : GenericRepository<Artwork>, IArtworkRepository
    {
        private readonly IMapper _mapper;
        public ArtworkRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public ArtworkRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<List<GetArtworkDTO>> GetArtworks()
        {
            var list = await _dbContext.Artworks
                        .Include(x => x.Likes)
                        .Include(x => x.Comments)
                        .Include(x => x.Reports)
                        .Include(x => x.Genre)
                        .ToListAsync();
            return _mapper.Map<List<GetArtworkDTO>>(list);
        }

        public async Task<GetArtworkDTO> GetArtworkById(int id)
        {
            var artwork = await _dbContext.Artworks
                            .Include(x => x.Likes)
                            .Include(x => x.Comments)
                            .Include(x => x.Reports)
                            .Include(x => x.Genre)
                            .FirstOrDefaultAsync(x => x.ArtworkId == id);
            return _mapper.Map<GetArtworkDTO>(artwork);
        }
        public async Task<List<GetArtworkDTO>> GetArtworksByCreatorId(int id)
        {
            var artwork = await _dbContext.Artworks.Where(x =>x.CreatorId == id).ToListAsync();
            return _mapper.Map<List<GetArtworkDTO>>(artwork);
            
        }

        public async Task<IEnumerable<TopSaleArtwork>?> GetTopSaleArtwork(int creatorId)
        {
            var topSaleArtworks = (from order in _dbContext.Orders
                          join artwork in _dbContext.Artworks on order.ArtworkId equals artwork.ArtworkId
                          join account in _dbContext.Accounts on artwork.CreatorId equals account.AccountId
                          group artwork by new { artwork.ArtworkId, artwork.Title, artwork.CreatorId } into groupedArtworks
                          orderby groupedArtworks.Count() descending
                          select new TopSaleArtwork
                          {
                              CreatorId = groupedArtworks.Key.CreatorId,
                              Title = groupedArtworks.Key.Title,
                              ArtworkCount = groupedArtworks.Count()
                          }).Take(5).ToList();
            var topSaleArtworksOfCreator = topSaleArtworks.Where(x => x.CreatorId == creatorId).AsEnumerable();
            return topSaleArtworksOfCreator;
        }
    }
}

