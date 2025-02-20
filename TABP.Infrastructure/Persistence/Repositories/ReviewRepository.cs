using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class ReviewRepository(AppDbContext context) : Repository<Review>(context), IReviewRepository
{
    public async Task<PaginatedResponse<Review>> GetByHotelIdAsync(
        Func<IQueryable<Review>, IOrderedQueryable<Review>> orderBy,
        Guid hotelId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var hotelReviews = DbSet.Include(r => r.User).Where(r => r.HotelId == hotelId);

        var reviews = orderBy(hotelReviews);

        var requstedPage = reviews.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaData = await requstedPage.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedResponse<Review>(await requstedPage.ToListAsync(cancellationToken), paginationMetaData);
    }
}