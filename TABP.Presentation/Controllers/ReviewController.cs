using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.Reviews.Queries.GetDetails;
using TABP.Presentation.DTOs.Review;

namespace TABP.Presentation.Controllers;
[Route("api/reviews")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ReviewController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> GetReviewDetails([FromBody] GetReviewDetailsRequest request)
    {
        var query = _mapper.Map<GetReviewDetailsQuery>(request);
        var paginatedList = await _mediator.Send(query);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(paginatedList.PaginationMetaData));

        return Ok(paginatedList.Items);
    }
}
