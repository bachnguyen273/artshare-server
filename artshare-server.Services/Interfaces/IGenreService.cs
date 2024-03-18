using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;

namespace artshare_server.Services.Interfaces
{
    public interface IGenreService
    {
        // READ
        Task<PagedResult<GetGenreDTO>> GetAllGenresAsync<Genre>(GenreFilters genreFilter);

        Task<GetGenreDTO> GetGenreByIdAsync(int genreId);

        // CREATE
        Task<bool> CreateGenreAsync(CreateGenreDTO genre);

        // UPDATE
        Task<bool> UpdateGenreAsync(int genreID, UpdateGenreDTO genre);

        // DELETE
        Task<bool> DeleteGenreAsync(int genreId);
    }
}