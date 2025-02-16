using Raven.Client.Exceptions;

namespace TABP.Application.Exceptions;
public class EntityInUseException(string msg) : ConflictException(msg)
{
}
