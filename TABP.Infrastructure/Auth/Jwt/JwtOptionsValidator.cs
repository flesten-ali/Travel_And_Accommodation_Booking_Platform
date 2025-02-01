using FluentValidation;
namespace TABP.Infrastructure.Auth.Jwt;

public class JwtOptionsValidator : AbstractValidator<JwtConfig>
{
    public JwtOptionsValidator()
    {
        RuleFor(jwt => jwt.Key)
            .NotEmpty().WithMessage("JWT Key is required!");

        RuleFor(jwt => jwt.Audience)
            .NotEmpty().WithMessage("JWT Audience is required!");

        RuleFor(jwt => jwt.Issuer)
            .NotEmpty().WithMessage("JWT Issuer is required!");

        RuleFor(jwt => jwt.ExpirationTimeInMinutes)
            .GreaterThan(0).WithMessage("Expiration time must be greater that zero!");
    }
}
