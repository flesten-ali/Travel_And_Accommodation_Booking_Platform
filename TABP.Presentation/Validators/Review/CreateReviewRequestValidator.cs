using FluentValidation;
using TABP.Presentation.DTOs.Review;

namespace TABP.Presentation.Validators.Review;
public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewRequestValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");

        RuleFor(x => x.Rate)
            .InclusiveBetween(1, 5).WithMessage("Rate must be between 1 and 5.");
    }
}
