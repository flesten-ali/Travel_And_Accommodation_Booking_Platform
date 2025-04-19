using MediatR;

namespace TABP.Application.Cities.Commands.Update;
public class UpdateCityCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
}