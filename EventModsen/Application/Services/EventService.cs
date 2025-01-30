namespace EventModsen.Application.Services;
using EventModsen.Application.Interfaces;
//using EventModsen.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventModsen.Domain.Interfaces;
using EventModsen.Application.DTOs;
using Mapster;
using EventModsen.Domain.Entities;
using EventModsen.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Routing;

public class EventService(IEventRepository _eventRepository, IOptions<PaginationSettings> _paginationOptions) : IEventService
{
    public Task AddEvent(EventDto @event)
    {
        var eventEntity = @event.Adapt<Event>();
        return _eventRepository.CreateAsync(eventEntity);
    }

    public Task AddImage()
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteEvent(int id)
    {
        return _eventRepository.DeleteAsync(id);
    }

    public Task<bool> DeleteImage()
    {
        throw new NotImplementedException();
    }

    public async Task<EventDto?> GetEventById(int id)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        return @event.Adapt<EventDto>();
    }

    public async Task<EventDto?> GetEventByName(string name)
    {
        var @eventEntity = await _eventRepository.GetByNameAsync(name);
        return @eventEntity.Adapt<EventDto>();
    }

    public async Task<IEnumerable<EventDto>?> GetEvents()
    {
        var events =  await _eventRepository.GetAllAsync();
        return events.Adapt<IEnumerable<EventDto>?>();
    }

    public async Task<IEnumerable<EventDto>> GetFilteredEvents(int pageNumber, DateTime? date = null, string? location = null, string? category = null)
    {
        var pageSize = _paginationOptions.Value.PageSize;
        var events = await _eventRepository.GetFilteredAsync(pageNumber, pageSize, date, location, category);
        return events.Adapt<IEnumerable<EventDto>>();
       
    }

    public Task<bool> UpdateEvent(EventDto @event)
    {
        var eventEntity = @event.Adapt<Event>();
        return _eventRepository.UpdateAsync(eventEntity);
    }
}
