using MediatR;
using TABP.Application.Owners.Common;

namespace TABP.Application.Owners.Queries.GetById;
public class GetOwnerByIdQuery : IRequest<OwnerResponse>
{
    public Guid OwnerId { get; set; }
}
