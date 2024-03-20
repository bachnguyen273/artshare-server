using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace artshare_server.WebApp.Helpers
{
    public static class ImageHelpers
    {

        public static async Task<string> UploadOriginalArtworkFile(string apiUrl, IFormFile artworkFile)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                // upload original artwork

                var originalArtworkContent = new MultipartFormDataContent();
                originalArtworkContent.Add(new StreamContent(artworkFile.OpenReadStream()), "file", artworkFile.FileName);

                var originalArtworkResponse = await httpClient.PostAsync(apiUrl + "/File/UploadFile?containerName=1", originalArtworkContent);

                if (originalArtworkResponse.IsSuccessStatusCode)
                {
                    var responseContent = await originalArtworkResponse.Content.ReadAsStringAsync();
                    dynamic responseData = JsonConvert.DeserializeObject(responseContent);
                    return responseData.data.fileUri;
                }
                else
                {
                    var responseContent = await originalArtworkResponse.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to upload artwork: {originalArtworkResponse.StatusCode} - {responseContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<string> UploadWatermarkArtworkFile(string apiUrl,  
                                                                        IFormFile artworkFile)
        {
            var watermarkUrl = "\"https://artsharing.blob.core.windows.net/watermark/c9da60b1-48e9-4a0e-8175-7fb8e2105de4.jpg";
            HttpClient httpClient = new HttpClient();
            string combinedArtworkAndWatermarkUri = "";
            try
            {
                // Download watermark
                byte[] watermarkData = await httpClient.GetByteArrayAsync(watermarkUrl);
                MemoryStream watermarkStream = new MemoryStream(watermarkData);
                IFormFile watermarkFile = new FormFile(watermarkStream, 0, watermarkData.Length, "file", watermarkUrl);

                // Upload watermark file and artwork file to combine and send to azure
                using (var formData = new MultipartFormDataContent())
                {
                    // Add artwork file
                    formData.Add(new StreamContent(artworkFile.OpenReadStream())
                    {
                        Headers =
                        {
                            ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "artwork",
                                FileName = artworkFile.FileName
                            }
                        }
                    }, "artwork");

                    // Add watermark file
                    formData.Add(new StreamContent(watermarkFile.OpenReadStream())
                    {
                        Headers =
                        {
                            ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "watermark",
                                FileName = watermarkFile.FileName
                            }
                        }
                    }, "watermark");

                    // Call the API endpoint
                    using (var response = await httpClient.PostAsync(apiUrl + "/File/UploadArtAndWatermark", formData))
                    {
                        response.EnsureSuccessStatusCode(); // Ensure success status code

                        // Read response content
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Parse response JSON
                        dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                        return responseData.data.fileUri;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
