using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Reviews.Commands.Create;
using TABP.Application.Reviews.Commands.Delete;
using TABP.Application.Reviews.Commands.Update;
using TABP.Application.Reviews.Queries.GetById;
using TABP.Application.Reviews.Queries.GetForHotel;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Review;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing hotel reviews, including creating, retrieving, updating, and deleting reviews.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/{hotelId:guid}/reviews")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
public class ReviewsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of reviews for the specified hotel.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel.</param>
    /// <param name="request">The request containing filtering options for the reviews.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of reviews for the hotel.</returns>
    /// <response code="200">Successfully retrieved the reviews for the hotel.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="404">No reviews found for the specified hotel.</response>
    [HttpGet]
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

    /// <summary>
    /// Creates a new review for the specified hotel.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel to review.</param>
    /// <param name="request">The review details to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The newly created review.</returns>
    /// <response code="201">Successfully created the review.</response>
    /// <response code="400">The input data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to create a review.</response>
    [HttpPost]
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

    /// <summary>
    /// Retrieves a review by its unique identifier.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel for the review.</param>
    /// <param name="id">The unique identifier of the review.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The review details corresponding to the specified ID.</returns>
    /// <response code="200">Successfully retrieved the review.</response>
    /// <response code="400">The review ID is invalid.</response>
    /// <response code="404">No review found with the specified ID.</response>
    [HttpGet("{id:guid}")]
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

    /// <summary>
    /// Updates the details of an existing review using its ID.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel for the review.</param>
    /// <param name="id">The unique identifier of the review to be updated.</param>
    /// <param name="request">The updated review details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the review is successfully updated.</returns>
    /// <response code="204">Successfully updated the review.</response>
    /// <response code="400">The input data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to update the review.</response>
    /// <response code="404">No review found with the specified ID.</response>
    [HttpPut("{id:guid}")]
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

    /// <summary>
    /// Deletes an existing review by its unique identifier.
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel for the review.</param>
    /// <param name="id">The unique identifier of the review to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the review is successfully deleted.</returns>
    /// <response code="204">Successfully deleted the review.</response>
    /// <response code="400">The review ID is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to delete the review.</response>
    /// <response code="404">No review found with the specified ID.</response>
    [HttpDelete("{id:guid}")]
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
