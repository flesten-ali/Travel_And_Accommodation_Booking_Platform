using FluentValidation;

namespace TABP.Infrastructure.Security.Jwt;

public class JwtOptionsValidator : AbstractValidator<JwtConfig>
{
    public JwtOptionsValidator()
    {
        RuleFor(jwt => jwt.Key)
            .NotEmpty().WithMessage("JWT key is required.");

        RuleFor(jwt => jwt.Audience)
            .NotEmpty().WithMessage("JWT audience is required.");

        RuleFor(jwt => jwt.Issuer)
            .NotEmpty().WithMessage("JWT issuer is required.");

        RuleFor(jwt => jwt.ExpirationTimeInMinutes)
            .GreaterThan(0).WithMessage("Expiration time must be greater that zero.");
    }
}
