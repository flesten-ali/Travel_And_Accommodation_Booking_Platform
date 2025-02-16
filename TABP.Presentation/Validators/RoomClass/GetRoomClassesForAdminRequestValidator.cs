using FluentValidation;
using TABP.Presentation.DTOs.RoomClass;

namespace TABP.Presentation.Validators.RoomClass;
public class GetRoomClassesForAdminRequestValidator : AbstractValidator<GetRoomClassesForAdminRequest>
{
    public GetRoomClassesForAdminRequestValidator()
    {
        RuleFor(x => x.PageSize)
             .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}
