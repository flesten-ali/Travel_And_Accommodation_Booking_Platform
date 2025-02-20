namespace TABP.Domain.Models;

public record PaginatedResponse<T>(IEnumerable<T> Items, PaginationMetaData PaginationMetaData);
