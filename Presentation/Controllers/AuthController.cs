using Application.DTOs.RequestDto;
using Application.DTOs.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Entities;

namespace Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthUseCase _authService) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken cancelToken = default)
    {
        var response = await _authService.Register(registerDto, cancelToken);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancelToken = default)
    {
        var response = await _authService.Login(loginDto, cancelToken);
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> GetNewAccessToken([FromQuery] string oldRefreshToken, CancellationToken cancelToken = default)
    {
        var response = await _authService.GetNewAccessToken(oldRefreshToken, cancelToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut(CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _authService.LogOut(int.Parse(userId), cancelToken);
        return Ok("Logged out successfully.");
    }

    //[Authorize(Roles = "Admin")]
    [Authorize]
    [HttpPut("current/role/admin")]
    public async Task<IActionResult> ChangeMembersRoleToAdmin(CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _authService.ChangeMemberRole(int.Parse(userId), Role.Admin, cancelToken);
        return Ok();
    }
}

