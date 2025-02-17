using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;
namespace TABP.Application.Hotels.Queries.GetForAdmin;

public class GetHotelsForAdminQuery : IRequest<PaginatedList<HotelForAdminResponse>>
{
    public PaginationParameters PaginationParameters { get; set; }
}
