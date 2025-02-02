namespace EventModsen.Configuration;

public class JwtSettings
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; } 
    public double AccessTokenLifetime { get; set; }
    public double RefreshTokenLifetime { get; set; }
}
