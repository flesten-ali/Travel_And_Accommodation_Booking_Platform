using MediatR;
using TABP.Application.Amenities.Common;
namespace TABP.Application.Amenities.Commands.Create;

public sealed record CreateAmenityCommand(string Name, string? Description) : IRequest<AmenityResponse>;
