using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;
namespace TABP.Application.Hotels.Queries.GetForAdmin;

public class GetHotelsForAdminQuery : IRequest<PaginatedResponse<HotelForAdminResponse>>
{
    public PaginationParameters PaginationParameters { get; set; }
}
