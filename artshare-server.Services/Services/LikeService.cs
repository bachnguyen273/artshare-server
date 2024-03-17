using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using AutoMapper;

namespace artshare_server.Services.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetLikeDTO>> GetAllLikesAsync()
        {
            var likeList = await _unitOfWork.LikeRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<GetLikeDTO>>(likeList);
        }

        public async Task<GetLikeDTO?> GetLikeByIdAsync(int likeId)
        {
            if (likeId > 0)
            {
                var like = await _unitOfWork.LikeRepo.GetByIdAsync(likeId);
                return _mapper.Map<GetLikeDTO>(like);
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