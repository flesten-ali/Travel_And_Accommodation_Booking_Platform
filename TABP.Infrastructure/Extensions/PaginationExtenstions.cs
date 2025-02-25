using Microsoft.EntityFrameworkCore;
using TABP.Domain.Models;

namespace TABP.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for handling pagination in queries.
/// </summary>
public static class PaginationExtenstions
{
    /// <summary>
    /// Retrieves a specific page of data from a given <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="queryable">The queryable data source.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based index).</param>
    /// <returns>A subset of the original query containing only the requested page.</returns>
    /// <remarks>
    /// This method applies `Skip` and `Take` operations to efficiently fetch the requested page of data.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var pagedData = dbContext.Users.GetRequestedPage(pageSize: 10, pageNumber: 2);
    /// </code>
    /// </example>
    public static IQueryable<T> GetRequestedPage<T>(this IQueryable<T> queryable, int pageSize, int pageNumber)
    {
        return queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    /// <summary>
    /// Generates pagination metadata, including total item count and the requested page details.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="queryable">The queryable data source.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageNumber">The page number being requested.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the async operation if needed.</param>
    /// <returns>
    /// A <see cref="PaginationMetaData"/> object containing pagination details such as total records, page size, and current page number.
    /// </returns>
    /// <remarks>
    /// This method calculates the total record count asynchronously using `CountAsync`, ensuring efficient database querying.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// var paginationInfo = await dbContext.Users.GetPaginationMetaDataAsync(pageSize: 10, pageNumber: 2);
    /// </code>
    /// </example>
    public static async Task<PaginationMetaData> GetPaginationMetaDataAsync<T>(
        this IQueryable<T> queryable,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        return new PaginationMetaData(pageSize, pageNumber, await queryable.CountAsync(cancellationToken));
    }
}
