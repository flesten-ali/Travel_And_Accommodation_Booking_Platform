using AutoMapper;
using MediatR;
using TABP.Application.RoomClasses.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.RoomClasses.Queries.GetById;

/// <summary>
/// Handles the query to retrieve a room class by its ID.
/// </summary>
public class GetRoomClassByIdQueryHandler 
    : IRequestHandler<GetRoomClassByIdQuery, RoomClassResponse>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetRoomClassByIdQueryHandler(
        IRoomClassRepository roomClassRepository, 
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve a room class by its ID.
    /// </summary>
    /// <param name="request">The query containing the room class ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a <see cref="RoomClassResponse"/>
    /// containing the room class details.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class is not found by the provided ID.</exception>
    public async Task<RoomClassResponse> Handle(
        GetRoomClassByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var roomClass = await _roomClassRepository.GetByIdAsync(
            request.Id,
            cancellationToken)
            ?? throw new NotFoundException(RoomClassExceptionMessages.NotFound);

        return _mapper.Map<RoomClassResponse>(roomClass);
    }
}