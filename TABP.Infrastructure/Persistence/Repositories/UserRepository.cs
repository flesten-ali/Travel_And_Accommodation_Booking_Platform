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

    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);

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

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await DbSet.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await DbSet.Where(u => u.Email == email).FirstOrDefaultAsync();
    }
}
