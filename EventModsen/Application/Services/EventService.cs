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

public class EventService(IEventRepository _eventRepository, IImageService _imageService, IOptions<PaginationSettings> paginationOptions) : IEventService
{
    private readonly int _pageSize = paginationOptions.Value.PageSize;

    public Task AddEvent(EventDto @event)
    {
        var eventEntity = @event.Adapt<Event>();
        return _eventRepository.CreateAsync(eventEntity);
    }
    public async Task<bool> DeleteEvent(int id)
    {
        bool isDeleted = await _eventRepository.DeleteAsync(id);
        await _imageService.RemoveAllEventImages(id);
        return isDeleted;
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
        var events = await _eventRepository.GetFilteredAsync(pageNumber, _pageSize, date, location, category);
        return events.Adapt<IEnumerable<EventDto>>();
       
    }

    public Task<bool> UpdateEvent(EventDto @event)
    {
        var eventEntity = @event.Adapt<Event>();
        return _eventRepository.UpdateAsync(eventEntity);
    }
}
