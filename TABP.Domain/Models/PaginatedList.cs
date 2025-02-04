namespace TABP.Domain.Models;

public record PaginatedList<T>(IEnumerable<T> Items, PaginationMetaData PaginationMetaData);
