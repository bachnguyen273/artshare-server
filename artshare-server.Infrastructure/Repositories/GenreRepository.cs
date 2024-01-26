using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;

namespace artshare_server.Infrastructure.Repositories
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}