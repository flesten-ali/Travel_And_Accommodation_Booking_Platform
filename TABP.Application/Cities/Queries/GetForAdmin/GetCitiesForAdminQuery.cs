using MediatR;
using TABP.Application.Cities.Common;
using TABP.Domain.Models;

namespace TABP.Application.Cities.Queries.GetForAdmin;
public class GetCitiesForAdminQuery : IRequest<PaginatedList<CityForAdminResponse>>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}