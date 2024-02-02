using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();

        Task<Comment?> GetCommentByIdAsync(int commentId);

        Task<bool> CreateCommentAsync(Comment comment);

        Task<bool> UpdateCommentAsync(Comment comment);

        Task<bool> DeleteCommentAsync(int commentId);
    }
}