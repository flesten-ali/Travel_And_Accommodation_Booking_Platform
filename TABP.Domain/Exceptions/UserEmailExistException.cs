using Raven.Client.Exceptions;

namespace TABP.Domain.Exceptions;
public class UserEmailExistException : ConflictException
{
    public UserEmailExistException(string email)
        : base($"User with email '{email}' already exists")
    {
    }
}
