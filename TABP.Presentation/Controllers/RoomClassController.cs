using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.RoomClasses.Queries.GetDetails;
using TABP.Presentation.DTOs.RoomClass;

namespace TABP.Presentation.Controllers;
[Route("api/room-classes")]
[ApiController]
public class RoomClassController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RoomClassController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("GetDetails")]
    public async Task<IActionResult> GetRoomClassDetails([FromBody] GetRoomClassDetailsRequest request)
    {
        var query = _mapper.Map<GetRoomClassDetailsQuery>(request);
        var paginatedList = await _mediator.Send(query);

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginatedList.PaginationMetaData));

        return Ok(paginatedList.Items);
    }
}
