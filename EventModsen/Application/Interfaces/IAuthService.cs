using EventModsen.Application.DTOs.RequestDto;
using EventModsen.Application.DTOs.Response;

namespace EventModsen.Application.Interfaces;

public interface IAuthService
{
    public Task<AuthResponseDto> Register(RegisterDto member);
    public Task<AuthResponseDto> Login(LoginDto credentials);
    public Task<AuthResponseDto> GetNewAccessToken(string oldRefreshToken);
    public Task LogOut(int memberId);
}
