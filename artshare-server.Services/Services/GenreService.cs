using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;
using artshare_server.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace artshare_server.Services.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetGenreDTO> GetGenreByIdAsync(int genreId)
        {
            GetGenreDTO result;
            try
            {
                Genre dto = await _unitOfWork.GenreRepo.GetByIdAsync(genreId);
                result = _mapper.Map<GetGenreDTO>(dto);
                if (result == null)
                {
                    throw new Exception("No genre found");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateGenreAsync(CreateGenreDTO createGenreDTO)
        {
            try
            {
                Genre entity = _mapper.Map<Genre>(createGenreDTO);
                await _unitOfWork.GenreRepo.AddAsync(entity);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateGenreAsync(int genreId, UpdateGenreDTO updateGenreDTO)
        {
            try
            {
                Genre check = await _unitOfWork.GenreRepo.GetByIdAsync(genreId);
                if (check == null)
                {
                    throw new Exception("No genre found");
                }
                Genre dto = _mapper.Map<Genre>(updateGenreDTO);
                _unitOfWork.GenreRepo.Update(dto);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteGenreAsync(int genreId)
        {
            try
            {
                Genre dto = await _unitOfWork.GenreRepo.GetByIdAsync(genreId);
                if (dto == null)
                {
                    throw new Exception("No genre found");
                }
                _unitOfWork.GenreRepo.Delete(dto);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PagedResult<GetGenreDTO>> GetAllGenresAsync<Genre>(GenreFilters filters)
        {
            // Apply filtering
            var items = await _unitOfWork.GenreRepo.GetAllAsync();
            IEnumerable<GetGenreDTO> list = _mapper.Map<IEnumerable<GetGenreDTO>>(items);
            IQueryable<GetGenreDTO> filteredItemsQuery = list.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Name))
                filteredItemsQuery = filteredItemsQuery.Where(item => item.Name.Contains(filters.Name, StringComparison.OrdinalIgnoreCase));

            // Apply sorting
            if (!string.IsNullOrEmpty(filters.SortBy))
            {
                switch (filters.SortBy)
                {
                    default:
                        // Handle other sorting filters using Utils.GetPropertyValue
                        filteredItemsQuery = filters.SortAscending ?
                            filteredItemsQuery.OrderBy(item => Utils.GetPropertyValue(item, filters.SortBy)) :
                            filteredItemsQuery.OrderByDescending(item => Utils.GetPropertyValue(item, filters.SortBy));
                        break;
                }
            }

            // Materialize the query before paging
            var pagedItems = filteredItemsQuery
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToList(); // Materialize the query

            // Count total items without paging
            var totalItemsCount = filteredItemsQuery.Count();

            return new PagedResult<GetGenreDTO>
            {
                Items = pagedItems,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize,
                TotalItems = totalItemsCount
            };
        }

    }
}