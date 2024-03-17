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
                BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

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

        public async Task<string> UploadFileAsync(string artworkUrl, string watermarkUrl)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Download artwork image
                byte[] artworkBytes = await DownloadImageAsync(artworkUrl);

                // Download watermark image
                byte[] watermarkBytes = await DownloadImageAsync(watermarkUrl);

                // Combine images
                Image combinedImage = CombineImages(artworkBytes, watermarkBytes);

                // Save the combined image to a memory stream
                combinedImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                memoryStream.Position = 0;

                // Upload combined image to Azure Blob Storage
                return await UploadToAzureBlobAsync(memoryStream, "watermark_" + artworkUrl);
            }
        }

        async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                return await webClient.DownloadDataTaskAsync(new Uri(imageUrl));
            }
        }

        static Image CombineImages(byte[] imageBytes, byte[] watermarkBytes)
        {
            using (MemoryStream imageStream = new MemoryStream(imageBytes))
            using (MemoryStream watermarkStream = new MemoryStream(watermarkBytes))
            {
                // Load images from streams
                Image image = Image.FromStream(imageStream);
                Image watermark = RemoveBackground(Image.FromStream(watermarkStream)); // Remove background from watermark

                // Create a bitmap large enough to hold the image
                Bitmap combinedImage = new Bitmap(image.Width, image.Height);

                using (Graphics g = Graphics.FromImage(combinedImage))
                {
                    // Draw the image
                    g.DrawImage(image, 0, 0);

                    // Calculate the position to draw the watermark (e.g., bottom right corner with some padding)
                    int x = image.Width - watermark.Width - 10; // 10 pixels from the right
                    int y = image.Height - watermark.Height - 10; // 10 pixels from the bottom

                    // Draw the watermark image
                    g.DrawImage(watermark, new Rectangle(x, y, watermark.Width, watermark.Height));
                }

                return combinedImage;
            }
        }

        // Function to remove background from watermark image
        static Bitmap RemoveBackground(Image image)
        {
            // Convert image to bitmap
            Bitmap bitmap = new Bitmap(image);

            // Threshold value for separating background and foreground
            int threshold = 100; // Adjust as needed

            // Loop through each pixel and set it to transparent if below threshold
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    if (pixelColor.GetBrightness() < threshold / 255.0)
                    {
                        bitmap.SetPixel(x, y, Color.Transparent);
                    }
                }
            }

            return bitmap;
        }



        async Task<string> UploadToAzureBlobAsync(Stream stream, string fileName)
        {
            // Create a blob service client
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("watermarkartwork");

            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Upload the stream to the blob
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }
    }
}
