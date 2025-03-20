using FluentValidation;
using TABP.Presentation.DTOs.Review;
using TABP.Presentation.Validators.Common;
namespace TABP.Presentation.Validators.Review;

public class GetHotelReviewsRequestValidator : AbstractValidator<GetHotelReviewsRequest>
{
    public GetHotelReviewsRequestValidator()
    {
        RuleFor(r => r.PaginationParameters)
                   .SetValidator(new PaginationParametersValidator());
    }
}