namespace TABP.Domain.Exceptions;
public class EntityInUseException(string message) : DomainException(message, "Entity is Already in Use")
{
}
