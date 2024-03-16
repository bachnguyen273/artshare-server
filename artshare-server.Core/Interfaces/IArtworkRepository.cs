using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IArtworkRepository : IGenericRepository<Artwork>
    {
        Task<List<GetArtworkDTO>> GetArtworks();
    }
}