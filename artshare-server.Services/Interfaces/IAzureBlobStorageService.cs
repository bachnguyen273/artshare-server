using artshare_server.ApiModels.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.Interfaces
{
    public interface IAzureBlobStorageService
    {
        // tạo ra 4 container 
        // 1. Avatar
        // 2. OrigialArtwork
        // 3. Watermark
        // 4. WatermarkArtwork
        Task<string> UploadFileAsync(string containerName, IFormFile file);
    }
}
