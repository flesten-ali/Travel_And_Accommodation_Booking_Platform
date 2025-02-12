using MediatR;

namespace TABP.Application.Cities.Commands.Delete;
public class DeleteCityCommand : IRequest
{
    public Guid Id { get; set; }
}
