using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TABP.Domain.Models;

namespace TABP.Presentation.Extensions;
public static class PaginationExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, PaginationMetaData paginationMetaData)
    {
        response.Headers["x-pagination"] = JsonSerializer.Serialize(paginationMetaData);
    }
}
