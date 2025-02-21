namespace TABP.Domain.Exceptions;
public class NotFoundException(string message ) : DomainException(message, "Resource Not Found")
{
}
