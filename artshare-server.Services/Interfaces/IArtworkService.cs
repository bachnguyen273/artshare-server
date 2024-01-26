using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IArtworkService
    {
        Task<IEnumerable<Artwork>> GetAllArtworksAsync();

        Task<Artwork> GetArtworkByIdAsync(int artworkId);

        Task<bool> CreateArtworkAsync(Artwork artwork);

        Task<bool> UpdateArtworkAsync(Artwork artwork);

        Task<bool> DeleteArtworkAsync(int artworkId);
    }
}