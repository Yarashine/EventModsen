using Application.DTOs.RequestDto;
using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthUseCase
{
    public Task<AuthResponseDto> Register(RegisterDto member, CancellationToken cancelToken = default);
    public Task<AuthResponseDto> Login(LoginDto credentials, CancellationToken cancelToken = default);
    public Task<AuthResponseDto> GetNewAccessToken(string oldRefreshToken, CancellationToken cancelToken = default);
    public Task LogOut(int memberId, CancellationToken cancelToken = default);
    public Task ChangeMemberRole(int memberId, Role role, CancellationToken cancelToken = default);
}
