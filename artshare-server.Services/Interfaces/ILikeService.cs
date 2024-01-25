using artshare_server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
