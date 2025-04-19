using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TABP.Domain.Models;

namespace TABP.Presentation.Extensions;

/// <summary>
/// Extension methods for <see cref="HttpResponse"/> to add pagination metadata headers 
/// to the HTTP response for paginated data.
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Adds pagination metadata to the response headers.
    /// This allows clients to receive pagination information (e.g., total pages, current page, item count) 
    /// along with the paginated data in the response body.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponse"/> object to which the pagination metadata is added.</param>
    /// <param name="paginationMetaData">The <see cref="PaginationMetaData"/> object containing the pagination information.</param>
    public static void AddPaginationHeader(this HttpResponse response, PaginationMetaData paginationMetaData)
    {
        response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetaData);
    }
}
