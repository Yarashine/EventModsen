namespace EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using System.Diagnostics.Eventing.Reader;
using EventModsen.Application.DTOs;

public interface IEventService
{
    public Task<IEnumerable<EventDto>?> GetEvents();
    public Task<IEnumerable<EventDto>?> GetFilteredEvents(int pageNumber, DateTime? date = null, string? location = null, string? category = null);
    public Task<EventDto?> GetEventById(int id);
    public Task<EventDto?> GetEventByName(string name);
    public Task AddEvent(EventDto @event);
    public Task<bool> UpdateEvent(EventDto @event);
    public Task<bool> DeleteEvent(int id);
    public Task AddImage();
    public Task<bool> DeleteImage();
}
