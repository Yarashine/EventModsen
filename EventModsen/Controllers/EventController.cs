namespace EventModsen.Controllers;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.RequestDto;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Application.UseCases.Events.Commands.Create;
using Application.UseCases.Events.Commands.Update;
using Application.UseCases.Events.Commands.Delete;
using Application.UseCases.Events.Queries.GetEventById;
using Application.UseCases.Events.Queries.GetEventByName;
using Application.UseCases.Events.Queries.GetEvents;
using Application.UseCases.Events.Queries.GetFilteredEvents;
using Mapster;
using Application.DTOs.Response;

[ApiController]
[Route("api/events")]
public class EventController(IMediator _mediator)  : Controller
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents(CancellationToken cancelToken = default)
    {
        var events = await _mediator.Send(new GetAllEventsQuery(), cancelToken);
        return Ok(events);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetFilteredEvents([FromQuery] int pageNumber, [FromQuery] DateTime? date, [FromQuery] string location, [FromQuery] string category, CancellationToken cancelToken = default)
    {
        var events = await _mediator.Send(new GetFilteredEventsQuery(pageNumber, date, location , category), cancelToken);
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetById([FromRoute] int id, CancellationToken cancelToken = default)
    {
        var @event = await _mediator.Send(new GetEventByIdQuery(id), cancelToken);
        return @event;
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<EventDto>> GetByName([FromRoute] string name, CancellationToken cancelToken = default)
    {
        var @event = await _mediator.Send(new GetEventByNameQuery(name), cancelToken);
        return @event;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddEvent([FromBody] CreateEventDto @event, CancellationToken cancelToken = default)
    {
        await _mediator.Send(@event.Adapt<CreateEventCommand>(), cancelToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] int id, CancellationToken cancelToken = default)
    {
        await _mediator.Send(new DeleteEventCommand() { Id = id}, cancelToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> PutEvent([FromBody] UpdateEventDto @event, CancellationToken cancelToken = default)
    {
        await _mediator.Send(@event.Adapt<UpdateEventCommand>(), cancelToken);
        return Ok();
    }
}
