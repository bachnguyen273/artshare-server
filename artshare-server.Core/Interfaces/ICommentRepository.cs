using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<GetCommentDTO>> GetCommentByArtworkId(int id);
        Task<List<GetCommentDTO>> GetComments();
    }
}