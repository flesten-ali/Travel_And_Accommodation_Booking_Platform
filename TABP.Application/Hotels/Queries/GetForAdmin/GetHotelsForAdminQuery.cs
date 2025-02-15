using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.Hotels.Queries.GetForAdmin;
public class GetHotelsForAdminQuery : IRequest<PaginatedList<HotelForAdminResponse>>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
