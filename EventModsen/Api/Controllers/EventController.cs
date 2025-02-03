namespace EventModsen.Api.Controllers;
using EventModsen.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;
using EventModsen.Application.DTOs.RequestDto;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/events")]
public class EventController(IEventService _eventService)  : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>?>> GetAllEvents()
    {
        var events = await _eventService.GetEvents();
        return Ok(events);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<EventDto>?>> GetFilteredEvents([FromQuery] int pageNumber, [FromQuery] DateTime? date, [FromQuery] string? location, [FromQuery] string? category)
    {
        var events = await _eventService.GetFilteredEvents(pageNumber, date, location, category);
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto?>> GetById([FromRoute] int id)
    {
        return await _eventService.GetEventById(id);
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<EventDto?>> GetByName([FromRoute] string name)
    {
        return await _eventService.GetEventByName(name);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddEvent([FromBody] CreateEventDto @event)
    {
        await _eventService.AddEvent(@event);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] int id)
    {
        await _eventService.DeleteEvent(id);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> PutEvent([FromBody] UpdateEventDto @event)
    {
        await _eventService.UpdateEvent(@event);
        return Ok();
    }
}
