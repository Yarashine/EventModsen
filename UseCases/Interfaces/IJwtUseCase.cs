using System.IdentityModel.Tokens.Jwt;

namespace Application.Interfaces;

public interface IJwtUseCase
{
    int? GetUserIdFromToken(string token, CancellationToken cancelToken = default);
    public string GenerateRefreshToken(int userId, string role, int age, CancellationToken cancelToken = default);
    public string GenerateAccessToken(int userId, string role, int age, CancellationToken cancelToken = default);
}
