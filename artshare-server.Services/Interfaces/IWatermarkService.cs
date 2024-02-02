using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IWatermarkService
    {
        Task<IEnumerable<Watermark>> GetAllWatermarksAsync();

        Task<Watermark?> GetWatermarkByIdAsync(int watermarkId);

        Task<bool> CreateWatermarkAsync(Watermark watermark);

        Task<bool> UpdateWatermarkAsync(Watermark watermark);

        Task<bool> DeleteWatermarkAsync(int watermarkId);
    }
}