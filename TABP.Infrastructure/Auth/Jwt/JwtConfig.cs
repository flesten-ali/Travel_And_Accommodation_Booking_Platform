namespace TABP.Infrastructure.Auth.Jwt;

public class JwtConfig
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public double ExpirationTimeInMinutes { get; set; }
}
