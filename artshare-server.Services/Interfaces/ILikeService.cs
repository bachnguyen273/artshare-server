using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface ILikeService
    {
        Task<IEnumerable<Like>> GetAllLikesAsync();

        Task<Like> GetLikeByIdAsync(int likeId);

        Task<bool> CreateLikeAsync(Like like);

        Task<bool> UpdateLikeAsync(Like like);

        Task<bool> DeleteLikeAsync(int likeId);
    }
}