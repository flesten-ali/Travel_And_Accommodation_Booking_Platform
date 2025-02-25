﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;
using TABP.Infrastructure.Extensions;
using TABP.Infrastructure.Persistence.DbContexts;

namespace TABP.Infrastructure.Persistence.Repositories;

public class HotelRepository(AppDbContext context) : Repository<Hotel>(context), IHotelRepository
{
    /// <summary>
    /// Searches for hotels based on a filter and returns a paginated list of hotel search results.
    /// </summary>
    /// <param name="filter">The filter expression used to query hotels.</param>
    /// <param name="orderBy">Function to order the hotels query.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="pageNumber">Page number for pagination.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>A paginated response containing hotel search results and pagination metadata.</returns>
    public async Task<PaginatedResponse<SearchHotelResult>> SearchHotelsAsync(
        Expression<Func<Hotel, bool>> filter,
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var hotels = DbSet.Where(filter);

        var selectedResult = orderBy(hotels).Select(h => new SearchHotelResult(default, default, null, default, default, null)
        {
            Name = h.Name,
            Description = h.Description,
            StarRating = h.Rate,
            PricePerNight = h.RoomClasses.Count != 0 ? h.RoomClasses.Min(rc => rc.Price) : 0,
            ThumbnailUrl = context.Images
                   .Where(img => img.ImageableId == h.Id && img.ImageType == ImageType.Thumbnail)
                   .Select(img => img.ImageUrl)
                   .FirstOrDefault() ?? ""

        });

        var resultToReturn = selectedResult.GetRequestedPage(pageSize, pageNumber);
        var paginationMetaData = await resultToReturn.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedResponse<SearchHotelResult>(await resultToReturn.ToListAsync(cancellationToken), paginationMetaData);
    }

    /// <summary>
    /// Retrieves a list of featured hotel deals, ordered by discounted price.
    /// </summary>
    /// <param name="limit">The maximum number of featured deals to return.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>A list of featured hotel deals with their details.</returns>
    public async Task<IEnumerable<FeaturedDealResult>> GetFeaturedDealsAsync(
        int limit,
        CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.UtcNow;

        var featuredDeals = await DbSet
            .Include(h => h.City)
            .Include(h => h.RoomClasses)
               .ThenInclude(rc => rc.Discounts)
            .Where(h => h.RoomClasses.Count != 0)
            .Select(h => new
            {
                Hotel = h,
                DiscountedPrice = h.RoomClasses.Min(rc =>
                    rc.Discounts.Count != 0
                    ? rc.Price * (1 - (rc.Discounts
                    .Where(d => d.StartDate <= currentDate && d.EndDate > currentDate)
                    .Max(d => d.Percentage) / 100))
                    : rc.Price)
            })
            .OrderBy(x => x.DiscountedPrice)
            .Take(limit)
            .Select(x => new FeaturedDealResult(default, default, null, default, default, default, default, default)
            {
                Description = x.Hotel.Description,
                CityName = x.Hotel.City.Name,
                Id = x.Hotel.Id,
                Name = x.Hotel.Name,
                StarRate = x.Hotel.Rate,
                ThumbnailUrl = context.Images
                       .Where(img => img.ImageableId == x.Hotel.Id && img.ImageType == ImageType.Thumbnail)
                       .Select(img => img.ImageUrl)
                       .FirstOrDefault() ?? "",

                DiscountedPrice = x.DiscountedPrice,
                OriginalPrice = x.Hotel.RoomClasses.Max(rc => rc.Price),
            })
            .ToListAsync(cancellationToken);

        return featuredDeals;
    }

    /// <summary>
    /// Retrieves a paginated list of hotels for admin management, ordered by the specified function.
    /// </summary>
    /// <param name="orderBy">Function to order the hotels query.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="pageNumber">Page number for pagination.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>A paginated response containing hotel details for admin management and pagination metadata.</returns>
    public async Task<PaginatedResponse<HotelForAdminResult>> GetHotelsForAdminAsync(
        Func<IQueryable<Hotel>,IOrderedQueryable<Hotel>> orderBy,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default)
    {
        var allHotels = DbSet.AsNoTracking();

        var hotels = orderBy(allHotels).Select(h => new HotelForAdminResult(default, default, default, default, default, default, null)
        {
            Id = h.Id,
            CityName = h.City.Name,
            Name = h.Name,
            Rate = h.Rate,
            CreatedDate = h.CreatedDate,
            UpdatedDate = h.UpdatedDate,
            OwnerName = h.Owner.Name,
        });

        var requestedPage = PaginationExtenstions.GetRequestedPage(hotels, pageSize, pageNumber);
        var paginationMetaDate = await requestedPage.GetPaginationMetaDataAsync(pageSize, pageNumber, cancellationToken);

        return new PaginatedResponse<HotelForAdminResult>(await hotels.ToListAsync(cancellationToken), paginationMetaDate);
    }
}