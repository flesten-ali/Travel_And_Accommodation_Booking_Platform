using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extenstions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;
public class CityRepository(AppDbContext context) : Repository<City>(context), ICityRepository
{
    public async Task<PaginatedList<CityForAdminResult>> GetCitiesForAdmin(int pageSize, int pageNumber)
    {
        var cities = DbSet.Select(c => new CityForAdminResult
        {
            Id = c.Id,
            Name = c.Name,
            Country = c.Country,
            PostOffice = c.PostOffice,
            CreatedDate = c.CreatedDate,
            UpdatedDate = c.UpdatedDate,
            NumberOfHotels = c.Hotels.Count(),
        });

        var requestedPage = PaginationExtenstions.GetRequestedPage(cities, pageNumber, pageSize);
        var paginationMetaData = await requestedPage.GetPaginationMetaDataAsync(pageNumber, pageSize);
        return new PaginatedList<CityForAdminResult>(await requestedPage.ToListAsync(), paginationMetaData);
    }
}