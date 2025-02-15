using FluentValidation;
using TABP.Presentation.DTOs.Hotel;

namespace TABP.Presentation.Validators.Hotel;
public class GetHotelsForAdminRequestValidator : AbstractValidator<GetHotelsForAdminRequest>
{
    public GetHotelsForAdminRequestValidator()
    {
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}
