using MediatR;
using TABP.Application.Amenities.Common;

namespace TABP.Application.Amenities.Queries.GetById;
public sealed record GetAmenityByIdQuery(Guid AmenityId) : IRequest<AmenityResponse>;
