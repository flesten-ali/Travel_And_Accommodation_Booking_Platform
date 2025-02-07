using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.Hotels.Queries.Search;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Controllers;

[Route("api/hotels")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public HotelController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchHotelRequest request)
    {
        var command = _mapper.Map<SearchHotelQuery>(request);
        var paginatedList = await _mediator.Send(command);

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginatedList.PaginationMetaData));

        return Ok(paginatedList.Items);
    }
}