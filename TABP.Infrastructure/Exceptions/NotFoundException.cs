namespace TABP.Infrastructure.Exceptions;
public class NotFoundException(Guid id) : Exception($"Object of id: {id} not found.")
{
}