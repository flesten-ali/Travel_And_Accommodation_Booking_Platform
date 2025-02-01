using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace TABP.Infrastructure.Auth.Jwt;

public static class FluentValidationOptionsBuilder
{
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            sp => new FluentValidationValidateOptions<TOptions>(optionsBuilder.Name, sp.GetRequiredService<IValidator<TOptions>>())
        );
        return optionsBuilder;
    }
}

internal class FluentValidationValidateOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions> _validator;

    public FluentValidationValidateOptions(string? name, IValidator<TOptions> validator)
    {
        Name = name;
        _validator = validator;
    }

    public string? Name { get; }

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        if (Name != null && Name != name)
        {
            return ValidateOptionsResult.Skip;
        }

        ArgumentNullException.ThrowIfNull(options);

        var validationResults = _validator.Validate(options);
        if (validationResults.IsValid)
        {
            return ValidateOptionsResult.Success;
        }
        var errors = validationResults.Errors.Select(err =>
        $"options validaions faild for ${err.PropertyName} with errror {err.ErrorMessage}!"
        );
        return ValidateOptionsResult.Fail(errors);
    }
}