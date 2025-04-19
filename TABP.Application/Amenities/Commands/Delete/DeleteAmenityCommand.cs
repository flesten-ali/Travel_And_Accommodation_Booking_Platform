using MediatR;

namespace TABP.Application.Amenities.Commands.Delete;
public sealed record DeleteAmenityCommand(Guid Id) : IRequest;
