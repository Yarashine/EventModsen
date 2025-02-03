using EventModsen.Application.DTOs.RequestDto;
using EventModsen.Application.DTOs.Response;
using EventModsen.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EventModsen.Domain.Entities;

namespace EventModsen.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService _authService) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var response = await _authService.Register(registerDto);
        if (response == null)
            return BadRequest("Registration failed.");
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var response = await _authService.Login(loginDto);
        if (response == null)
            return Unauthorized("Invalid credentials.");
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> GetNewAccessToken([FromQuery] string oldRefreshToken)
    {
        var response = await _authService.GetNewAccessToken(oldRefreshToken);
        if (response == null)
            return Unauthorized("Invalid refresh token.");
        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _authService.LogOut(int.Parse(userId));
        return Ok("Logged out successfully.");
    }

    //[Authorize(Roles = "Admin")]
    [Authorize]
    [HttpPut("current/role/admin")]
    public async Task<IActionResult> ChangeMembersRoleToAdmin()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _authService.ChangeMemberRole(int.Parse(userId), Role.Admin);
        return Ok();
    }
}

