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
using Application.Contracts;


namespace Application.UseCases.Images.Commands.Create;

public class AddImageCommandHandler(IEventRepository _eventRepository,
                                IHttpContextAccessor httpContextAccessor,
                                IImageRepository _imageRepository,
                                IImageService _imageService,
                                IDistributedCache _cache) : IRequestHandler<CreateImageCommand, string>
{
    private readonly string _baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Images/";
    public async Task<string> Handle(CreateImageCommand request, CancellationToken cancellationToken = default)
    {

        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken) ?? throw new NotFoundException("Event");

        var partitionImageUrl = await _imageService.SaveImage(request.Image, request.EventId, cancellationToken);

        var imageUrl = _baseUrl + partitionImageUrl;

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

        return imageUrl;
    }

}

