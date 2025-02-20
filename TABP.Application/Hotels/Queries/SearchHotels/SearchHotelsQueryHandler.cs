﻿using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TABP.Application.Extensions;
using TABP.Application.Helpers;
using TABP.Domain.Entities;
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

        return _mapper.Map<PaginatedList<SearchHotelResponse>>(result);
    }

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

    private static Expression<Func<Hotel, bool>> IsPriceInRangeExpression(SearchHotelsQuery request)
    {
        return hotel =>
             (request.MinPrice == null || hotel.RoomClasses.Any(rc => rc.Price >= request.MinPrice)) &&
             (request.MaxPrice == null || hotel.RoomClasses.Any(rc => rc.Price <= request.MaxPrice));
    }

    private static Expression<Func<Hotel, bool>> IsRateMatchExpression(SearchHotelsQuery request)
    {
        return hotel =>
             (request.StarRating == null || hotel.Rate == request.StarRating);
    }

    private static Expression<Func<Hotel, bool>> IsRoomTypeMatchExpression(SearchHotelsQuery request)
    {
        var roomType = request.RoomType?.ToLower();
        return hotel =>
            (string.IsNullOrEmpty(request.RoomType) ||
             hotel.RoomClasses.Any(rc => rc.RoomType.ToString().ToLower().Contains(roomType)));
    }

    private static Expression<Func<Hotel, bool>> IsAdultsMatchExpression(SearchHotelsQuery request)
    {
        return hotel =>
             (request.AdultsCapacity == 0 || hotel.RoomClasses.Any(rc => rc.AdultsCapacity == request.AdultsCapacity)) &&
             (request.ChildrenCapacity == 0 || hotel.RoomClasses.Any(rc => rc.ChildrenCapacity == request.ChildrenCapacity));
    }

    private static Expression<Func<Hotel, bool>> IsCityMatchExpression(SearchHotelsQuery request)
    {
        return hotel =>
             (string.IsNullOrEmpty(request.City) || hotel.City.Name.ToLower().Equals(request.City.ToLower()));
    }

    private static Expression<Func<Hotel, bool>> IsRoomClassAvailabeExpression(SearchHotelsQuery request)
    {
        return hotel => hotel.RoomClasses
        .Any(rc => rc.Rooms.Count(room =>
                room.Bookings.All(b => b.CheckOutDate <= request.CheckInDate || b.CheckInDate >= request.CheckOutDate))
        == request.NumberOfRooms);
    }

    private static Expression<Func<Hotel, bool>> IsAmenityMatchExpression(SearchHotelsQuery request)
    {
        return hotel => (request.Amenities == null || request.Amenities.Count == 0 ||
             request.Amenities.All(reqAmenity =>
                 hotel.RoomClasses.Any(rc => rc.Name.ToLower().Contains(reqAmenity.ToLower()))));
    }
}