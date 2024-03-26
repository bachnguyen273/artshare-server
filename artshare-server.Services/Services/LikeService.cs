using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;

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

        public async Task<Like?> GetLikeByIdAsync(int likeId)
        {
            if (likeId > 0)
            {
                var like = await _unitOfWork.LikeRepo.GetByIdAsync(likeId);
                return like;
            }
            return null;
        }

        public async Task<bool> CreateLikeAsync(Like like)
        {
            try
            {
                _unitOfWork.LikeRepo.AddAsync(like);
                var result = await _unitOfWork.SaveAsync() > 0;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateLikeAsync(Like like)
        {
            try
            {
                _unitOfWork.LikeRepo.Update(like);
                var result = await _unitOfWork.SaveAsync() > 0;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLikeAsync(Like like)
        {
            try
            {
                _unitOfWork.LikeRepo.Delete(like);
                var result = await _unitOfWork.SaveAsync() > 0;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Like>> GetAllLikeByArtworkId(int artworkId)
        {
            return await _unitOfWork.LikeRepo.GetAllLikeByArtworkId(artworkId);
        }

        public async Task<Like> GetLikeByAccountIdAndArtworkId(int accountId, int artworkId)
        {
            return await _unitOfWork.LikeRepo.GetLikeByAccountIdAndArtworkId(accountId, artworkId);
        }

        public async Task<int> CountLikeByArtWorkId(int artworkId)
        {
            return await _unitOfWork.LikeRepo.CountLikeByArtWorkId(artworkId);
        }

        public async Task<int> CountDisLikeByArtWorkId(int artworkId)
        {
            return await _unitOfWork.LikeRepo.CountDisLikeByArtWorkId(artworkId);
        }


    }
}