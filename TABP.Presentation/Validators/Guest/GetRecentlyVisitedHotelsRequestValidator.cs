using FluentValidation;
using TABP.Presentation.DTOs.Guest;
namespace TABP.Presentation.Validators.Guest;

public class GetRecentlyVisitedHotelsRequestValidator : AbstractValidator<GetRecentlyVisitedHotelsRequest>
{
    public GetRecentlyVisitedHotelsRequestValidator()
    {
        RuleFor(x => x.Limit)
               .GreaterThan(0).WithMessage("Limit must be greater than 0.");
    }
}
