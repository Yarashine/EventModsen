
using Application.UseCases.Notifications.Queries.GetUnreadNotifications;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventModsen.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController(IMediator _mediator) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUnreadNotifications(CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new BadRequestException("User is unauthorused"); ;
        var notifications = await _mediator.Send(new GetUnreadNotificationsQuery(int.Parse(userId)), cancelToken);
        return Ok(notifications);
    }

}
