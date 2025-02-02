using EventModsen.Configuration;
using Microsoft.Extensions.Options;
using EventModsen.Application.Interfaces;
using EventModsen.Domain.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace EventModsen.Application.Services
{
    public class ImageService(IOptions<ImageSettings> options, IHttpContextAccessor httpContextAccessor, IImageRepository _imageRepository) : IImageService
    {
        private readonly string _imagePath = options.Value.ImagePath;
        private readonly string _baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Images/";

        public async Task<string> SaveImageAsync(IFormFile file, int eventId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл отсутствует или пуст.");

            string eventFolderPath = Path.Combine(_imagePath, eventId.ToString());
            await Console.Out.WriteLineAsync(eventFolderPath);
            Directory.CreateDirectory(eventFolderPath);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(eventFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"{_baseUrl}{eventId}/{fileName}";

            await _imageRepository.AddImageUrlToEvent(eventId, imageUrl);

            return filePath;
        }

        public async Task<IEnumerable<ImageInfoDto>> GetAllEventImages(int eventId)
        {
            var images = await _imageRepository.GetAllImageUrlsFromEvent(eventId);
            //images.Select(i => i.Adapt<ImageInfoDto>());
            return images.Adapt<IEnumerable<ImageInfoDto>>();
        }

        public async Task RemoveAllEventImages(int eventId)
        {
            string eventFolderPath = Path.Combine(_imagePath, eventId.ToString());
            if(Directory.Exists(eventFolderPath))
            {
                Directory.Delete(eventFolderPath, true);
            }
            await _imageRepository.RemoveAllImagesFromEvent(eventId);

        }

    }
}
