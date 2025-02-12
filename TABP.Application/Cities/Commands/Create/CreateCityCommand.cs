using MediatR;
using TABP.Application.Cities.Common;
namespace TABP.Application.Cities.Commands.Create;

public class CreateCityCommand : IRequest<CityResponse>
{
    public string Name { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
}