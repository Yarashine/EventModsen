using Application.Configuration;
using Application.RepositoryInterfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Images.Commands.RemoveAllEventImages;

public class RemoveAllEventImagesHandler(
    IEventRepository _eventRepository,
    IOptions<ImageSettings> options,
    IDistributedCache _cache)
    : IRequestHandler<RemoveAllEventImagesCommand>
{
    private readonly string _imagePath = options.Value.ImagePath;

    public async Task Handle(RemoveAllEventImagesCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken)
                     ?? throw new NotFoundException("Event");

        string eventFolderPath = Path.Combine(_imagePath, request.EventId.ToString());

        if (Directory.Exists(eventFolderPath))
        {
            Directory.Delete(eventFolderPath, true);
        }

        await _cache.RemoveAsync($"image-{request.EventId}", cancellationToken);
    }
}
