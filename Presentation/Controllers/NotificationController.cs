using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController(INotificationUseCase _notificationService) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUnreadNotifications(CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var notifications = await _notificationService.GetAllMembersUnreadNotifications(int.Parse(userId), cancelToken);
        return Ok(notifications);
    }

}
