using TABP.Domain.Entities;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User?> AuthenticateUserAsync(string email, string password, CancellationToken cancellationToken = default);
}