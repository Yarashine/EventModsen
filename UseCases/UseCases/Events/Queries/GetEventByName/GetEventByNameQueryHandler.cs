using Application.DTOs.Response;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventByName;

public class GetEventByNameQueryHandler(IEventRepository _eventRepository) : IRequestHandler<GetEventByNameQuery, EventDto>
{

    public async Task<EventDto> Handle(GetEventByNameQuery request, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetByNameAsync(request.Name, cancellationToken)
                    ?? throw new NotFoundException("Event");
        return @event.Adapt<EventDto>();
    }
}

