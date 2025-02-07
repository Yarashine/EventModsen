namespace EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using System.Diagnostics.Eventing.Reader;
using EventModsen.Application.DTOs;
using EventModsen.Application.DTOs.RequestDto;

public interface IEventService
{
    public Task<IEnumerable<EventDto>> GetEvents(CancellationToken cancelToken = default);
    public Task<IEnumerable<EventDto>> GetFilteredEvents(int pageNumber, CancellationToken cancelToken = default, DateTime? date = null, string? location = null, string? category = null);
    public Task<EventDto> GetEventById(int id, CancellationToken cancelToken = default);
    public Task<EventDto> GetEventByName(string name, CancellationToken cancelToken = default);
    public Task AddEvent(CreateEventDto @event, CancellationToken cancelToken = default);
    public Task UpdateEvent(UpdateEventDto @event, CancellationToken cancelToken = default);
    public Task DeleteEvent(int id, CancellationToken cancelToken = default);
}
