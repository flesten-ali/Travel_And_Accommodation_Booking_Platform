using TABP.Domain.Entities;
using TABP.Domain.Enums;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IImageRepository : IRepository<Image>
{
    Task DeleteByIdAsync(Guid entityId, ImageType imageType);
}