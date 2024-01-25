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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            var commentList = await _unitOfWork.CommentRepo.GetAllAsync();
            return commentList;
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            if (commentId > 0)
            {
                var comment = await _unitOfWork.CommentRepo.GetByIdAsync(commentId);
                if (comment != null)
                {
                    return comment;
                }
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
