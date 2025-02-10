using FluentValidation;
using TABP.Presentation.DTOs.Booking;

namespace TABP.Presentation.Validators.Booking;
public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(b => b.HotelId)
           .NotNull()
           .WithMessage("Hotel ID is required.");

        RuleFor(b => b.UserId)
            .NotNull()
            .WithMessage("User ID is required.");

        RuleFor(b => b.RoomIds)
            .NotEmpty()
            .WithMessage("At least one room must be selected.");

        RuleFor(b => b.CheckInDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Check-in date must be in the future.");

        RuleFor(b => b.CheckOutDate)
            .GreaterThan(b => b.CheckInDate)
            .WithMessage("Check-out date must be after the check-in date.");

        RuleFor(b => b.PaymentMethod)
            .IsInEnum()
            .WithMessage("Invalid payment  method.");

        RuleFor(b => b.Remarks)
            .MaximumLength(300)
            .WithMessage("Remarks cannot exceed 300 characters.");
    }
}
