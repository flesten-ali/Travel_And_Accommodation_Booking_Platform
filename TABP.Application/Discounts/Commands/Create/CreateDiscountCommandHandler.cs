using AutoMapper;
using MediatR;
using TABP.Application.Discounts.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Discounts.Commands.Create;
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
