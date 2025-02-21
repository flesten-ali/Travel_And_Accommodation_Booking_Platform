namespace TABP.Domain.Exceptions;
public class ExistsException(string message) : DomainException(message, "Conflict Detected")
{
}