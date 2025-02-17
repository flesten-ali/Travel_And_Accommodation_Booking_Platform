using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.Reviews.GetForHotel;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Review;
namespace TABP.Presentation.Controllers;

[Route("api/{hotelId:guid}/reviews")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
[SwaggerTag("Hotel Reviews Operations")]
public class ReviewsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get reviews for a hotel",
        Description = "Fetch a list of reviews for the specified hotel."
    )]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelReviews(
        Guid hotelId,
        [FromQuery] GetHotelReviewsRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetHotelReviewsQuery>(request);
        query.HotelId = hotelId;

        var reviews = await _mediator.Send(query, cancellationToken);

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(reviews.PaginationMetaData));

        return Ok(reviews.Items);
    }
}
