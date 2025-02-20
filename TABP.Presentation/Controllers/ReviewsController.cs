﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.Reviews.Commands.Create;
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

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(reviews.PaginationMetaData));

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
        var command = _mapper.Map<CreateReviewCommand>(request);
        command.UserId = User.GetUserId();
        command.HotelId = hotelId;

        var createdReview = await _mediator.Send(command, cancellationToken);

        return Created();
        //return CreatedAtAction(nameof(GetReview), new { id = createdReview.Id }, createdReview);
    }
}
