using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
namespace TABP.Application.Hotels.Queries.SearchHotels;

public class SearchHotelsQueryHandler :
    IRequestHandler<SearchHotelsQuery, PaginatedList<SearchHotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public SearchHotelsQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<SearchHotelResponse>> Handle(
        SearchHotelsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = BuildFilterExpression(request);
        var orderBy = BuildSort(request);

        var result = await _hotelRepository.SearchHotels(filter, orderBy, request.PageSize, request.PageNumber);
        return _mapper.Map<PaginatedList<SearchHotelResponse>>(result);
    }

    private static Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> BuildSort(SearchHotelsQuery request)
    {
        return request.SortBy?.ToLower() switch
        {
            "name" => request.SortOrder == SortOrder.Descending
                    ? (hotels) => hotels.OrderByDescending(x => x.Name)
                    : (hotels) => hotels.OrderBy(x => x.Name),

            "price" => request.SortOrder == SortOrder.Descending
                    ? (hotels) => hotels.OrderByDescending(h => h.RoomClasses.Max(rc => rc.Price))
                    : (hotels) => hotels.OrderBy(h => h.RoomClasses.Min(rc => rc.Price)),

            "starrating" => request.SortOrder == SortOrder.Descending
                    ? (hotels) => hotels.OrderByDescending(h => h.Rate)
                    : (hotels) => hotels.OrderBy(h => h.Rate),

            _ => (hotels) => hotels.OrderBy(h => h.Id)
        };
    }

    private static Expression<Func<Hotel, bool>> BuildFilterExpression(SearchHotelsQuery request)
    {
        return hotel =>
            (request.MinPrice == null || hotel.RoomClasses.Any(rc => rc.Price >= request.MinPrice)) &&
            (request.MaxPrice == null || hotel.RoomClasses.Any(rc => rc.Price <= request.MaxPrice)) &&

            (request.StarRating == null || hotel.Rate == request.StarRating) &&

            (string.IsNullOrEmpty(request.RoomType) ||
             hotel.RoomClasses.Any(rc => rc.RoomType.ToString().ToLower()
                                          .Contains(request.RoomType.ToLower()))) &&

            (request.AdultsCapacity == 0 || hotel.RoomClasses.Any(rc => rc.AdultsCapacity == request.AdultsCapacity)) &&
            (request.ChildrenCapacity == 0 || hotel.RoomClasses.Any(rc => rc.ChildrenCapacity == request.ChildrenCapacity)) &&

            (string.IsNullOrEmpty(request.City) ||
             hotel.City.Name.ToLower().Equals(request.City.ToLower())) &&

             hotel.RoomClasses.Any(rc =>
               rc.Rooms.Count(room =>
                room.Bookings.All(b => b.CheckOutDate <= request.CheckInDate || b.CheckInDate >= request.CheckOutDate)
                ) >= request.NumberOfRooms)
               &&

        (request.Amenities == null || request.Amenities.Count == 0 ||
             request.Amenities.All(reqAmenity =>
                 hotel.RoomClasses.Any(rc => rc.Name.ToLower().Contains(reqAmenity.ToLower()))));
    }
}