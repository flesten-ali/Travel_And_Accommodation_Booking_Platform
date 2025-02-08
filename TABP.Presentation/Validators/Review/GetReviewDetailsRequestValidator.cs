using FluentValidation;
using TABP.Application.Reviews.Queries.GetDetails;
namespace TABP.Presentation.Validators.Review;

public class GetReviewDetailsRequestValidator : AbstractValidator<GetReviewDetailsQuery>
{
    public GetReviewDetailsRequestValidator()
    {
        RuleFor(r => r.HotelId)
            .NotEmpty().WithMessage("HotelId is required.");

        RuleFor(r => r.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(r => r.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}