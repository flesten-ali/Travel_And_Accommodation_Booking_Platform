using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Security.Password;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context, IPasswordHasher passwordHasher) : Repository<User>(context), IUserRepository
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    /// <summary>
    /// Authenticates a user based on the provided email and password.
    /// </summary>
    /// <param name="email">The email of the user attempting to log in.</param>
    /// <param name="password">The password provided by the user.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the authenticated <see cref="User"/> if credentials are valid;
    /// otherwise, returns null.
    /// </returns>
    public async Task<User?> AuthenticateUserAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByEmailAsync(email, cancellationToken);

        if (user == null)
        {
            return null;
        }

        if (_passwordHasher.VerifyPassword(user.PasswordHash, password))
        {
            return user;
        }
        return null;
    }

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the <see cref="User"/> if found; otherwise, returns null.
    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
