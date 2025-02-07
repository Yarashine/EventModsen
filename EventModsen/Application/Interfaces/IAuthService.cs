using EventModsen.Application.DTOs.RequestDto;
using EventModsen.Application.DTOs.Response;
using EventModsen.Domain.Entities;

namespace EventModsen.Application.Interfaces;

public interface IAuthService
{
    public Task<AuthResponseDto> Register(RegisterDto member, CancellationToken cancelToken = default);
    public Task<AuthResponseDto> Login(LoginDto credentials, CancellationToken cancelToken = default);
    public Task<AuthResponseDto> GetNewAccessToken(string oldRefreshToken, CancellationToken cancelToken = default);
    public Task LogOut(int memberId, CancellationToken cancelToken = default);
    public Task ChangeMemberRole(int memberId, Role role, CancellationToken cancelToken = default);
}
