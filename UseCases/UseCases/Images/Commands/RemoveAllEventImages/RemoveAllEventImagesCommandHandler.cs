using Application.Configuration;
using Application.Contracts;
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
    IImageService _imageService,
    IDistributedCache _cache)
    : IRequestHandler<RemoveAllEventImagesCommand>
{
    public async Task Handle(RemoveAllEventImagesCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken)
                     ?? throw new NotFoundException("Event");

        _imageService.RemoveAllImageFilesFromEvent(request.EventId);        

        await _cache.RemoveAsync($"image-{request.EventId}", cancellationToken);
    }
}
