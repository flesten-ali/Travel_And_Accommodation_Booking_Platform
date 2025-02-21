namespace TABP.Domain.Exceptions;
public class UserUnauthorizedException(string message) : DomainException(message, "Unauthorized Access")
{
}