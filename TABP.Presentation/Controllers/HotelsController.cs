using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.Hotels.Commands.Create;
using TABP.Application.Hotels.Commands.ImageGallery;
using TABP.Application.Hotels.Commands.Thumbnail;
using TABP.Application.Hotels.Queries.GetDetails;
using TABP.Application.Hotels.Queries.GetFeaturedDeals;
using TABP.Application.Hotels.Queries.GetHotelById;
using TABP.Application.Hotels.Queries.SearchHotels;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Controllers;

[Route("api/hotels")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class HotelsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchHotels([FromQuery] SearchHotelRequest request)
    {
        var command = _mapper.Map<SearchHotelsQuery>(request);

        var hotels = await _mediator.Send(command);

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(hotels.PaginationMetaData));

        return Ok(hotels.Items);
    }

    [HttpGet("{id:guid}/details")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelDetails(Guid id)
    {
        var query = new GetHotelQuery()
        {
            HotelId = id
        };

        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelRequest request)
    {
        var command = _mapper.Map<CreateHotelCommand>(request);

        var createdHotel = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetHotel), new { id = createdHotel.Id }, createdHotel);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHotel(Guid id)
    {
        var query = new GetHotelByIdQuery { HotelId = id };

        var hotel = await _mediator.Send(query);

        return Ok(hotel);
    }

    [HttpPost("{id:guid}/thumbnail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadHotelThumbnail(Guid id, [FromForm] UploadThumbnailRequest request)
    {
        var command = _mapper.Map<UploadThumbnailCommand>(request);
        command.HotelId = id;

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPost("{id:guid}/gallery")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadHotelGalleryImages(Guid id, [FromForm] UploadImageGalleryRequest request)
    {
        var command = _mapper.Map<UploadImageGalleryCommand>(request);
        command.HotelId = id;

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet("featured-deals")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFeaturedDeals(int limit)
    {
        var query = new GetFeaturedDealsQuery { Limit = limit };

        var featuredDeals = await _mediator.Send(query);

        return Ok(featuredDeals);
    }
}