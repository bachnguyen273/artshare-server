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
                        .Include(x => x.Creator)
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
                            .Include(x => x.Creator)
                            .FirstOrDefaultAsync(x => x.ArtworkId == id);
            return _mapper.Map<GetArtworkDTO>(artwork);
        }
        public async Task<List<GetArtworkDTO>> GetArtworksByCreatorId(int id)
        {
            var artwork = await _dbContext.Artworks.Where(x =>x.CreatorId == id).ToListAsync();
            return _mapper.Map<List<GetArtworkDTO>>(artwork);
            
        }
    }
}