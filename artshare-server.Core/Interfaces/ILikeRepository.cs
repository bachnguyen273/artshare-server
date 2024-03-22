using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface ILikeRepository : IGenericRepository<Like>
    {
        Task<IEnumerable<Like>> GetAllLikeByArtworkId(int artworkId);
        Task<Like> GetLikeByAccountIdAndArtworkId(int accountId, int artworkId);
        Task<int> CountLikeByArtWorkId(int artworkId);
        Task<int> CountDisLikeByArtWorkId(int artworkId);
    }
}