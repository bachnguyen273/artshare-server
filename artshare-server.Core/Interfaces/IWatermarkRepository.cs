﻿using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IWatermarkRepository : IGenericRepository<Watermark>
    {
        Task<Watermark?> GetByCreatorIdAsync(int id);
    }    
}