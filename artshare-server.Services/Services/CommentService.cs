using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using AutoMapper;

namespace artshare_server.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCommentDTO>> GetAllCommentsAsync()
        {
            var commentList = await _unitOfWork.CommentRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<GetCommentDTO>>(commentList);
        }

        public async Task<GetCommentDTO?> GetCommentByIdAsync(int commentId)
        {
            if (commentId > 0)
            {
                var comment = await _unitOfWork.CommentRepo.GetByIdAsync(commentId);
                return _mapper.Map<GetCommentDTO>(comment);
            }
            return null;
        }

        public async Task<bool> CreateCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            throw new NotImplementedException();
        }
    }
}