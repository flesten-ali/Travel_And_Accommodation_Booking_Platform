using AutoMapper;
using MediatR;
using TABP.Application.Owners.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Owners.Queries.GetById;

/// <summary>
/// Handles the query to retrieve an owner by their ID.
/// </summary>
public class GetOwnerByIdQueryHandler : IRequestHandler<GetOwnerByIdQuery, OwnerResponse>
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public GetOwnerByIdQueryHandler(IOwnerRepository ownerRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve an owner by their ID.
    /// </summary>
    /// <param name="request">The query containing the ID of the owner to be retrieved.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the owner details mapped to the response DTO.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown if the owner with the specified ID is not found.</exception>
    public async Task<OwnerResponse> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken = default)
    {
        var owner = await _ownerRepository.GetByIdAsync(request.OwnerId, cancellationToken)
            ?? throw new NotFoundException(OwnerExceptionMessages.NotFound);

        return _mapper.Map<OwnerResponse>(owner);
    }
}
