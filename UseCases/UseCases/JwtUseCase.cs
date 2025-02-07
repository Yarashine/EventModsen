namespace Application.Services;
using Application.Interfaces;
using Application.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



public class JwtUseCase(IOptions<JwtSettings> options) : IJwtUseCase
{
    private readonly JwtSettings _jwtSettings = options.Value;

    public int? GetUserIdFromToken(string token, CancellationToken cancelToken = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
            return null;

        var jwt = tokenHandler.ReadJwtToken(token);
        var userId = jwt?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if(userId is not null && int.TryParse(userId?.Value, out int id))
        {
            return id;
        }
        return null;
    }
    private string GenerateToken(int userId, string role, double duration, int age, CancellationToken cancelToken = default)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Role, role),
            new("age", age.ToString())  
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret
            ?? throw new InvalidOperationException("Key not configured")));

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            notBefore: now,
            claims: claims,
            expires: now.AddMinutes(duration),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var tokenHandler = new JwtSecurityTokenHandler();
        string jwtToken = tokenHandler.WriteToken(jwt);

        return jwtToken; 

    }
    public string GenerateAccessToken(int userId, string role, int age, CancellationToken cancelToken = default)
    {
        return GenerateToken(userId, role, Convert.ToDouble(_jwtSettings.AccessTokenLifetime), age, cancelToken);
    }
    public string GenerateRefreshToken(int userId, string role, int age, CancellationToken cancelToken = default)
    {

        return GenerateToken(userId, role, Convert.ToDouble(_jwtSettings.RefreshTokenLifetime), age, cancelToken);
    }
}
