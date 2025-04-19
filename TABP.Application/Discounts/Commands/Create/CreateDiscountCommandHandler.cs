using AutoMapper;
using MediatR;
using TABP.Application.Discounts.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Discounts.Commands.Create;

/// <summary>
/// Handles the command for creating a new discount for a specific room class.
/// </summary>
public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, DiscountResponse>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiscountRepository _discountRepository;

    public CreateDiscountCommandHandler(
        IRoomClassRepository roomClassRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IDiscountRepository discountRepository)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _discountRepository = discountRepository;
    }

    /// <summary>
    /// Handles the creation of a new discount for a room class.
    /// </summary>
    /// <param name="request">The request containing the details of the discount to be created.</param>
    /// <param name="cancellationToken">A cancellation token for gracefully canceling the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the created discount's response model.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class does not exist.</exception>
    public async Task<DiscountResponse> Handle(CreateDiscountCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        var discount = _mapper.Map<Discount>(request);

        await _discountRepository.CreateAsync(discount, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DiscountResponse>(discount);
    }
}
