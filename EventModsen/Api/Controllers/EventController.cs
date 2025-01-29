namespace EventModsen.Api.Controllers;
using EventModsen.Application.Services;
using Microsoft.AspNetCore.Mvc;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;

[ApiController]
public class EventController(EventService _eventService)  : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<EventDto>?>> GetAllEvents()
    {
        return await _eventService.GetEvents();
    }
}
