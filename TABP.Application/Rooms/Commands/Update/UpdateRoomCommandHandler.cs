using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Rooms.Commands.Update;
public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomCommandHandler(
        IRoomRepository roomRepository,
        IRoomClassRepository roomClassRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateRoomCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        var room = await _roomRepository.GetByIdAsync(request.Id, cancellationToken)
             ?? throw new NotFoundException(RoomExceptionMessages.NotFound);

        _mapper.Map(request, room);
        _roomRepository.Update(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
