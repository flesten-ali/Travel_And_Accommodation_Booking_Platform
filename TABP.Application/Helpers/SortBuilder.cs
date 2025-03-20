using TABP.Application.Shared;
using TABP.Domain.Entities;
using TABP.Domain.Enums;

namespace TABP.Application.Helpers;

/// <summary>
/// Provides static methods to build sorting expressions for various entities based on the given pagination parameters.
/// </summary>
public class SortBuilder
{
    /// <summary>
    /// Builds a sorting expression for the <see cref="Hotel"/> entity based on the specified pagination parameters.
    /// </summary>
    /// <param name="paginationParameters">The pagination parameters, including sorting column and order.</param>
    /// <returns>A function that applies the sorting to an <see cref="IQueryable{Hotel}"/>.</returns>
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

    /// <summary>
    /// Builds a sorting expression for the <see cref="City"/> entity based on the specified pagination parameters.
    /// </summary>
    /// <param name="paginationParameters">The pagination parameters, including sorting column and order.</param>
    /// <returns>A function that applies the sorting to an <see cref="IQueryable{City}"/>.</returns>
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

    /// <summary>
    /// Builds a sorting expression for the <see cref="Review"/> entity based on the specified pagination parameters.
    /// </summary>
    /// <param name="paginationParameters">The pagination parameters, including sorting column and order.</param>
    /// <returns>A function that applies the sorting to an <see cref="IQueryable{Review}"/>.</returns>
    public static Func<IQueryable<Review>, IOrderedQueryable<Review>> BuildReviewSort(
        PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "date" => isDescending
                    ? (reviews) => reviews.OrderByDescending(x => x.CreatedDate)
                    : (reviews) => reviews.OrderBy(x => x.CreatedDate),

            _ => (reviews) => reviews.OrderBy(h => h.Id)
        };
    }

    /// <summary>
    /// Builds a sorting expression for the <see cref="RoomClass"/> entity based on the specified pagination parameters.
    /// </summary>
    /// <param name="paginationParameters">The pagination parameters, including sorting column and order.</param>
    /// <returns>A function that applies the sorting to an <see cref="IQueryable{RoomClass}"/>.</returns>
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

    /// <summary>
    /// Builds a sorting expression for the <see cref="Room"/> entity based on the specified pagination parameters.
    /// </summary>
    /// <param name="paginationParameters">The pagination parameters, including sorting column and order.</param>
    /// <returns>A function that applies the sorting to an <see cref="IQueryable{Room}"/>.</returns>
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

    /// <summary>
    /// Builds a sorting expression for the <see cref="CartItem"/> entity based on the specified pagination parameters.
    /// </summary>
    /// <param name="paginationParameters">The pagination parameters, including sorting column and order.</param>
    /// <returns>A function that applies the sorting to an <see cref="IQueryable{CartItem}"/>.</returns>
    public static Func<IQueryable<CartItem>, IOrderedQueryable<CartItem>> BuildCartItemSort(
       PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "price" => isDescending
                    ? (cartItems) => cartItems.OrderByDescending(x => x.RoomClass.Price)
                    : (cartItems) => cartItems.OrderBy(x => x.RoomClass.Price),

            _ => (cartItems) => cartItems.OrderBy(h => h.Id)
        };
    }

    /// <summary>
    /// Builds a sorting expression for the <see cref="Discount"/> entity based on the specified pagination parameters.
    /// </summary>
    /// <param name="paginationParameters">The pagination parameters, including sorting column and order.</param>
    /// <returns>A function that applies the sorting to an <see cref="IQueryable{Discount}"/>.</returns>
    public static Func<IQueryable<Discount>, IOrderedQueryable<Discount>> BuildDiscountSort(
      PaginationParameters paginationParameters)
    {
        var isDescending = paginationParameters.SortOrder == SortOrder.Descending;
        return paginationParameters.OrderColumn switch
        {
            "percentage" => isDescending
                    ? (discounts) => discounts.OrderByDescending(x => x.Percentage)
                    : (discounts) => discounts.OrderBy(x => x.Percentage),

            "startdate" => isDescending
                    ? (discounts) => discounts.OrderByDescending(x => x.StartDate)
                    : (discounts) => discounts.OrderBy(x => x.StartDate),

            "enddate" => isDescending
                    ? (discounts) => discounts.OrderByDescending(x => x.EndDate)
                    : (discounts) => discounts.OrderBy(x => x.EndDate),

            _ => (discounts) => discounts.OrderBy(h => h.Id)
        };
    }
}