using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Bookings.Commands.Add;
using TABP.Application.Bookings.Queries.PdfConfirmation;
using TABP.Presentation.DTOs.Booking;

namespace TABP.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BookingController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("book")]
    public async Task<IActionResult> Add([FromBody] AddBookingRequest request)
    {
        var command = _mapper.Map<AddBookingCommand>(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("get-pdf-cofirmation/{id:guid}")]
    public async Task<IActionResult> GetPdfConfirmation(Guid  id)
    {
        var query = new GetPdfConfirmationQuery { BookingId = id };
        var result  = await _mediator.Send(query);
        return Ok(result);
    }
}
