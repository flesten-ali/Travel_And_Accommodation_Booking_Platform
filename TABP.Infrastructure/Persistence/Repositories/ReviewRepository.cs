using Microsoft.EntityFrameworkCore;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extenstions;
using TABP.Infrastructure.Persistence.DbContexts;
namespace TABP.Infrastructure.Persistence.Repositories;

public class ReviewRepository(AppDbContext context) : Repository<Review>(context), IReviewRepository
{
    public async Task<PaginatedList<Review>?> GetByHotelIdAsync(Guid hotelId, int pageSize, int pageNumber)
    {
        var reviews = DbSet.Where(r => r.HotelId == hotelId).Include(r => r.User);

        if (reviews == null)
        {
            return null;
        }

        var requstedPage = reviews.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaData = await requstedPage.GetPaginationMetaDataAsync(pageSize, pageNumber);

        return new PaginatedList<Review>(await requstedPage.ToListAsync(), paginationMetaData);
    }
}