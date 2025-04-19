using FluentValidation;

namespace TABP.Infrastructure.Services.Image;

public class CloudinaryOptionsValidator : AbstractValidator<CloudinaryConfig>
{
    public CloudinaryOptionsValidator()
    {
        RuleFor(x => x.Cloud)
          .NotEmpty().WithMessage("Cloud name is required.");

        RuleFor(x => x.ApiKey)
            .NotEmpty().WithMessage("API Key is required.");

        RuleFor(x => x.ApiSecret)
            .NotEmpty().WithMessage("API Secret is required.");
    }
}
