using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;

namespace artshare_server.Services.Interfaces
{
    public interface IArtworkService
    {
        //Task<IEnumerable<Artwork>> GetAllArtworksAsync();
        Task<PagedResult<Artwork>> GetAllArtworksAsync<T>(ArtworkFilters filter);
        Task<Artwork?> GetArtworkByIdAsync(int artworkId);

        //Task<bool> CreateArtworkAsync(Artwork artwork);
        Task<bool> CreateArtworkAsync(CreateArtworkDTO createArtworkDTO);
        Task<bool> UpdateArtworkAsync(Artwork artwork);

        Task<bool> DeleteArtworkAsync(int artworkId);
    }
}