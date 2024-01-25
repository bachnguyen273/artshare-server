using artshare_server.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();

        Task<Comment> GetCommentByIdAsync(int commentId);

        Task<bool> CreateCommentAsync(Comment comment);

        Task<bool> UpdateCommentAsync(Comment comment);

        Task<bool> DeleteCommentAsync(int commentId);
    }
}
