using AutoMapper;
using MediatR;
using TABP.Application.Owners.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Owners.Commands.Create;

/// <summary>
/// Handles the command to create a new owner in the system.
/// </summary>
public class CreateOwnerCommandHandler : IRequestHandler<CreateOwnerCommand, OwnerResponse>
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOwnerCommandHandler(
        IOwnerRepository ownerRepository, 
        IMapper mapper, 
        IUnitOfWork unitOfWork)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the request to create a new owner.
    /// </summary>
    /// <param name="request">The command containing the information required to create the owner.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <see cref="OwnerResponse"/>.</returns>
    /// <exception cref="ConflictException">Thrown when an owner already exists with the given email address.</exception>
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
