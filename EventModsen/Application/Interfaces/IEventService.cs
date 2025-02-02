namespace EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using System.Diagnostics.Eventing.Reader;
using EventModsen.Application.DTOs;
using EventModsen.Application.DTOs.RequestDto;

public interface IEventService
{
    public Task<IEnumerable<EventDto>> GetEvents();
    public Task<IEnumerable<EventDto>> GetFilteredEvents(int pageNumber, DateTime? date = null, string? location = null, string? category = null);
    public Task<EventDto> GetEventById(int id);
    public Task<EventDto> GetEventByName(string name);
    public Task AddEvent(CreateEventDto @event);
    public Task UpdateEvent(UpdateEventDto @event);
    public Task DeleteEvent(int id);
}
