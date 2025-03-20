using FluentValidation;
using TABP.Presentation.DTOs.Owner;

namespace TABP.Presentation.Validators.Owner;
public class CreateOwnerRequsetValidator : AbstractValidator<CreateOwnerRequest>
{
    public CreateOwnerRequsetValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Owner name is required.")
            .Length(3, 100).WithMessage("Owner name must be between 3 and 100 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .Length(10, 200).WithMessage("Address must be between 10 and 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("A valid phone number is required.");
    }
}
