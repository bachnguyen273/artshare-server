using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly IMapper _mapper;
        public GenreRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public GenreRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<List<GetGenreDTO>> GetGenres()
        {
            var list = await _dbContext.Genres
               .Include(x => x.Artworks)
               .ToListAsync();
            return _mapper.Map<List<GetGenreDTO>>(list);
        }
    }
}