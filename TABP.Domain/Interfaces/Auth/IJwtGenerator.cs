using TABP.Domain.Entities;
using TABP.Domain.Models;
namespace TABP.Domain.Interfaces.Auth;

public interface IJwtGenerator
{
    JwtToken GenerateToken(User user);
}
