using Application.DTOs.Response;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEvents;

public class GetAllEventsQueryHandler(IEventRepository _eventRepository) : IRequestHandler<GetAllEventsQuery, IEnumerable<EventDto>>
{

    public async Task<IEnumerable<EventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository.GetAllAsync(cancellationToken);
        return events.Adapt<IEnumerable<EventDto>>();
    }
}
