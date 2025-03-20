using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class DiscountRepository(AppDbContext context) : Repository<Discount>(context), IDiscountRepository
{
    /// <summary>
    /// Retrieves a paginated list of discounts for a specific room class, ordered by the specified function.
    /// </summary>
    /// <param name="orderBy">Function to order the discounts query.</param>
    /// <param name="roomClassId">The ID of the room class to filter discounts for.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="pageNumber">Page number for pagination.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>A paginated response containing discount information and pagination metadata.</returns>
    public async Task<PaginatedResponse<Discount>> GetDiscountsForRoomClass(
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

        return new PaginatedResponse<Discount>(await requestedPage.ToListAsync(cancellationToken), paginationMetaData);
    }
}