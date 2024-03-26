using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using AutoMapper;
using System.ComponentModel.Design;

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

        public async Task<bool> CreateCommentAsync(CreateCommentDTO _comment)
        {
            Comment comment= _mapper.Map<Comment>(_comment);

            await _unitOfWork.CommentRepo.AddAsync(comment);
            var result = await _unitOfWork.SaveAsync() > 0;
            return result;
        }

        public Task<bool> DeleteCommentAsync(int commentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetCommentDTO>> GetAllCommentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetCommentDTO>> GetCommentByArtworkId(int commentId)
        {
            if (commentId > 0)
            {
                var cmt = await _unitOfWork.CommentRepo.GetCommentByArtworkId(commentId);
                return cmt;
            }
            return null;
        }

        public async Task<List<GetCommentDTO>> GetCommentByIdAsync(int commentId)
        {
            if (commentId > 0)
            {
                var cmt = await _unitOfWork.CommentRepo.GetCommentByArtworkId(commentId);
                return _mapper.Map<List<GetCommentDTO>>(cmt);
            }
            return null;
        }

        public async Task<List<GetCommentDTO>> GetComments()
        {
            var cmt = await _unitOfWork.CommentRepo.GetComments();
            return _mapper.Map<List<GetCommentDTO>>(cmt);

        }

        public Task<bool> UpdateCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}