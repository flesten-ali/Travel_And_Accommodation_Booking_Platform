using TABP.Domain.Interfaces.Services.Guids;
namespace TABP.Infrastructure.Services.Guids;

public class GuidProvider : IGuidProvider
{
    public Guid NewGuid()
    {
        return Guid.NewGuid();
    }
}
