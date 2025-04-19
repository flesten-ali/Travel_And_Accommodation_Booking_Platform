using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Hotels.Commands.Create;
using TABP.Application.Hotels.Commands.Delete;
using TABP.Application.Hotels.Commands.ImageGallery;
using TABP.Application.Hotels.Commands.Thumbnail;
using TABP.Application.Hotels.Commands.Update;
using TABP.Application.Hotels.Queries.GetById;
using TABP.Application.Hotels.Queries.GetDetails;
using TABP.Application.Hotels.Queries.GetFeaturedDeals;
using TABP.Application.Hotels.Queries.GetForAdmin;
using TABP.Application.Hotels.Queries.SearchHotels;
using TABP.Application.RoomClasses.Queries.GetForHotel;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs;
using TABP.Presentation.DTOs.Hotel;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing hotel-related operations, including search, creation, updating, and deleting hotels.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/hotels")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class HotelsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Searches for hotels using various filters such as location, price, and amenities.
    /// </summary>
    /// <param name="request">The request containing the search filters for hotels.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of hotels matching the search criteria.</returns>
    /// <response code="200">Returns a list of hotels matching the search criteria.</response>
    /// <response code="400">The request data is invalid.</response>
    [HttpGet("search")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchHotels(
        [FromQuery] SearchHotelRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<SearchHotelsQuery>(request);

        var hotels = await mediator.Send(command, cancellationToken);

        Response.AddPaginationHeader(hotels.PaginationMetaData);

        return Ok(hotels.Items);
    }

    /// <summary>
    /// Retrieves detailed information about a specific hotel by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the hotel.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The details of the hotel with the given ID.</returns>
    /// <response code="200">Returns the hotel details.</response>
    /// <response code="404">The hotel with the specified ID was not found.</response>
    [HttpGet("{id:guid}/details")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHotelDetails(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetHotelQuery(id);

        var hotel = await mediator.Send(query, cancellationToken);

        return Ok(hotel);
    }

    /// <summary>
    /// Creates a new hotel by providing necessary details such as name, address, and facilities.
    /// </summary>
    /// <param name="request">The request containing the hotel creation details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created hotel.</returns>
    /// <response code="201">Returns the created hotel.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the necessary permissions to create a hotel.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateHotel(
        CreateHotelRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateHotelCommand>(request);

        var createdHotel = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetHotel), new { id = createdHotel.Id }, createdHotel);
    }

    /// <summary>
    /// Retrieves a hotel by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the hotel.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The hotel with the specified ID.</returns>
    /// <response code="200">Returns the hotel with the specified ID.</response>
    /// <response code="404">The hotel with the specified ID was not found.</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotel(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetHotelByIdQuery(id);

        var hotel = await mediator.Send(query, cancellationToken);

        return Ok(hotel);
    }

    /// <summary>
    /// Uploads a thumbnail image for a specific hotel.
    /// </summary>
    /// <param name="id">The unique identifier of the hotel.</param>
    /// <param name="request">The request containing the thumbnail image data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the upload is successful.</returns>
    /// <response code="204">The thumbnail image was successfully uploaded.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="404">The hotel with the specified ID was not found.</response>
    /// <response code="403">The user does not have the necessary permissions to upload the thumbnail.</response>
    /// <response code="401">The user is not authenticated.</response>
    [HttpPost("{id:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadHotelThumbnail(
        Guid id,
        [FromForm] UploadHotelThumbnailRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UploadHotelThumbnailCommand>(request);
        command.HotelId = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Uploads images to the gallery for a specific hotel.
    /// </summary>
    /// <param name="id">The unique identifier of the hotel.</param>
    /// <param name="request">The request containing the gallery images data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the upload is successful.</returns>
    /// <response code="204">The gallery images were successfully uploaded.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="404">The hotel with the specified ID was not found.</response>
    /// <response code="403">The user does not have the necessary permissions to upload the gallery images.</response>
    /// <response code="401">The user is not authenticated.</response>
    [HttpPost("{id:guid}/gallery")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadHotelGalleryImages(
        Guid id,
        [FromForm] UploadHotelImageGalleryRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UploadHotelImageGalleryCommand>(request);
        command.HotelId = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Retrieves featured hotel deals based on the specified limit.
    /// </summary>
    /// <param name="limit">The number of featured deals to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of featured hotel deals.</returns>
    /// <response code="200">Returns the list of featured hotel deals.</response>
    /// <response code="400">The request data is invalid.</response>
    [HttpGet("featured-deals")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFeaturedDeals(int limit, CancellationToken cancellationToken)
    {
        var query = new GetFeaturedDealsQuery(limit);

        var featuredDeals = await mediator.Send(query, cancellationToken);

        return Ok(featuredDeals);
    }

    /// <summary>
    /// Retrieves hotels for admin management purposes.
    /// </summary>
    /// <param name="request">The request containing the filters for retrieving hotels.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of hotels for admin management.</returns>
    /// <response code="200">Returns a list of hotels for admin management.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="403">The user does not have the necessary permissions to access this resource.</response>
    [HttpGet("get-for-admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHotelsForAdmin(
        [FromQuery] GetHotelsForAdminRequest request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetHotelsForAdminQuery>(request);

        var hotels = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(hotels.PaginationMetaData);

        return Ok(hotels.Items);
    }

    /// <summary>
    /// Updates the details of an existing hotel using its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the hotel.</param>
    /// <param name="request">The request containing the updated hotel details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the hotel details were successfully updated.</returns>
    /// <response code="204">The hotel details were successfully updated.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="404">The hotel with the specified ID was not found.</response>
    /// <response code="403">The user does not have the necessary permissions to update the hotel.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHotel(
        Guid id,
        UpdateHotelRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateHotelCommand>(request);
        command.Id = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing hotel by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the hotel.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the hotel was successfully deleted.</returns>
    /// <response code="204">The hotel was successfully deleted.</response>
    /// <response code="404">The hotel with the specified ID was not found.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="403">The user does not have the necessary permissions to delete the hotel.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteHotel(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteHotelCommand(id);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Retrieves a list of room classes for a specific hotel.
    /// </summary>
    /// <param name="id">The unique identifier of the hotel.</param>
    /// <param name="request">The request containing the filters for retrieving room classes.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of room classes available for the hotel.</returns>
    /// <response code="200">Returns a list of room classes available for the hotel.</response>
    /// <response code="404">The hotel with the specified ID was not found.</response>
    [HttpGet("{id:guid}/room-classes")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetRoomClassesForHotel(
        Guid id,
        [FromQuery] GetHotelRoomClassesRequest request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetHotelRoomClassesQuery>(request);
        query.HotelId = id;

        var roomClasses = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(roomClasses.PaginationMetaData);

        return Ok(roomClasses.Items);
    }
}