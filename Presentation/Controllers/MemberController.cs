using Application.UseCases.Events.Queries.GetEventById;
using Application.UseCases.Members.Commands.AddMemberToEvent;
using Application.UseCases.Members.Commands.RemoveMemberFromEvent;
using Application.UseCases.Members.Queries.GetMemberByEvent;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/members")]
public class MemberController(IMediator _mediator) : Controller
{
    [Authorize(Policy = "AgePolicy")]
    [HttpPost("add-to-event/{eventId}")]
    public async Task<IActionResult> AddToEvent(int eventId, CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new BadRequestException("User is unauthorused");
        await _mediator.Send(new AddMemberToEventCommand(int.Parse(userId), eventId), cancelToken);
        return Ok();
    }

    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetAllMembersByEvent(int eventId, CancellationToken cancelToken = default)
    {
        var members = await _mediator.Send(new GetMembersByEventQuery(eventId), cancelToken);
        return Ok(members);
    }

    [HttpGet("id/{memberId}")]
    public async Task<IActionResult> GetMemberById(int memberId, CancellationToken cancelToken = default)
    {
        var member = await _mediator.Send(new GetEventByIdQuery(memberId), cancelToken);
        return Ok(member);
    }

    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "AgePolicy")]
    [HttpDelete("delete-from-event/{eventId}")]
    public async Task<IActionResult> DeleteMemberFromEvent(int eventId, CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new BadRequestException("User is unauthorused");
        await _mediator.Send(new RemoveMemberFromEventCommand(int.Parse(userId), eventId), cancelToken);
        return Ok();
    }
}
