using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetForHotel;

/// <summary>
/// Handles the query to retrieve a paginated list of room classes for a specific hotel.
/// </summary>
public class GetHotelRoomClassesQueryHandler
    : IRequestHandler<GetHotelRoomClassesQuery, PaginatedResponse<HotelRoomClassesQueryResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetHotelRoomClassesQueryHandler(
        IRoomClassRepository roomClassRepository,
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to retrieve a paginated list of room classes for a specific hotel.
    /// </summary>
    /// <param name="request">The query containing the hotel ID and pagination parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, returning a
    /// <see cref="PaginatedResponse{HotelRoomClassesQueryResponse}"/> 
    /// containing the paginated room class data for the hotel.</returns>
    public async Task<PaginatedResponse<HotelRoomClassesQueryResponse>> Handle(
        GetHotelRoomClassesQuery request,
        CancellationToken cancellationToken = default)
    {
        var orderBy = SortBuilder.BuildRoomClassSort(request.PaginationParameters);

        var roomClasses = await _roomClassRepository
            .GetByHotelIdAsync(
            orderBy,
            request.HotelId,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<HotelRoomClassesQueryResponse>>(roomClasses);
    }
}