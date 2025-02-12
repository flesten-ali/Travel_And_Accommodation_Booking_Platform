﻿using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Domain.Interfaces.Persistence.Repositories;
public interface ICityRepository : IRepository<City>
{
    Task<PaginatedList<CityForAdminResult>> GetCitiesForAdmin(int pageSize, int pageNumber);
}
