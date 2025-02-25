namespace TABP.Domain.Models;

public sealed record PaginatedResponse<T>(IEnumerable<T> Items, PaginationMetaData PaginationMetaData);
