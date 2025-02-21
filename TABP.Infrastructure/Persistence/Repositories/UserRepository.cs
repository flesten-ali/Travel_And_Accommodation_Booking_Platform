using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Security.Password;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context, IPasswordHasher passwordHasher) : Repository<User>(context), IUserRepository
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

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

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
