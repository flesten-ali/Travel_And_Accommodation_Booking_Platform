using AutoMapper;
using MediatR;
using TABP.Application.Owners.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Owners.Commands.Create;
public class CreateOwnerCommandHandler : IRequestHandler<CreateOwnerCommand, OwnerResponse>
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOwnerCommandHandler(IOwnerRepository ownerRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<OwnerResponse> Handle(CreateOwnerCommand request, CancellationToken cancellationToken = default)
    {
        if (await _ownerRepository.ExistsAsync(o => o.Email == request.Email, cancellationToken))
        {
            throw new ConflictException(OwnerExceptionMessages.ExistWithEmail);
        }

        var owner = _mapper.Map<Owner>(request);

        await _ownerRepository.CreateAsync(owner, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OwnerResponse>(owner);
    }
}
