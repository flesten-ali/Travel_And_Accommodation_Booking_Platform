using MediatR;
using TABP.Application.Cities.Common;

namespace TABP.Application.Cities.Queries.GetById;
public class GetCityByIdQuery : IRequest<CityResponse>
{
    public Guid Id { get; set; }
}