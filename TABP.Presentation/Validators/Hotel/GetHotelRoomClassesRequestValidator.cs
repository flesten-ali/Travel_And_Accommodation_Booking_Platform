using FluentValidation;
using TABP.Presentation.DTOs.Hotel;

namespace TABP.Presentation.Validators.Hotel;
public class GetHotelRoomClassesRequestValidator : AbstractValidator<GetHotelRoomClassesRequest>
{
    public GetHotelRoomClassesRequestValidator()
    {
        RuleFor(r => r.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(r => r.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}