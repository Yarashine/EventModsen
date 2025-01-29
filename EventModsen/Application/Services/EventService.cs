namespace EventModsen.Application.Services;
using EventModsen.Application.Interfaces;
//using EventModsen.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventModsen.Domain.Interfaces;
using EventModsen.Application.DTOs;
using Mapster;
using EventModsen.Infrastructure.DB.Models;

public class EventService(IEventRepository _eventRepository) : IEventService
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

    public async Task<List<EventDto>?> GetEvents()
    {
        var events =  await _eventRepository.GetAllAsync();
        return events.Adapt<List<EventDto>?>();
    }

    public Task<bool> UpdateEvent(EventDto @event)
    {
        var eventEntity = @event.Adapt<Event>();
        return _eventRepository.UpdateAsync(eventEntity);
    }
}
