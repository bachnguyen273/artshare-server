using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        public LikeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<int> CountDisLikeByArtWorkId(int artworkId)
        {
            return _dbContext.Likes.Where(l => l.ArtworkId.Equals(artworkId) && l.IsLike==false).Count();
        }

        public async Task<int> CountLikeByArtWorkId(int artworkId)
        {
            return _dbContext.Likes.Where(l => l.ArtworkId.Equals(artworkId) && l.IsLike==true).Count();
        }

        public async Task<IEnumerable<Like>> GetAllLikeByArtworkId(int artworkId)
        {
            return await _dbContext.Likes.Where(l => l.ArtworkId == artworkId).ToListAsync();
        }

        public async Task<Like> GetLikeByAccountIdAndArtworkId(int accountId, int artworkId)
        {
            return _dbContext.Likes.FirstOrDefault(l => l.AccountId == accountId && l.ArtworkId.Equals(artworkId));
        }
    }
}