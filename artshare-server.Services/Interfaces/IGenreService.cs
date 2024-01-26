using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();

        Task<Genre> GetGenreByIdAsync(int genreId);

        Task<bool> CreateGenreAsync(Genre genre);

        Task<bool> UpdateGenreAsync(Genre genre);

        Task<bool> DeleteGenreAsync(int genreId);
    }
}