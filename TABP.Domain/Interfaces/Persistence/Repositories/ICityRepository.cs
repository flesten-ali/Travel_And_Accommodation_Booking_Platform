using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface ICityRepository : IRepository<City>
{
    Task<PaginatedList<CityForAdminResult>> GetCitiesForAdminAsync(
        int pageSize,
        int pageNumber,
        Func<IQueryable<City>, IOrderedQueryable<City>> orderBy);
}