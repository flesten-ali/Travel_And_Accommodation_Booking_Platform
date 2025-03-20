using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TABP.Application.Extensions;
using TABP.Application.Helpers;
using TABP.Domain.Entities;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Hotels.Queries.SearchHotels;

/// <summary>
/// Handles the query to search hotels based on specified criteria such as price range, rating, room type, etc.
/// </summary>
public class SearchHotelsQueryHandler :
    IRequestHandler<SearchHotelsQuery, PaginatedResponse<SearchHotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public SearchHotelsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the query to search for hotels with specified filters and sorting options.
    /// </summary>
    /// <param name="request">The query containing search criteria and pagination parameters.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation.
    /// The result is a paginated response containing search results as <see cref="SearchHotelResponse"/>.
    /// </returns>
    public async Task<PaginatedResponse<SearchHotelResponse>> Handle(
        SearchHotelsQuery request,
        CancellationToken cancellationToken = default)
    {
        var filter = BuildFilterExpression(request);
        var orderBy = SortBuilder.BuildHotelSort(request.PaginationParameters);

        var result = await _hotelRepository.SearchHotelsAsync(
            filter,
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<SearchHotelResponse>>(result);
    }

    /// <summary>
    /// Builds a combined filter expression based on the search criteria.
    /// </summary>
    /// <param name="request">The search query containing the filter criteria.</param>
    /// <returns>A combined expression representing the filter for hotel search.</returns>
    private static Expression<Func<Hotel, bool>> BuildFilterExpression(SearchHotelsQuery request)
    {
        var filters = new List<Expression<Func<Hotel, bool>>>
        {
               IsPriceInRangeExpression(request),
               IsRateMatchExpression(request) ,
               IsRoomTypeMatchExpression(request) ,
               IsAdultsMatchExpression(request) ,
               IsCityMatchExpression(request) ,
               IsRoomTypeMatchExpression(request) ,
               IsAmenityMatchExpression(request),
               IsRoomClassAvailabeExpression(request)
        };

        var combinedFilter = filters.Aggregate((current, next) => current.And(next));
        return combinedFilter;
    }

    /// <summary>
    /// Creates a filter expression for price range.
    /// </summary>
    private static Expression<Func<Hotel, bool>> IsPriceInRangeExpression(SearchHotelsQuery request)
    {
        return hotel =>
             (request.MinPrice == null || hotel.RoomClasses.Any(rc => rc.Price >= request.MinPrice)) &&
             (request.MaxPrice == null || hotel.RoomClasses.Any(rc => rc.Price <= request.MaxPrice));
    }

    /// <summary>
    /// Creates a filter expression for matching star rating.
    /// </summary>
    private static Expression<Func<Hotel, bool>> IsRateMatchExpression(SearchHotelsQuery request)
    {
        return hotel =>
             (request.StarRating == null || hotel.Rate == request.StarRating);
    }

    /// <summary>
    /// Creates a filter expression for matching room type.
    /// </summary>
    private static Expression<Func<Hotel, bool>> IsRoomTypeMatchExpression(SearchHotelsQuery request)
    {
        var roomType = request.RoomType?.ToLower();
        return hotel =>
            (string.IsNullOrEmpty(request.RoomType) ||
             hotel.RoomClasses.Any(rc => rc.RoomType.ToString().ToLower().Contains(roomType)));
    }

    /// <summary>
    /// Creates a filter expression for matching adults capacity.
    /// </summary>
    private static Expression<Func<Hotel, bool>> IsAdultsMatchExpression(SearchHotelsQuery request)
    {
        return hotel =>
             (request.AdultsCapacity == 0 || hotel.RoomClasses.Any(rc => rc.AdultsCapacity == request.AdultsCapacity)) &&
             (request.ChildrenCapacity == 0 || hotel.RoomClasses.Any(rc => rc.ChildrenCapacity == request.ChildrenCapacity));
    }

    /// <summary>
    /// Creates a filter expression for matching city name.
    /// </summary>
    private static Expression<Func<Hotel, bool>> IsCityMatchExpression(SearchHotelsQuery request)
    {
        return hotel =>
             (string.IsNullOrEmpty(request.City) || hotel.City.Name.ToLower().Equals(request.City.ToLower()));
    }

    /// <summary>
    /// Creates a filter expression for checking room class availability.
    /// </summary>
    private static Expression<Func<Hotel, bool>> IsRoomClassAvailabeExpression(SearchHotelsQuery request)
    {
        return hotel => hotel.RoomClasses
        .Any(rc => rc.Rooms.Count(room =>
                room.Bookings.All(b => b.CheckOutDate <= request.CheckInDate || b.CheckInDate >= request.CheckOutDate))
        == request.NumberOfRooms);
    }

    /// <summary>
    /// Creates a filter expression for matching amenities.
    /// </summary>
    private static Expression<Func<Hotel, bool>> IsAmenityMatchExpression(SearchHotelsQuery request)
    {
        return hotel => (request.Amenities == null || !request.Amenities.Any() ||
             request.Amenities.All(reqAmenity =>
                 hotel.RoomClasses.Any(rc => rc.Name.ToLower().Contains(reqAmenity.ToLower()))));
    }
}