using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.Cities.GetForAdmin;
public class GetCitiesForAdminQuery : IRequest<PaginatedList<CityForAdminResponse>>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}