using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;
public class CartItemRepository(AppDbContext context) : Repository<CartItem>(context), ICartItemRepository
{
    public async Task<PaginatedList<CartItem>> GetCartItemsAsync(
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

        return new PaginatedList<CartItem>(await requestedPage.ToListAsync(cancellationToken), paginationMetaData);
    }
}
