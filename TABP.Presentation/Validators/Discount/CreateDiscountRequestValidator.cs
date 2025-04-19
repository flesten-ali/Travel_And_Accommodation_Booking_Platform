using FluentValidation;
using TABP.Presentation.DTOs.Discount;

namespace TABP.Presentation.Validators.Discount;
public class CreateDiscountRequestValidator : AbstractValidator<CreateDiscountRequest>
{
    public CreateDiscountRequestValidator()
    {
        RuleFor(d => d.Percentage)
            .InclusiveBetween(0, 100).WithMessage("Percentage must be between 0 and 100.");

        RuleFor(d => d.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Start date must be in the future.")
            .LessThan(d => d.EndDate).WithMessage("Start date must be before the end date.");

        RuleFor(d => d.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(d => d.StartDate).WithMessage("End date must be after the start date.");
    }
}
