using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Bookings.Commands.Create;
using TABP.Application.Bookings.Queries.PdfConfirmation;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Booking;
namespace TABP.Presentation.Controllers;

[Route("api/bookings")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BookingController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBooking([FromBody] AddBookingRequest request)
    {
        var command = _mapper.Map<CreateBookingCommand>(request);

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{id:guid}/invoice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetInvoicePdf(Guid id)
    {
        var query = new GetInvoicePdfQuery { BookingId = id };

        var result = await _mediator.Send(query);

        return File(result.PdfContent, "application/pdf", "invoice.pdf");
    }
}
