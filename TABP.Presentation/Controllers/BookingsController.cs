using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Bookings.Commands.Create;
using TABP.Application.Bookings.Commands.Delete;
using TABP.Application.Bookings.Queries.GetById;
using TABP.Application.Bookings.Queries.InvoicePdf;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Booking;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing hotel bookings for guests.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/user/bookings")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
public class BookingsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Retrieves booking details for a given ID.
    /// </summary>
    /// <param name="id">The unique identifier of the booking.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The booking details if found.</returns>
    /// <response code="200">The booking details were successfully retrieved.</response>
    /// <response code="404">The specified booking was not found.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to access the booking details.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingByIdQuery(id);

        var booking = await mediator.Send(query, cancellationToken);

        return Ok(booking);
    }

    /// <summary>
    /// Creates a new hotel booking for a guest.
    /// </summary>
    /// <param name="request">The booking details.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The newly created booking.</returns>
    /// <response code="201">The booking was successfully created.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to create a booking.</response>
    /// <response code="404">The specified resource was not found.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBooking(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateBookingCommand>(request);
        command.UserId = User.GetUserId();

        var createdBooking = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, createdBooking);
    }

    /// <summary>
    /// Generates and retrieves an invoice PDF for the booking.
    /// </summary>
    /// <param name="id">The unique identifier of the booking.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A PDF file containing the invoice.</returns>
    /// <response code="200">The invoice PDF was successfully generated.</response>
    /// <response code="404">The specified booking was not found.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to retrieve the invoice.</response>
    [HttpGet("{id:guid}/invoice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetInvoicePdf(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetInvoicePdfQuery(id);

        var result = await mediator.Send(query, cancellationToken);

        return File(result.PdfContent, "application/pdf", "invoice.pdf");
    }

    /// <summary>
    /// Deletes an existing booking by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the booking to be deleted.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>No content response if successful.</returns>
    /// <response code="204">The booking was successfully deleted.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have permission to delete the booking.</response>
    /// <response code="404">The specified booking was not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBooking(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBookingCommand(id, User.GetUserId());

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
