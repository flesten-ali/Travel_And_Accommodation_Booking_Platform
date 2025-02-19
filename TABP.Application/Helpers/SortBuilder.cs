using TABP.Application.Shared;
using TABP.Domain.Entities;
using TABP.Domain.Enums;

namespace TABP.Application.Helper;
public class SortBuilder
{
    public static Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> BuildHotelSort(
        PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "name" => isDescending
                    ? (hotels) => hotels.OrderByDescending(x => x.Name)
                    : (hotels) => hotels.OrderBy(x => x.Name),

            "price" => isDescending
                    ? (hotels) => hotels.OrderByDescending(h => h.RoomClasses.Max(rc => rc.Price))
                    : (hotels) => hotels.OrderBy(h => h.RoomClasses.Min(rc => rc.Price)),

            "starrating" => isDescending
                    ? (hotels) => hotels.OrderByDescending(h => h.Rate)
                    : (hotels) => hotels.OrderBy(h => h.Rate),

            _ => (hotels) => hotels.OrderBy(h => h.Id)
        };
    }

    public static Func<IQueryable<City>, IOrderedQueryable<City>> BuildCitySort(
        PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn?.ToLower().Trim() switch
        {
            "name" => isDescending
            ? cities => cities.OrderByDescending(x => x.Name)
            : cities => cities.OrderBy(x => x.Name),

            "country" => isDescending
            ? cities => cities.OrderByDescending(x => x.Country)
            : cities => cities.OrderBy(x => x.Country),

            _ => cities => cities.OrderBy(x => x.Id),
        };
    }

    public static Func<IQueryable<Review>, IOrderedQueryable<Review>> BuildReviewSort(
        PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "date" => isDescending
                    ? (reviews) => reviews.OrderByDescending(x => x.CreatedAt)
                    : (reviews) => reviews.OrderBy(x => x.CreatedAt),

            _ => (reviews) => reviews.OrderBy(h => h.Id)
        };
    }

    public static Func<IQueryable<RoomClass>, IOrderedQueryable<RoomClass>> BuildRoomClassSort(
        PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "date" => isDescending
                    ? (roomClasses) => roomClasses.OrderByDescending(x => x.CreatedDate)
                    : (roomClasses) => roomClasses.OrderBy(x => x.CreatedDate),

            "adultscapacity" => isDescending
                    ? (roomClasses) => roomClasses.OrderByDescending(x => x.AdultsCapacity)
                    : (roomClasses) => roomClasses.OrderBy(x => x.AdultsCapacity),

            "childrencapacity" => isDescending
          ? (roomClasses) => roomClasses.OrderByDescending(x => x.ChildrenCapacity)
          : (roomClasses) => roomClasses.OrderBy(x => x.ChildrenCapacity),

            _ => (roomClasses) => roomClasses.OrderBy(h => h.Id)
        };
    }

    public static Func<IQueryable<Room>, IOrderedQueryable<Room>> BuildRoomSort(
     PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "date" => isDescending
                    ? (rooms) => rooms.OrderByDescending(x => x.CreatedDate)
                    : (rooms) => rooms.OrderBy(x => x.CreatedDate),

            "roomnumber" => isDescending
                        ? (rooms) => rooms.OrderByDescending(x => x.RoomNumber)
                        : (rooms) => rooms.OrderBy(x => x.RoomNumber),

            _ => (rooms) => rooms.OrderBy(h => h.Id)
        };
    }

    public static Func<IQueryable<CartItem>, IOrderedQueryable<CartItem>> BuildCartItemSort(
       PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "price" => isDescending
                    ? (cartItems) => cartItems.OrderByDescending(x => x.RoomClass.Price)
                    : (cartItems) => cartItems.OrderBy(x => x.RoomClass.Price),

            _ => (rooms) => rooms.OrderBy(h => h.Id)
        };
    }
}
