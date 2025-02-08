using FluentValidation;
using TABP.Presentation.DTOs.RoomClass;

namespace TABP.Presentation.Validators.RoomClass;
public class GetRoomClassDetailsRequestValidator : AbstractValidator<GetRoomClassDetailsRequest>
{
    public GetRoomClassDetailsRequestValidator()
    {
        RuleFor(r => r.HotelId)
            .NotEmpty().WithMessage("HotelId is required.");

        RuleFor(r => r.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(r => r.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}