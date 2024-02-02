﻿using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;

namespace artshare_server.Services.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            var genreList = await _unitOfWork.GenreRepo.GetAllAsync();
            return genreList;
        }

        public async Task<Genre?> GetGenreByIdAsync(int genreId)
        {
            if (genreId > 0)
            {
                var genre = await _unitOfWork.GenreRepo.GetByIdAsync(genreId);
                return genre;
            }
            return null;
        }

        public async Task<bool> CreateGenreAsync(Genre genre)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateGenreAsync(Genre genre)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteGenreAsync(int genreId)
        {
            throw new NotImplementedException();
        }
    }
}