using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<GetCommentDTO>> GetAllCommentsAsync();
        Task<bool> CreateCommentAsync(CreateCommentDTO comment);

        Task<bool> UpdateCommentAsync(Comment comment);

        Task<bool> DeleteCommentAsync(int commentId);

        Task<List<GetCommentDTO>> GetCommentByArtworkId(int id);
        Task<List<GetCommentDTO>> GetComments();
    }
}