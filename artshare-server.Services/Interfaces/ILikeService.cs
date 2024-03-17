using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface ILikeService
    {
        Task<IEnumerable<GetLikeDTO>> GetAllLikesAsync();

        Task<GetLikeDTO?> GetLikeByIdAsync(int likeId);

        Task<bool> CreateLikeAsync(Like like);

        Task<bool> UpdateLikeAsync(Like like);

        Task<bool> DeleteLikeAsync(int likeId);
    }
}