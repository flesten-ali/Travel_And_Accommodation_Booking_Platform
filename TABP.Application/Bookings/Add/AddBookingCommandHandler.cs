using MediatR;
using TABP.Domain.Constants;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Email;
using TABP.Domain.Interfaces.Services.Html;
using TABP.Domain.Interfaces.Services.Pdf;
using TABP.Domain.Models;

namespace TABP.Application.Bookings.Add;
public class AddBookingCommandHandler : IRequestHandler<AddBookingCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPdfService _pdfService;
    private readonly IInvoiceHtmlGenerationService _invoiceHtmlGenerationService;
    private readonly IEmailSenderService _emailSenderService;

    public AddBookingCommandHandler(
        IUserRepository userRepository,
        IRoomRepository roomRepository,
        IHotelRepository hotelRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IPdfService pdfService,
        IInvoiceHtmlGenerationService invoiceHtmlGenerationService,
        IEmailSenderService emailSenderService
    )
    {
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _pdfService = pdfService;
        _invoiceHtmlGenerationService = invoiceHtmlGenerationService;
        _emailSenderService = emailSenderService;
    }

    public async Task<Guid> Handle(AddBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (user.Role != Roles.Guest)
        {
            throw new UserUnauthorizedException("Only Guest can make bookings");
        }

        var rooms = await _roomRepository.GetAllByIdAsync(request.RoomIds);
        if (rooms == null || rooms.Count() != request.RoomIds.Count())
        {
            throw new NotFoundException("One or more room not found");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var booking = new Booking
            {
                BookingDate = DateTime.UtcNow,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                PaymentMethod = request.PaymentMethod,
                Remarks = request.Remarks,
                UserId = request.UserId,
                TotalPrice = CalculateTotalPric(rooms, request.CheckInDate, request.CheckOutDate),
                Rooms = rooms.ToList(),
                User = user,
            };
            await _bookingRepository.AddAsync(booking);

            var invoice = new Invoice
            {
                InvoiceId = Guid.NewGuid(),
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                IssueDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Completed,
                TotalPrice = booking.TotalPrice,
                HotelAddress = rooms.FirstOrDefault()?.RoomClass.Hotel.City.Address!,
                RoomDetails = rooms.Select(room => new RoomDetails
                {
                    Floor = room.Floor,
                    RoomNumber = room.RoomNumber,
                }).ToList()
            };

            var invoiceHtml = _invoiceHtmlGenerationService.GenerateHtml(invoice);
            var invoicePdf = await _pdfService.GeneratePdfAsync(invoiceHtml);

            var emailAttachment = new EmailAttachment
            {
                FileName = "Invoice.pdf",
                FileContent = invoicePdf,
                ContentType = "application/pdf"
            };

            await _emailSenderService.SendEmailAsync(user.Email, "Your Booking Invoice", invoiceHtml, [emailAttachment]);
            await _unitOfWork.CommitAsync();
            return booking.Id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private double CalculateTotalPric(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate)
    {
        var total = 0.0;
        foreach (var room in rooms)
        {
            var price = room.RoomClass.Price;
            var discount = room.RoomClass.Discounts.Where(d => d.RoomClassId == room.RoomClassId).Max(d => d.Percentage);
            total += price - (price * (discount / 100));
        }
        var stayDuration = checkOutDate.Day - checkInDate.Day;
        return stayDuration * total;
    }
}
