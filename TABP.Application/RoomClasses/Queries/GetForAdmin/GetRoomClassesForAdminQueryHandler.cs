using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetForAdmin;

/// <summary>
/// Handles the query to retrieve a paginated list of room classes for an admin view.
/// </summary>
public class GetRoomClassesForAdminQueryHandler
    : IRequestHandler<GetRoomClassesForAdminQuery, PaginatedResponse<RoomClassForAdminResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetRoomClassesForAdminQueryHandler(
        IRoomClassRepository roomClassRepository,
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve a paginated list of room classes for the admin.
    /// </summary>
    /// <param name="request">The query containing the pagination parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a
    /// <see cref="PaginatedResponse{RoomClassForAdminResponse}"/> 
    /// containing the paginated room class data for the admin.</returns>
    public async Task<PaginatedResponse<RoomClassForAdminResponse>> Handle(
        GetRoomClassesForAdminQuery request,
        CancellationToken cancellationToken = default)
    {
        var orderBy = SortBuilder.BuildRoomClassSort(request.PaginationParameters);

        var roomClasses = await _roomClassRepository.GetRoomClassesForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<RoomClassForAdminResponse>>(roomClasses);
    }
}