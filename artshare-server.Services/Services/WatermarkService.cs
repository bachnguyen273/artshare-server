using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;
using artshare_server.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace artshare_server.Services.Services
{
    public class WatermarkService : IWatermarkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly IMapper _mapper;

        public WatermarkService(IUnitOfWork unitOfWork, IMapper mapper, IAzureBlobStorageService azureBlobStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureBlobStorageService = azureBlobStorageService;
        }

        public async Task<PagedResult<GetWatermarkDTO>> GetAllWatermarksAsync(WatermarkFilters filters)
        {
			// Apply filtering
			var items = _mapper.Map<IEnumerable<GetWatermarkDTO>>(await _unitOfWork.WatermarkRepo.GetAllAsync());
			IQueryable<GetWatermarkDTO> filteredItemsQuery = items.AsQueryable();

			if (filters.CreatorId != null)
				filteredItemsQuery = filteredItemsQuery.Where(item => item.CreatorId == filters.CreatorId);
			if (filters.WatermarkId != null)
				filteredItemsQuery = filteredItemsQuery.Where(item => item.WatermarkId == filters.WatermarkId);

			// Apply sorting
			if (!string.IsNullOrEmpty(filters.SortBy))
			{
				switch (filters.SortBy)
				{
					default:
						// Handle other sorting filter using Utils.GetPropertyValue
						filteredItemsQuery = filters.SortAscending ?
							filteredItemsQuery.OrderBy(item => Utils.GetPropertyValue(item, filters.SortBy)) :
							filteredItemsQuery.OrderByDescending(item => Utils.GetPropertyValue(item, filters.SortBy));
						break;
				}
			}

			// Apply paging
			var pagedItems = filteredItemsQuery
				.Skip((filters.PageNumber - 1) * filters.PageSize)
				.Take(filters.PageSize)
				.ToList(); // Materialize the query

			return new PagedResult<GetWatermarkDTO>
			{
				Items = pagedItems,
				PageNumber = filters.PageNumber,
				PageSize = filters.PageSize,
				TotalItems = pagedItems.Count()
			};
		}
        public async Task<GetWatermarkDTO?> GetWatermarkByIdAsync(int watermarkId)
        {
            if (watermarkId > 0)
            {
                var watermark = await _unitOfWork.WatermarkRepo.GetByIdAsync(watermarkId);
                return _mapper.Map<GetWatermarkDTO>(watermark);
            }
            return null;
        }

        public async Task<GetWatermarkDTO?> GetByCreatorIdAsync(int creatorId)
        {
            var watermark = await _unitOfWork.WatermarkRepo.GetByCreatorIdAsync(creatorId);
            return _mapper.Map<GetWatermarkDTO>(watermark);
        }  

        public async Task<bool> UpdateWatermarkAsync(int id, UpdateWatermarkDTO updateWatermarkDTO)
        {
            var oldWatermark = await _unitOfWork.WatermarkRepo.GetByIdAsync(id);
            if (updateWatermarkDTO.WatermarkFile != null)
            {
                oldWatermark.WatermarkUrl = await _azureBlobStorageService.UploadFileAsync("Watermark", updateWatermarkDTO.WatermarkFile);
            }
            _unitOfWork.WatermarkRepo.Update(oldWatermark);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> DeleteWatermarkAsync(int watermarkId)
        {
            var watermark = await _unitOfWork.WatermarkRepo.GetByIdAsync(watermarkId);
            _unitOfWork.WatermarkRepo.Delete(watermark);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> CreateWatermarkAsync(CreateWatermarkDTO createWatermarkDTO)
        {
            var watermark = _mapper.Map<Watermark>(createWatermarkDTO);
            await _unitOfWork.WatermarkRepo.AddAsync(watermark);
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}