using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.Reviews.Queries.GetDetails;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Review;
namespace TABP.Presentation.Controllers;

[Route("api/{hotelId:guid}/reviews")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
public class ReviewsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelReviews(Guid hotelId, [FromQuery] GetHotelReviewsRequest request)
    {
        var query = _mapper.Map<GetHotelReviewsQuery>(request);
        query.HotelId = hotelId;

        var reviews = await _mediator.Send(query);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(reviews.PaginationMetaData));

        return Ok(reviews.Items);
    }
}
