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
    public async Task<IActionResult> AddToEvent(int eventId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _memberService.AddToEvent(int.Parse(userId), eventId);
        return Ok();
    }

    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetAllMembersByEvent(int eventId)
    {
        var members = await _memberService.GetAllMembersByEvent(eventId);
        if (members == null || members.Count() == 0)
            return NotFound("No members found for this event.");
        return Ok(members);
    }

    [HttpGet("id/{memberId}")]
    public async Task<IActionResult> GetMemberById(int memberId)
    {
        var member = await _memberService.GetMemberById(memberId);
        if (member == null)
            return NotFound("Member not found.");
        return Ok(member);
    }

    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "AgePolicy")]
    [HttpDelete("delete-from-event/{eventId}")]
    public async Task<IActionResult> DeleteMemberFromEvent(int eventId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _memberService.DeleteMemberFromEvent(int.Parse(userId), eventId);
        return Ok();
    }
}
