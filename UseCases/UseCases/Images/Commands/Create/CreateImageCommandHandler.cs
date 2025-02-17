using MediatR;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Application.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Mapster;
using Application.DTOs.Response;


namespace Application.UseCases.Images.Commands.Create;

public class AddImageCommandHandler(IEventRepository _eventRepository,
                                IOptions<ImageSettings> options,
                                IHttpContextAccessor httpContextAccessor,
                                IImageRepository _imageRepository,
                                IDistributedCache _cache) : IRequestHandler<CreateImageCommand, string>
{
    private readonly string _imagePath = options.Value.ImagePath;
    private readonly string _baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Images/";
    public async Task<string> Handle(CreateImageCommand request, CancellationToken cancellationToken = default)
    {
        if (request.Image == null || request.Image.Length == 0)
            throw new ArgumentException("File does not exist or empty");

        var allowedImageTypes = new[] { "image/jpeg", "image/png" };
        if (!allowedImageTypes.Contains(request.Image.ContentType))
        {
            throw new InvalidOperationException("The file must be an image (jpeg, png).");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(request.Image.FileName)?.ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new InvalidOperationException("The file must have one of the following extensions: .jpg, .jpeg, .png.");
        }

        const long maxFileSizeInBytes = 5 * 1024 * 1024;
        if (request.Image.Length > maxFileSizeInBytes)
        {
            throw new InvalidOperationException("The file size must not exceed 5MB.");
        }

        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken) ?? throw new NotFoundException("Event");

        string eventFolderPath = Path.Combine(_imagePath, request.EventId.ToString());
        await Console.Out.WriteLineAsync(eventFolderPath);
        Directory.CreateDirectory(eventFolderPath);

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
        string filePath = Path.Combine(eventFolderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.Image.CopyToAsync(stream);
        }

        var imageUrl = $"{_baseUrl}{request.EventId}/{fileName}";

        await _imageRepository.AddImageUrlToEvent(request.EventId, imageUrl, cancellationToken);

        var cachedImagesJson = await _cache.GetStringAsync($"image-{request.EventId}");
        var cachedImages = new List<ImageInfoDto>();

        if (!string.IsNullOrEmpty(cachedImagesJson))
        {
            cachedImages = JsonSerializer.Deserialize<List<ImageInfoDto>>(cachedImagesJson);
        }
        else
        {
            var images = await _imageRepository.GetAllImageUrlsFromEvent(request.EventId, cancellationToken);
            cachedImages = images.ToList().Adapt<List<ImageInfoDto>>();
        }

        var updatedImages = cachedImages.Append(new ImageInfoDto() { ImageUrl = imageUrl });
        var serializedImages = JsonSerializer.Serialize(updatedImages);

        await _cache.SetStringAsync($"image-{request.EventId}", serializedImages);

        return filePath;
    }

}

