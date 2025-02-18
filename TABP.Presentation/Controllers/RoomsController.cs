using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.Rooms.Queries.GetForAdmin;
using TABP.Presentation.DTOs.Room;
namespace TABP.Presentation.Controllers;

[Route("api/rooms")]
[ApiController]
//[Authorize(Roles = Roles.Admin)]
[SwaggerTag("Room Management")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RoomsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("get-for-admin")]
    [SwaggerOperation(
      Summary = "Get rooms for admin",
      Description = "Fetch a list of rooms for administrative purposes."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoomsForAdmin(
      [FromQuery] GetRoomsForAdminRequest request,
      CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetRoomsForAdminQuery>(request);

        var rooms = await _mediator.Send(query, cancellationToken);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(rooms.PaginationMetaData));

        return Ok(rooms.Items);
    }
}
