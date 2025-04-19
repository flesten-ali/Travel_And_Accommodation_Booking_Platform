using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Rooms.Commands.Update;

/// <summary>
/// Handles the command to update the details of a room.
/// </summary>
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

    /// <summary>
    /// Handles the request to update the details of a room.
    /// </summary>
    /// <param name="request">The command containing the updated room information.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a unit value when completed.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class or room is not found.</exception>
    public async Task<Unit> Handle(
        UpdateRoomCommand request,
        CancellationToken cancellationToken = default)
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
