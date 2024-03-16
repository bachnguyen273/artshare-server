using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;

namespace artshare_server.Services.Interfaces
{
    public interface IWatermarkService
    {
        Task<PagedResult<GetWatermarkDTO>> GetAllWatermarksAsync(WatermarkFilters filters);

        Task<GetWatermarkDTO?> GetWatermarkByIdAsync(int watermarkId);
        
        Task<GetWatermarkDTO?> GetByCreatorIdAsync(int creatorId);

        Task<bool> CreateWatermarkAsync(CreateWatermarkDTO createWatermarkDTO);

        Task<bool> UpdateWatermarkAsync(int id, UpdateWatermarkDTO updateWatermarkDTO);

        Task<bool> DeleteWatermarkAsync(int watermarkId);
    }
}