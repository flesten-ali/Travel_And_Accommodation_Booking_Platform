using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;
public class CityRepository(AppDbContext context) : Repository<City>(context), ICityRepository
{
    public async Task<PaginatedResponse<CityForAdminResult>> GetCitiesForAdminAsync(
        Func<IQueryable<City>, IOrderedQueryable<City>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var allCities = DbSet.AsNoTracking();

        var cities = orderBy(allCities).Select(c => new CityForAdminResult
        {
            Id = c.Id,
            Name = c.Name,
            Country = c.Country,
            PostOffice = c.PostOffice,
            CreatedDate = c.CreatedDate,
            UpdatedDate = c.UpdatedDate,
            NumberOfHotels = c.Hotels.Count(),
        });

        var requestedPage = PaginationExtenstions.GetRequestedPage(cities, pageSize, pageNumber);
        var paginationMetaData = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedResponse<CityForAdminResult>(await requestedPage.ToListAsync(cancellationToken), paginationMetaData);
    }
}