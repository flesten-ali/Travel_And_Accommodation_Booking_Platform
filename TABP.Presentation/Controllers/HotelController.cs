using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.Hotels.Queries.GetDetails;
using TABP.Application.Hotels.Queries.SearchHotels;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Controllers;

[Route("api/[controller]")]
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
        var command = _mapper.Map<SearchHotelsQuery>(request);
        var paginatedList = await _mediator.Send(command);

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginatedList.PaginationMetaData));

        return Ok(paginatedList.Items);
    }

    [HttpGet("GetDetails{id:guid}")]
    public async Task<IActionResult> GetHotelDetails(Guid id)
    {
        var query = new GetHotelDetailsQuery()
        {
            HotelId = id
        };
        var response = await _mediator.Send(query);
        return Ok(response);
    }
}