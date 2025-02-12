using FluentValidation;
using TABP.Presentation.DTOs.City;

namespace TABP.Presentation.Validators.City;
public class GetCitiesForAdminRequestValidator: AbstractValidator<GetCitiesForAdminRequest>
{
    public GetCitiesForAdminRequestValidator()
    {
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page Size must be between 1 and 100.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}
