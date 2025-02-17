using Application.DTOs.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Entities;
using MediatR;
using Application.UseCases.Auth.Commands.RegisterMember;
using Application.UseCases.Auth.Commands.Login;
using Application.UseCases.Auth.Commands.GetNewAccessToken;
using Application.UseCases.Auth.Commands.Logout;
using Domain.Exceptions;
using Application.UseCases.Auth.Commands.ChangeMembersRole;

namespace Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator _mediator) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken cancelToken = default)
    {
        var response = await _mediator.Send(new RegisterMemberCommand(registerDto), cancelToken);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancelToken = default)
    {
        var response = await _mediator.Send(new LoginMemberCommand(loginDto), cancelToken);
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> GetNewAccessToken([FromQuery] string oldRefreshToken, CancellationToken cancelToken = default)
    {
        var response = await _mediator.Send(new GetNewAccessTokenCommand(oldRefreshToken), cancelToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut(CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new BadRequestException("User is unauthorused");
        await _mediator.Send(new LogoutMemberCommand(int.Parse(userId)), cancelToken);
        return Ok("Logged out successfully.");
    }

    //[Authorize(Roles = "Admin")]
    [Authorize]
    [HttpPut("current/role/admin")]
    public async Task<IActionResult> ChangeMembersRoleToAdmin(CancellationToken cancelToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new BadRequestException("User is unauthorused");
        await _mediator.Send(new ChangeMembersRoleCommand(int.Parse(userId), Role.Admin), cancelToken);
        return Ok();
    }
}

