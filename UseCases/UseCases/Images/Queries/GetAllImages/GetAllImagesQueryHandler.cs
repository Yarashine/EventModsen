using MediatR;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Mapster;
using Application.DTOs.Response;

namespace Application.UseCases.Images.Queries.GetAllImages;


public class GetAllImagesQueryHandler(IEventRepository _eventRepository, IImageRepository _imageRepository, IDistributedCache _cache) : IRequestHandler<GetAllImagesQuery, IEnumerable<ImageInfoDto>>
{
    public async Task<IEnumerable<ImageInfoDto>> Handle(GetAllImagesQuery request, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken) ?? throw new NotFoundException("Event");

        var cachedImagesJson = await _cache.GetStringAsync($"image-{request.EventId}");
        if (!string.IsNullOrEmpty(cachedImagesJson))
        {
            var cachedImages = JsonSerializer.Deserialize<List<ImageInfoDto>>(cachedImagesJson);
            return cachedImages;
        }

        var images = await _imageRepository.GetAllImageUrlsFromEvent(request.EventId, cancellationToken);

        var imageDtos = images.Adapt<IEnumerable<ImageInfoDto>>();

        var serializedImages = JsonSerializer.Serialize(imageDtos);
        await _cache.SetStringAsync($"image-{request.EventId}", serializedImages);

        return imageDtos;
    }
}

