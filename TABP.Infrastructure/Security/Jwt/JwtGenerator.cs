using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Security.Jwt;
using TABP.Domain.Models;

namespace TABP.Infrastructure.Security.Jwt;

/// <summary>
/// Handles JWT (JSON Web Token) generation for user authentication.
/// </summary>
public class JwtGenerator : IJwtGenerator
{
    private readonly JwtConfig _jwtConfig;

    public JwtGenerator(IOptions<JwtConfig> jwtConfigOptions)
    {
        _jwtConfig = jwtConfigOptions.Value;
    }

    /// <summary>
    /// Generates a JWT access token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being generated.</param>
    /// <returns>A <see cref="JwtToken"/> containing the generated access token.</returns>
    /// <remarks>
    /// The token includes the user's ID (`sub` claim), username, and role. 
    /// The token is signed using HMAC SHA-256 and is valid for the configured expiration time.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var jwtGenerator = new JwtGenerator(options);
    /// var token = jwtGenerator.GenerateToken(user);
    /// Console.WriteLine(token.Token);
    /// </code>
    /// </example>
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

        return new JwtToken(default)
        {
            Token = token,
        };
    }
}