namespace TABP.Domain.Models;

public class PaginationMetaData
{
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPagesCount { get; set; }
    public PaginationMetaData(int pageSize, int currentPage, int totalItemsCount)
    {
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalItemsCount = totalItemsCount;
        TotalPagesCount = pageSize == 0 ? 0 : (int)Math.Ceiling(totalItemsCount / (double)pageSize);
    }
}
