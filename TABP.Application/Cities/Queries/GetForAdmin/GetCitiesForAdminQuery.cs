using MediatR;
using TABP.Application.Cities.Common;
using TABP.Application.Shared;
using TABP.Domain.Models;

namespace TABP.Application.Cities.Queries.GetForAdmin;
public sealed record GetCitiesForAdminQuery(PaginationParameters PaginationParameters) 
    : IRequest<PaginatedResponse<CityForAdminResponse>>;
