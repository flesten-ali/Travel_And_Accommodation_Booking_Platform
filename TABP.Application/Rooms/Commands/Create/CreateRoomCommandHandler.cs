using AutoMapper;
using MediatR;
using TABP.Application.Rooms.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Rooms.Commands.Create;

/// <summary>
/// Handles the command to create a new room for a specific room class.
/// </summary>
public class CreateRoomCommandHandler 
    : IRequestHandler<CreateRoomCommand, RoomResponse>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(
        IRoomClassRepository roomClassRepository,
        IMapper mapper,
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the request to create a new room for the specified room class.
    /// </summary>
    /// <param name="request">The command containing the room class ID and room details.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning the created room's response.</returns>
    public async Task<RoomResponse> Handle(
        CreateRoomCommand request, 
        CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        var room = _mapper.Map<Room>(request);

        await _roomRepository.CreateAsync(room, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RoomResponse>(room);
    }
}
