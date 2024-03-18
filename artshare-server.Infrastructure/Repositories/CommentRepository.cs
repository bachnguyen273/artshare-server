using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        protected readonly AppDbContext _dbContext;
        public CommentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsByArtworkIdAsync(int artworkId)
        {
            return await _dbContext.Comments.Where(c => c.ArtworkId == artworkId).ToListAsync();
        }
    }
}