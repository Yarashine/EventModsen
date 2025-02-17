using Application.Configuration;
using Application.DTOs.Response;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.UseCases.Events.Queries.GetFilteredEvents;

public class GetFilteredEventsQueryHandler(IEventRepository _eventRepository, IOptions<PaginationSettings> paginationOptions) : IRequestHandler<GetFilteredEventsQuery, IEnumerable<EventDto>>
{
    private readonly int _pageSize = paginationOptions.Value.PageSize;

    public async Task<IEnumerable<EventDto>> Handle(GetFilteredEventsQuery request, CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository.GetFilteredAsync(
            request.PageNumber, _pageSize, cancellationToken, request.Date, request.Location, request.Category);
        return events.Adapt<IEnumerable<EventDto>>();
    }
}

