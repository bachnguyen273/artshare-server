using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;

namespace artshare_server.Infrastructure.Repositories
{
    public class ArtworkRepository : GenericRepository<Artwork>, IArtworkRepository
    {
        public ArtworkRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}