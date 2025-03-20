using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class CartItemRepository(AppDbContext context) : Repository<CartItem>(context), ICartItemRepository
{
    /// <summary>
    /// Retrieves a paginated list of cart items with ordering.
    /// </summary>
    /// <param name="orderBy">Function to order the cart items query.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="pageNumber">Page number for pagination.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    public async Task<PaginatedResponse<CartItem>> GetCartItemsAsync(
        Func<IQueryable<CartItem>, IOrderedQueryable<CartItem>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var allCartItems = DbSet
             .Include(c => c.RoomClass)
             .AsNoTracking();

        var cartItems = orderBy(allCartItems);

        var requestedPage = cartItems.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaData = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedResponse<CartItem>(await requestedPage.ToListAsync(cancellationToken), paginationMetaData);
    }
}
