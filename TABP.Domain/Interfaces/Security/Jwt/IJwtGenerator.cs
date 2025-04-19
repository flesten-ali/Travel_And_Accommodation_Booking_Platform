using TABP.Domain.Entities;
using TABP.Domain.Models;
namespace TABP.Domain.Interfaces.Security.Jwt;

public interface IJwtGenerator
{
    JwtToken GenerateToken(User user);
}