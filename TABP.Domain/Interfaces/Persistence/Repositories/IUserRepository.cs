using TABP.Domain.Entities;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> AuthenticateUser(string email, string password);
}