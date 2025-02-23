using MediatR;
using TABP.Application.Cities.Common;

namespace TABP.Application.Cities.Queries.GetById;
public sealed record GetCityByIdQuery(Guid Id) : IRequest<CityResponse>;
