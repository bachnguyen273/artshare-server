using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        Task<List<GetGenreDTO>> GetGenres();
    }
}