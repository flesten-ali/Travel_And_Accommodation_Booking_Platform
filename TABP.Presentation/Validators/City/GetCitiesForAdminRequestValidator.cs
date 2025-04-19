using FluentValidation;
using TABP.Presentation.DTOs.City;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.City;
public class GetCitiesForAdminRequestValidator : AbstractValidator<GetCitiesForAdminRequest>
{
    public GetCitiesForAdminRequestValidator()
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}