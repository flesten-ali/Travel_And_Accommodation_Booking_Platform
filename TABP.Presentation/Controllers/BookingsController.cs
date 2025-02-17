using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Bookings.Commands.Create;
using TABP.Application.Bookings.Queries.GetBookingById;
using TABP.Application.Bookings.Queries.InvoicePdf;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Booking;
namespace TABP.Presentation.Controllers;

[Route("api/bookings")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
public class BookingsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{id:guid}")]
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateBookingCommand>(request);

        var booking = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
    }

    [HttpPost("{id:guid}/invoice")]
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
}
