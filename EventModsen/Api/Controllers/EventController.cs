namespace EventModsen.Api.Controllers;
using EventModsen.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;

[ApiController]
[Route("api/[controller]")]
public class EventController(IEventService _eventService)  : Controller
{
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<EventDto>?>> GetAllEvents()
    {
        var events = await _eventService.GetEvents();
        return Ok(events);
    }

    [HttpGet]
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

    [HttpPost]
    public async Task<IActionResult> AddEvent([FromBody] EventDto @event)
    {
        await _eventService.AddEvent(@event);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] int id)
    {
        var isDeleted = await _eventService.DeleteEvent(id);
        if(isDeleted)
            return Ok();
        return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> PutEvent([FromBody] EventDto @event)
    {
        await _eventService.UpdateEvent(@event);
        return Ok();
    }
}
