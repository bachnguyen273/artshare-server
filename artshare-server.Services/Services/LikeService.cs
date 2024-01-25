using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Like>> GetAllLikesAsync()
        {
            var likeList = await _unitOfWork.LikeRepo.GetAllAsync();
            return likeList;
        }

        public async Task<Like> GetLikeByIdAsync(int likeId)
        {
            if (likeId > 0)
            {
                var like = await _unitOfWork.LikeRepo.GetByIdAsync(likeId);
                if (like != null)
                {
                    return like;
                }
            }
            return null;
        }

        public async Task<bool> CreateLikeAsync(Like like)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateLikeAsync(Like like)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteLikeAsync(int likeId)
        {
            throw new NotImplementedException();
        }
    }
}
