using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.Reviews.Commands.Create;
using TABP.Application.Reviews.Commands.Delete;
using TABP.Application.Reviews.Commands.Update;
using TABP.Application.Reviews.Queries.GetById;
using TABP.Application.Reviews.Queries.GetForHotel;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Review;
using TABP.Presentation.Extensions;
namespace TABP.Presentation.Controllers;

[Route("api/{hotelId:guid}/reviews")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
[SwaggerTag("Hotel Reviews Operations")]
public class ReviewsController(IMediator mediator, IMapper mapper) : ControllerBase
{
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
        var query = mapper.Map<GetHotelReviewsQuery>(request);
        query.HotelId = hotelId;

        var reviews = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(reviews.PaginationMetaData);

        return Ok(reviews.Items);
    }

    [HttpPost]
    [SwaggerOperation(
         Summary = "Create a new review",
         Description = "Create a new review by providing necessary details such as hotel ID."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateReview(
     Guid hotelId,
     CreateReviewRequest request,
     CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateReviewCommand>(request);
        command.UserId = User.GetUserId();
        command.HotelId = hotelId;

        var createdReview = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetReview),
            new
            {
                hotelId = createdReview.HotelId,
                id = createdReview.Id
            }, createdReview);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get review by ID",
        Description = "Retrieve information of a review by its unique identifier."
    )]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReview(Guid hotelId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetReviewByIdQuery(id, hotelId);
         
        var review = await mediator.Send(query, cancellationToken);

        return Ok(review);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update review details",
        Description = "Update the details of an existing review using its ID."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReview(
    Guid hotelId,
    Guid id,
    UpdateReviewRequest request,
    CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateReviewCommand>(request);
        command.Id = id;
        command.HotelId = hotelId;
        command.UserId = User.GetUserId();

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete a review",
        Description = "Delete an existing review by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteReview(Guid hotelId, Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand(id, User.GetUserId(), hotelId);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
