using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class DiscountRepository(AppDbContext context) : Repository<Discount>(context), IDiscountRepository
{
    public async Task<PaginatedList<Discount>> GetDiscountsForRoomClass(
        Func<IQueryable<Discount>, IOrderedQueryable<Discount>> orderBy,
        Guid roomClassId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var roomClassDiscounts = DbSet
            .Where(d => d.RoomClassId == roomClassId)
            .AsNoTracking();

        var resultDiscounts = orderBy(roomClassDiscounts);

        var requestedPage = resultDiscounts.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaData = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedList<Discount>(await requestedPage.ToListAsync(cancellationToken), paginationMetaData);
    }
}