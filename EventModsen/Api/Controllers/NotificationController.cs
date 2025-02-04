using EventModsen.Application.Interfaces;
using EventModsen.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EventModsen.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController(INotificationService _notificationService) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUnreadNotifications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var notifications = await _notificationService.GetAllMembersUnreadNotifications(int.Parse(userId));
        return Ok(notifications);
    }

}
