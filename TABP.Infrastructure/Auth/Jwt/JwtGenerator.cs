using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Auth;
using TABP.Domain.Models;
namespace TABP.Infrastructure.Auth.Jwt;

public class JwtGenerator : IJwtGenerator
{
    private readonly JwtConfig _jwtConfig;

    public JwtGenerator(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public JwtToken GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var signingCredantials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new("sub" , user.Id.ToString()),
            new("username"   , user.UserName),
            new(ClaimTypes.Role,user.Role)
        };

        var jwtSecurityToken = new JwtSecurityToken(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationTimeInMinutes),
            signingCredantials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return new JwtToken
        {
            Token = token,
        };
    }
}