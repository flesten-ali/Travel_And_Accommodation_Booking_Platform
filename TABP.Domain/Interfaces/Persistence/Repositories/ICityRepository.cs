using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface ICityRepository : IRepository<City>
{
    Task<PaginatedResponse<CityForAdminResult>> GetCitiesForAdminAsync(
        Func<IQueryable<City>, IOrderedQueryable<City>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default);
}