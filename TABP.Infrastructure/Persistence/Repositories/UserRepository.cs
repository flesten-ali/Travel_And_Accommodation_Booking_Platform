using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Security.Password;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly IPasswordHasher _passwordHasher;

    public UserRepository(AppDbContext context, IPasswordHasher passwordHasher) : base(context)
    {
        _passwordHasher = passwordHasher;
    }

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

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
    }
}
