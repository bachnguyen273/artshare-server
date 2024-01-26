using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;

namespace artshare_server.Services.Services
{
    public class WatermarkService : IWatermarkService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WatermarkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Watermark>> GetAllWatermarksAsync()
        {
            var watermarkList = await _unitOfWork.WatermarkRepo.GetAllAsync();
            return watermarkList;
        }

        public async Task<Watermark> GetWatermarkByIdAsync(int watermarkId)
        {
            if (watermarkId > 0)
            {
                var watermark = await _unitOfWork.WatermarkRepo.GetByIdAsync(watermarkId);
                if (watermark != null)
                {
                    return watermark;
                }
            }
            return null;
        }

        public async Task<bool> CreateWatermarkAsync(Watermark watermark)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWatermarkAsync(Watermark watermark)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteWatermarkAsync(int watermarkId)
        {
            throw new NotImplementedException();
        }
    }
}