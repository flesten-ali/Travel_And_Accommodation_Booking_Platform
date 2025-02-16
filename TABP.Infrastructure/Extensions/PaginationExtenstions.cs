using Microsoft.EntityFrameworkCore;
using TABP.Domain.Models;
namespace TABP.Infrastructure.Extensions;

public static class PaginationExtenstions
{
    public static IQueryable<T> GetRequestedPage<T>(this IQueryable<T> queryable, int pageSize, int pageNumber)
    {
        return queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public static async Task<PaginationMetaData> GetPaginationMetaDataAsync<T>(
        this IQueryable<T> queryable,
        int pageSize,
        int pageNumber)
    {
        return new PaginationMetaData(pageSize, pageNumber, await queryable.CountAsync());
    }
}
