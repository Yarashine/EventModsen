using EventModsen.Configuration;
using Microsoft.Extensions.Options;
using EventModsen.Application.Interfaces;
using EventModsen.Domain.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;
using Mapster;
using Microsoft.AspNetCore.Http;
using EventModsen.Domain.Exceptions;
using EventModsen.Application.DTOs.RequestDto;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace EventModsen.Application.Services
{
    public class ImageService(IEventRepository _eventRepository,
                                IOptions<ImageSettings> options, 
                                IHttpContextAccessor httpContextAccessor, 
                                IImageRepository _imageRepository,
                                IDistributedCache _cache) : IImageService
    {
        private readonly string _imagePath = options.Value.ImagePath;
        private readonly string _baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Images/";

        public async Task<string> SaveImageAsync(IFormFile file, int eventId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File does not exist or empty");

            var allowedImageTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedImageTypes.Contains(file.ContentType))
            {
                throw new InvalidOperationException("The file must be an image (jpeg, png).");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("The file must have one of the following extensions: .jpg, .jpeg, .png.");
            }

            const long maxFileSizeInBytes = 5 * 1024 * 1024;
            if (file.Length > maxFileSizeInBytes)
            {
                throw new InvalidOperationException("The file size must not exceed 5MB.");
            }

            var @event = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("Event");

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

            var cachedImagesJson = await _cache.GetStringAsync($"image-{eventId}");
            var cachedImages = new List<ImageInfoDto>();

            if (!string.IsNullOrEmpty(cachedImagesJson))
            {
                cachedImages = JsonSerializer.Deserialize<List<ImageInfoDto>>(cachedImagesJson);
            }
            else
            {
                var images = await _imageRepository.GetAllImageUrlsFromEvent(eventId);
                cachedImages = images.ToList().Adapt<List<ImageInfoDto>>();
            }

            var updatedImages = cachedImages.Append(new ImageInfoDto() { ImageUrl = imageUrl });
            var serializedImages = JsonSerializer.Serialize(updatedImages);

            await _cache.SetStringAsync($"image-{eventId}", serializedImages);

            return filePath;
        }

        public async Task<IEnumerable<ImageInfoDto>> GetAllEventImages(int eventId)
        {
            var @event = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("Event");

            var cachedImagesJson = await _cache.GetStringAsync($"image-{eventId}");
            if(!string.IsNullOrEmpty(cachedImagesJson))
            {
                var cachedImages = JsonSerializer.Deserialize<List<ImageInfoDto>>(cachedImagesJson);
                return cachedImages;
            }

            var images = await _imageRepository.GetAllImageUrlsFromEvent(eventId);
            //images.Select(i => i.Adapt<ImageInfoDto>());
            var imageDtos = images.Adapt<IEnumerable<ImageInfoDto>>();

            var serializedImages = JsonSerializer.Serialize(imageDtos);
            await _cache.SetStringAsync($"image-{eventId}", serializedImages);

            return imageDtos;
        }

        public async Task RemoveAllEventImages(int eventId)
        {
            var @event = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("Event");
            string eventFolderPath = Path.Combine(_imagePath, eventId.ToString());
            if(Directory.Exists(eventFolderPath))
            {
                Directory.Delete(eventFolderPath, true);
            }

            await _cache.RemoveAsync($"image-{eventId}");

            //await _imageRepository.RemoveAllImagesFromEvent(eventId);

        }

    }
}
