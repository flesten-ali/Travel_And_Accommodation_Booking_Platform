using MediatR;
using TABP.Application.Owners.Common;

namespace TABP.Application.Owners.Queries.GetById;
public sealed record GetOwnerByIdQuery(Guid OwnerId) : IRequest<OwnerResponse>;
