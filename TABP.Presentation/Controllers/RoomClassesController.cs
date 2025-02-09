using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.RoomClasses.Queries.GetDetails;
using TABP.Presentation.DTOs.RoomClass;

namespace TABP.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RoomClassesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RoomClassesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("GetDetails")]
    public async Task<IActionResult> GetRoomClassDetails([FromBody] GetRoomClassDetailsRequest request)
    {
        var query = _mapper.Map<GetHotelRoomClassesQuery>(request);
        var paginatedList = await _mediator.Send(query);

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginatedList.PaginationMetaData));

        return Ok(paginatedList.Items);
    }
}
