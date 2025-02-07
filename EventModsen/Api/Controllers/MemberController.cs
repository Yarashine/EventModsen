using EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventModsen.Api.Controllers;

[ApiController]
[Route("api/members")]
public class MemberController(IMemberService _memberService) : Controller
{
    [Authorize(Policy = "AgePolicy")]
    [HttpPost("add-to-event/{eventId}")]
    public async Task<IActionResult> AddToEvent(int eventId, CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _memberService.AddToEvent(int.Parse(userId), eventId, cancelToken);
        return Ok();
    }

    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetAllMembersByEvent(int eventId, CancellationToken cancelToken = default)
    {
        var members = await _memberService.GetAllMembersByEvent(eventId, cancelToken);
        return Ok(members);
    }

    [HttpGet("id/{memberId}")]
    public async Task<IActionResult> GetMemberById(int memberId, CancellationToken cancelToken = default)
    {
        var member = await _memberService.GetMemberById(memberId, cancelToken);
        return Ok(member);
    }

    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "AgePolicy")]
    [HttpDelete("delete-from-event/{eventId}")]
    public async Task<IActionResult> DeleteMemberFromEvent(int eventId, CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _memberService.DeleteMemberFromEvent(int.Parse(userId), eventId, cancelToken);
        return Ok();
    }
}
