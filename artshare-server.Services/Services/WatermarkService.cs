using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
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
        private readonly IMapper _mapper;

        public WatermarkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Watermark>> GetAllWatermarksAsync()
        {
            var watermarkList = await _unitOfWork.WatermarkRepo.GetAllAsync();
            return watermarkList;
        }

        public async Task<Watermark?> GetWatermarkByIdAsync(int watermarkId)
        {
            if (watermarkId > 0)
            {
                var watermark = await _unitOfWork.WatermarkRepo.GetByIdAsync(watermarkId);
                return watermark;
            }
            return null;
        }

        public async Task<Watermark?> GetByCreatorIdAsync(int creatorId)
        {
            var watermark = await _unitOfWork.WatermarkRepo.GetByCreatorIdAsync(creatorId);
            return watermark;
        }

        public async Task<WatermarkCreateDTO> CreateWatermarkAsync(WatermarkCreateDTO postedWatermark)
        {
                var watermark = _mapper.Map<Watermark>(postedWatermark);
                await _unitOfWork.WatermarkRepo.AddAsync(watermark);
                bool result = await _unitOfWork.SaveAsync() > 0;
                if (result) { return postedWatermark; }
                return null;
        }        

        public async Task<WatermarkDTO> UpdateWatermarkAsync(int id, WatermarkDTO newWatermark)
        {
            var oldWatermark = await _unitOfWork.WatermarkRepo.GetByIdAsync(id);
            oldWatermark.WatermarkUrl = newWatermark.WatermarkUrl;
            _unitOfWork.WatermarkRepo.Update(oldWatermark);
            bool result =  await _unitOfWork.SaveAsync() > 0;
            if (result == true)
            {
                var returnObject = _mapper.Map<WatermarkDTO>(oldWatermark);
                return returnObject;
            }
            return null;
        }

        public async Task<bool> DeleteWatermarkAsync(int watermarkId)
        {
            var watermark = await _unitOfWork.WatermarkRepo.GetByIdAsync(watermarkId);
            _unitOfWork.WatermarkRepo.Delete(watermark);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}