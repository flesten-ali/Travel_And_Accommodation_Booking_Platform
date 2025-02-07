﻿using System.Linq.Expressions;
using TABP.Domain.Entities;
using TABP.Domain.Models;
namespace TABP.Domain.Interfaces.Persistence.Repositories;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<PaginatedList<SearchHotelResult>> SearchHotels(
        Expression<Func<Hotel, bool>> filter,
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderBy,
        int pageSize,
        int pageCount);

    Task<Hotel?> GetHotelByIdAsync(Guid hotelId, params Expression<Func<Hotel, object>>[] includeProperties);
}
