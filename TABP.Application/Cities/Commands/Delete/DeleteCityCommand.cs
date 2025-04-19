using MediatR;

namespace TABP.Application.Cities.Commands.Delete;
public sealed record DeleteCityCommand(Guid Id) : IRequest;
