using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Http;
public static class FileHelper
{
    public static async Task<IFormFile> CombineImages(IFormFile image1, IFormFile image2)
    {
        if (image1 == null || image2 == null)
            throw new ArgumentNullException("Images cannot be null");

        if (!IsImage(image1) || !IsImage(image2))
            throw new ArgumentException("Files must be images");

        using (var stream1 = new MemoryStream())
        {
            await image1.CopyToAsync(stream1);
            using (var stream2 = new MemoryStream())
            {
                await image2.CopyToAsync(stream2);
                using (var img1 = Image.Load(stream1.ToArray()).CloneAs<Rgba32>())
                using (var img2 = Image.Load(stream2.ToArray()).CloneAs<Rgba32>())
                {
                    img1.Mutate(x => x.DrawImage(img2, new Point(0, 0), 1f));

                    var outputStream = new MemoryStream();
                    img1.SaveAsPng(outputStream);
                    outputStream.Seek(0, SeekOrigin.Begin);

                    var combinedImage = new FormFile(outputStream, 0, outputStream.Length, null, null)
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "image/png"
                    };

                    return combinedImage;
                }
            }
        }

    }

    private static bool IsImage(IFormFile file)
    {
        var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".bmp", ".svg" };
        return imageExtensions.Any(e => file.FileName.EndsWith(e, StringComparison.OrdinalIgnoreCase));
    }
}
