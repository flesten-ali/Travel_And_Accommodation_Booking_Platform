using Microsoft.AspNetCore.Identity;
using TABP.Domain.Interfaces.Security.Password;
namespace TABP.Infrastructure.Security.Password;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher;
    public PasswordHasher()
    {
        _passwordHasher = new();
    }

    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
        return result switch
        {
            PasswordVerificationResult.Success => true,
            PasswordVerificationResult.Failed => false,
            _ => false,
        };
    }
}