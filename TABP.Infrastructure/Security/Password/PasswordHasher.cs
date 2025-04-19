using Microsoft.AspNetCore.Identity;
using TABP.Domain.Interfaces.Security.Password;

namespace TABP.Infrastructure.Security.Password;

/// <summary>
/// Provides password hashing and verification using ASP.NET Core Identity's built-in PasswordHasher.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher;
    public PasswordHasher()
    {
        _passwordHasher = new();
    }

    /// <summary>
    /// Hashes the given password using ASP.NET Core Identity's password hashing algorithm.
    /// </summary>
    /// <param name="password">The plaintext password to hash.</param>
    /// <returns>A hashed password string.</returns>
    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    /// <summary>
    /// Verifies a hashed password against a provided plaintext password.
    /// </summary>
    /// <param name="hashedPassword">The previously hashed password.</param>
    /// <param name="providedPassword">The plaintext password to verify.</param>
    /// <returns>
    /// <c>true</c> if the password matches; otherwise, <c>false</c>.
    /// </returns>
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