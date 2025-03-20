using MediatR;
using TABP.Application.Cities.Common;
namespace TABP.Application.Cities.Commands.Create;

public sealed record CreateCityCommand(
    string Name,
    string? PostalCode,
    string? Address,
    string Country,
    string PostOffice) : IRequest<CityResponse>;
