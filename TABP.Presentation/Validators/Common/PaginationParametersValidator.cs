using FluentValidation;
using TABP.Application.Shared;

namespace TABP.Presentation.Validators.Common;
public class PaginationParametersValidator : AbstractValidator<PaginationParameters>
{
    public PaginationParametersValidator()
    {

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.OrderColumn)
            .MaximumLength(20).WithMessage("Order column cannot exceed 20 characters.")
            .Must(value => string.IsNullOrEmpty(value) || value.All(char.IsLetter))
            .WithMessage("Order column must contain only lette");
    }
}
