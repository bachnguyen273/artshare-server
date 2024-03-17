using artshare_server.ApiModels.DTOs;
using artshare_server.Services.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<string> UploadFileAsync(string containerName, IFormFile file)
        {
            try
            {
                // Get a reference to a container
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                // Create the container if it does not exist.
                await containerClient.CreateIfNotExistsAsync();

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + ".jpg");

                // Open the file and upload its data
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // Return the URI of the blob (file)
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
