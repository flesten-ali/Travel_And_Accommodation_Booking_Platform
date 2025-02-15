using Raven.Client.Exceptions;

namespace TABP.Domain.Exceptions;
public class EntityInUseException(string msg) : ConflictException(msg)
{
}
