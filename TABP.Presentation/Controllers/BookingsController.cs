using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Bookings.Commands.Create;
using TABP.Application.Bookings.Commands.Delete;
using TABP.Application.Bookings.Queries.GetBookingById;
using TABP.Application.Bookings.Queries.InvoicePdf;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Booking;
namespace TABP.Presentation.Controllers;

[Route("api/bookings")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
[SwaggerTag("Manage hotel bookings for guests.")]
public class BookingsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get a booking by ID",
        Description = "Fetches booking details for the given ID."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingByIdQuery { BookingId = id };

        var booking = await _mediator.Send(query, cancellationToken);

        return Ok(booking);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new booking",
        Description = "Creates a new hotel booking for a guest."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateBookingCommand>(request);

        var createdBooking = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, createdBooking);
    }

    [HttpPost("{id:guid}/invoice")]
    [SwaggerOperation(
        Summary = "Get invoice PDF",
        Description = "Generates and retrieves an invoice PDF for the booking."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetInvoicePdf(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetInvoicePdfQuery { BookingId = id };

        var result = await _mediator.Send(query, cancellationToken);

        return File(result.PdfContent, "application/pdf", "invoice.pdf");
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete a booking",
        Description = "Delete an existing booking by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBooking(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBookingCommand { Id = id };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
