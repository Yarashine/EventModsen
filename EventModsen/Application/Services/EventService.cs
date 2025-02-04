namespace EventModsen.Application.Services;
using EventModsen.Application.Interfaces;
//using EventModsen.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventModsen.Domain.Interfaces;
using EventModsen.Application.DTOs;
using EventModsen.Application.DTOs.RequestDto;
using Mapster;
using EventModsen.Domain.Entities;
using EventModsen.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Routing;
using EventModsen.Domain.Exceptions;
using System.Xml.Linq;

public class EventService(IEventRepository _eventRepository, 
                            IImageService _imageService, 
                            INotificationService _notificationService,
                            IMemberService _memberService,
                            IOptions<PaginationSettings> paginationOptions) : IEventService
{
    private readonly int _pageSize = paginationOptions.Value.PageSize;

    public async Task AddEvent(CreateEventDto @event)
    {
        var eventWithName = await _eventRepository.GetByNameAsync(@event.Name);
        if (eventWithName is not null)
            throw new BadRequestException("Event with this name already exist");
        var eventEntity = @event.Adapt<Event>();
        await _eventRepository.CreateAsync(eventEntity);
    }
    public async Task DeleteEvent(int id)
    {
        var @event = await _eventRepository.GetByIdAsync(id) ?? throw new NotFoundException("Event");
        await _imageService.RemoveAllEventImages(id);
        await _eventRepository.DeleteAsync(id);
    }
    public async Task<EventDto> GetEventById(int id)
    {
        var @event = await _eventRepository.GetByIdAsync(id) ?? throw new NotFoundException("Event");
        return @event.Adapt<EventDto>();
    }

    public async Task<EventDto> GetEventByName(string name)
    {
        var @eventEntity = await _eventRepository.GetByNameAsync(name) ?? throw new NotFoundException("Event");
        return @eventEntity.Adapt<EventDto>();
    }

    public async Task<IEnumerable<EventDto>> GetEvents()
    {
        var events =  await _eventRepository.GetAllAsync();
        return events.Adapt<IEnumerable<EventDto>>();
    }

    public async Task<IEnumerable<EventDto>> GetFilteredEvents(int pageNumber, DateTime? date = null, string? location = null, string? category = null)
    {
        var events = await _eventRepository.GetFilteredAsync(pageNumber, _pageSize, date, location, category);
        return events.Adapt<IEnumerable<EventDto>>();       
    }

    public async Task UpdateEvent(UpdateEventDto @event)
    {
        var eventForCheckingName = await _eventRepository.GetByIdAsync(@event.Id) ?? throw new NotFoundException("Event");
        if (eventForCheckingName.Name != @event.Name)
        {
            var eventWithName = await _eventRepository.GetByNameAsync(@event.Name);
            if (eventWithName is not null)
                throw new BadRequestException("Event with this name already exist");
        }

        if (eventForCheckingName.Location != @event.Location)
        {
            var eventMembers = await _memberService.GetAllMembersByEvent(@event.Id);
            foreach(var member in eventMembers)
            {
                await _notificationService.AddNotification($"The location has changed to {@event.Location}", member.Id);
            }
        }
        else if (eventForCheckingName.DateTimeEvent != @event.DateTimeEvent)
        {
            var eventMembers = await _memberService.GetAllMembersByEvent(@event.Id);
            foreach (var member in eventMembers)
            {
                await _notificationService.AddNotification($"The date and time has changed to {@event.DateTimeEvent}", member.Id);
            }
        }

        var eventEntity = @event.Adapt<Event>();
        await _eventRepository.UpdateAsync(eventEntity);
    }
}
