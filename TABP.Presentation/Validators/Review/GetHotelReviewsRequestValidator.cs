using FluentValidation;
using TABP.Presentation.DTOs.Review;
namespace TABP.Presentation.Validators.Review;

public class GetHotelReviewsRequestValidator : AbstractValidator<GetHotelReviewsRequest>
{
    public GetHotelReviewsRequestValidator()
    {
        RuleFor(r => r.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(r => r.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
    }
}