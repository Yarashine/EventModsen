using System.IdentityModel.Tokens.Jwt;

namespace EventModsen.Application.Interfaces
{
    public interface IJwtService
    {
        int? GetUserIdFromToken(string token);
        public string GenerateRefreshToken(int userId, string role, int age);
        public string GenerateAccessToken(int userId, string role, int age);
    }
}
