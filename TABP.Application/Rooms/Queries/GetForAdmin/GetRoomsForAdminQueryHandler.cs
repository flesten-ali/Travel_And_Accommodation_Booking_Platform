using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Rooms.Queries.GetForAdmin;

/// <summary>
/// Handles the query to retrieve rooms for the admin, including pagination and sorting.
/// </summar
public class GetRoomsForAdminQueryHandler
    : IRequestHandler<GetRoomsForAdminQuery, PaginatedResponse<RoomForAdminResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomsForAdminQueryHandler(
        IRoomClassRepository roomClassRepository,
        IRoomRepository roomRepository,
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve rooms for the admin with pagination and sorting.
    /// </summary>
    /// <param name="request">The query containing the room class ID and pagination parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning the paginated list of rooms for the admin.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class is not found.</exception>
    public async Task<PaginatedResponse<RoomForAdminResponse>> Handle(
        GetRoomsForAdminQuery request,
        CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomExceptionMessages.NotFound);
        }

        var orderBy = SortBuilder.BuildRoomSort(request.PaginationParameters);

        var roomClasses = await _roomRepository.GetRoomsForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<RoomForAdminResponse>>(roomClasses);
    }
}
