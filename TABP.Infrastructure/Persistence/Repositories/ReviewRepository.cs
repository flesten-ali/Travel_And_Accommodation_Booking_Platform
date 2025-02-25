using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class ReviewRepository(AppDbContext context) : Repository<Review>(context), IReviewRepository
{
    /// <summary>
    /// Asynchronously retrieves a paginated list of reviews for a specific hotel, ordered by the provided criteria.
    /// </summary>
    /// <param name="orderBy">A function to apply sorting to the reviews.</param>
    /// <param name="hotelId">The ID of the hotel for which reviews are being retrieved.</param>
    /// <param name="pageSize">The number of reviews to return per page.</param>
    /// <param name="pageNumber">The page number of results to return.</param>
    /// <param name="cancellationToken">A cancellation token to observe while awaiting the asynchronous operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning a paginated response with the reviews and pagination metadata.
    /// </returns>
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