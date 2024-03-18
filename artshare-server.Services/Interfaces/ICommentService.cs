using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<GetCommentDTO>> GetAllCommentsAsync();

        Task<GetCommentDTO?> GetCommentByIdAsync(int commentId);

        Task<bool> CreateCommentAsync(Comment comment);

        Task<bool> UpdateCommentAsync(Comment comment);

        Task<bool> DeleteCommentAsync(int commentId);
    }
}