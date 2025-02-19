using Application.Configuration;
using Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.FileStorage;

public class ImageService(IOptions<ImageSettings> options) : IImageService
{

    private readonly string _imagePath = options.Value.ImagePath;
    public async Task<string> SaveImage(IFormFile image, int eventId, CancellationToken cancelToken)
    {
        if (image == null || image.Length == 0)
            throw new ArgumentException("File does not exist or empty");

        var allowedImageTypes = new[] { "image/jpeg", "image/png" };
        if (!allowedImageTypes.Contains(image.ContentType))
        {
            throw new InvalidOperationException("The file must be an image (jpeg, png).");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(image.FileName)?.ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new InvalidOperationException("The file must have one of the following extensions: .jpg, .jpeg, .png.");
        }

        const long maxFileSizeInBytes = 5 * 1024 * 1024;
        if (image.Length > maxFileSizeInBytes)
        {
            throw new InvalidOperationException("The file size must not exceed 5MB.");
        }

        string eventFolderPath = Path.Combine(_imagePath, eventId.ToString());
        await Console.Out.WriteLineAsync(eventFolderPath);
        Directory.CreateDirectory(eventFolderPath);

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        string filePath = Path.Combine(eventFolderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream, cancelToken);
        }

        var partitionImageUrl = $"{eventId}/{fileName}";

        return partitionImageUrl;

    }
    public void RemoveAllImageFilesFromEvent(int eventId)
    {
        string eventFolderPath = Path.Combine(_imagePath, eventId.ToString());

        if (Directory.Exists(eventFolderPath))
        {
            Directory.Delete(eventFolderPath, true);
        }
    }
}
