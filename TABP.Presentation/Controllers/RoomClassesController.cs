using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.RoomClasses.Queries.GetDetails;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.RoomClass;
namespace TABP.Presentation.Controllers;

[Route("api/{hotelId:guid}/room-classes")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class RoomClassesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    // think about remove from here/ may fet in the hotel controller/ see this when add rest of crud
    public async Task<IActionResult> GetHotelRoomClasses(Guid hotelId, [FromQuery] GetHotelRoomClassesRequest request)
    {
        var query = _mapper.Map<GetHotelRoomClassesQuery>(request);
        query.HotelId = hotelId;

        var roomClasses = await _mediator.Send(query);

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(roomClasses.PaginationMetaData));

        return Ok(roomClasses.Items);
    }
}
