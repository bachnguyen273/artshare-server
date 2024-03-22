using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly IMapper _mapper;
        public CommentRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }
        public async Task<List<GetCommentDTO>> GetComments()
        {
            var comment = await _dbContext.Comments
                            .Include(x => x.Commenter)
                            .ToListAsync();
            return _mapper.Map<List<GetCommentDTO>>(comment);
        }
        public async Task<List<GetCommentDTO>> GetCommentByArtworkId(int id)
        {
            var comment = await _dbContext.Comments
                            .Include(x => x.Commenter)
                            .FirstOrDefaultAsync(x => x.ArtworkId == id);
            return _mapper.Map<List<GetCommentDTO>>(comment);
        }

        
    }
}