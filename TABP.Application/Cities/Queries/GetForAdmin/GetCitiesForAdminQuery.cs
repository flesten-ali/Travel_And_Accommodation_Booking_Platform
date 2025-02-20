using MediatR;
using TABP.Application.Cities.Common;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.Cities.Queries.GetForAdmin;
public class GetCitiesForAdminQuery : IRequest<PaginatedResponse<CityForAdminResponse>>
{
    public PaginationParameters PaginationParameters { get; set; }
}