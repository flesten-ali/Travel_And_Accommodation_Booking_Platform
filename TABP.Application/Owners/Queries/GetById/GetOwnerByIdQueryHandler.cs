using AutoMapper;
using MediatR;
using TABP.Application.Owners.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Owners.Queries.GetById;
public class GetOwnerByIdQueryHandler : IRequestHandler<GetOwnerByIdQuery, OwnerResponse>
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public GetOwnerByIdQueryHandler(IOwnerRepository ownerRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    public async Task<OwnerResponse> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken = default)
    {
        var owner = await _ownerRepository.GetByIdAsync(request.OwnerId, cancellationToken)
            ?? throw new NotFoundException(OwnerExceptionMessages.NotFound);

        return _mapper.Map<OwnerResponse>(owner);
    }
}
