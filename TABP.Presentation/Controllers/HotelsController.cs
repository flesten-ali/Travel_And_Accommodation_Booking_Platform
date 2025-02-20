using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
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
namespace TABP.Presentation.Controllers;

[Route("api/hotels")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
[SwaggerTag("Hotel Management API")]
public class HotelsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Search hotels",
        Description = "Search for hotels using various filters such as location, price, and amenities."
    )]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchHotels(
        [FromQuery] SearchHotelRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SearchHotelsQuery>(request);

        var hotels = await _mediator.Send(command, cancellationToken);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(hotels.PaginationMetaData));

        return Ok(hotels.Items);
    }

    [HttpGet("{id:guid}/details")]
    [SwaggerOperation(
        Summary = "Get hotel details",
        Description = "Fetch detailed information about a specific hotel."
    )]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHotelDetails(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetHotelQuery()
        {
            HotelId = id
        };

        var hotel = await _mediator.Send(query, cancellationToken);

        return Ok(hotel);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new hotel",
        Description = "Create a new hotel by providing necessary details such as name, address, and facilities."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateHotel(
        CreateHotelRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateHotelCommand>(request);

        var createdHotel = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetHotel), new { id = createdHotel.Id }, createdHotel);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get hotel by ID",
        Description = "Retrieve information of a hotel by its unique identifier."
    )]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHotel(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetHotelByIdQuery { HotelId = id };

        var hotel = await _mediator.Send(query, cancellationToken);

        return Ok(hotel);
    }

    [HttpPost("{id:guid}/thumbnail")]
    [SwaggerOperation(
        Summary = "Upload hotel thumbnail",
        Description = "Upload a thumbnail image for the specified hotel."
    )]
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
        var command = _mapper.Map<UploadHotelThumbnailCommand>(request);
        command.HotelId = id;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/gallery")]
    [SwaggerOperation(
        Summary = "Upload hotel gallery images",
        Description = "Upload images to the gallery for the specified hotel."
    )]
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
        var command = _mapper.Map<UploadHotelImageGalleryCommand>(request);
        command.HotelId = id;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("featured-deals")]
    [SwaggerOperation(
        Summary = "Get featured hotel deals",
        Description = "Fetch a list of featured deals for hotels based on the specified limit."
    )]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFeaturedDeals(int limit, CancellationToken cancellationToken)
    {
        var query = new GetFeaturedDealsQuery { Limit = limit };

        var featuredDeals = await _mediator.Send(query, cancellationToken); 

        return Ok(featuredDeals);
    }

    [HttpGet("get-for-admin")]
    [SwaggerOperation(
        Summary = "Get hotels for admin",
        Description = "Retrieve a list of hotels for admin management purposes."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHotelsForAdmin(
        [FromQuery] GetHotelsForAdminRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetHotelsForAdminQuery>(request);

        var hotels = await _mediator.Send(query, cancellationToken);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(hotels.PaginationMetaData));

        return Ok(hotels.Items);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update hotel details",
        Description = "Update the details of an existing hotel using its ID."
    )]
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
        var command = _mapper.Map<UpdateHotelCommand>(request);
        command.Id = id;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete a hotel",
        Description = "Delete an existing hotel by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteHotel(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteHotelCommand { Id = id };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}/room-classes")]
    [SwaggerOperation(
        Summary = "Get room classes for a hotel",
        Description = "Retrieve a list of room classes for a specific hotel."
    )]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetHotelRoomClasses(
        Guid id,
        [FromQuery] GetHotelRoomClassesRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetHotelRoomClassesQuery>(request);
        query.HotelId = id;

        var roomClasses = await _mediator.Send(query, cancellationToken);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(roomClasses.PaginationMetaData));

        return Ok(roomClasses.Items);
    }
}