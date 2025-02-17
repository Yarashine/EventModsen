using Application.DTOs.Response;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler(IEventRepository _eventRepository) : IRequestHandler<GetEventByIdQuery, EventDto>
{
    public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new NotFoundException("Event");
        return @event.Adapt<EventDto>();
    }
}
