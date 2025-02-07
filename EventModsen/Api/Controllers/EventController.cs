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
    public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents(CancellationToken cancelToken = default)
    {
        var events = await _eventService.GetEvents(cancelToken);
        return Ok(events);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetFilteredEvents([FromQuery] int pageNumber, [FromQuery] DateTime? date, [FromQuery] string? location, [FromQuery] string? category, CancellationToken cancelToken = default)
    {
        var events = await _eventService.GetFilteredEvents(pageNumber, cancelToken, date, location, category);
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto?>> GetById([FromRoute] int id, CancellationToken cancelToken = default)
    {
        return await _eventService.GetEventById(id, cancelToken);
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<EventDto?>> GetByName([FromRoute] string name, CancellationToken cancelToken = default)
    {
        return await _eventService.GetEventByName(name, cancelToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddEvent([FromBody] CreateEventDto @event, CancellationToken cancelToken = default)
    {
        await _eventService.AddEvent(@event, cancelToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] int id, CancellationToken cancelToken = default)
    {
        await _eventService.DeleteEvent(id, cancelToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> PutEvent([FromBody] UpdateEventDto @event, CancellationToken cancelToken = default)
    {
        await _eventService.UpdateEvent(@event, cancelToken);
        return Ok();
    }
}
