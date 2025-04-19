using FluentValidation;
using TABP.Presentation.DTOs.City;

namespace TABP.Presentation.Validators.City;
public class UpdateCityRequestValidator : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator()
    {
        RuleFor(x => x.Name)
          .NotEmpty().WithMessage("City name is required.")
          .MaximumLength(100).WithMessage("City name must not exceed 100 characters.");

        RuleFor(x => x.PostalCode)
            .MaximumLength(10).WithMessage("Postal Code must not exceed 10 characters.");

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");

        RuleFor(x => x.PostOffice)
            .NotEmpty().WithMessage("Post office is required.")
            .MaximumLength(100).WithMessage("Post office must not exceed 100 characters.");
    }
}
