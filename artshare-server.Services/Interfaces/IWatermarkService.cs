using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IWatermarkService
    {
        Task<IEnumerable<Watermark>> GetAllWatermarksAsync();

        Task<Watermark?> GetWatermarkByIdAsync(int watermarkId);
        
        Task<Watermark?> GetByCreatorIdAsync(int creatorId);

        Task<WatermarkCreateDTO> CreateWatermarkAsync(WatermarkCreateDTO watermark);

        Task<WatermarkDTO> UpdateWatermarkAsync(int id, WatermarkDTO newWatermark);

        Task<bool> DeleteWatermarkAsync(int watermarkId);
    }
}