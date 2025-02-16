using TABP.Domain.Enums;

namespace TABP.Application;
public class PaginationParameters
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public SortOrder SortOrder { get; set; }
    public string? OrderColumn { get; set; }
}
