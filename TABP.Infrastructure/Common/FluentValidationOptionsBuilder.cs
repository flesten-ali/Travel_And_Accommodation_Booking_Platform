using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TABP.Infrastructure.Common;

/// <summary>
/// Provides an extension method for integrating FluentValidation with the options validation system in .NET.
/// </summary>
public static class FluentValidationOptionsBuilder
{
    /// <summary>
    /// Registers FluentValidation for validating options classes in .NET's built-in options system.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options class to validate.</typeparam>
    /// <param name="optionsBuilder">The options builder for configuring validation.</param>
    /// <returns>The modified <see cref="OptionsBuilder{TOptions}"/> to allow for fluent configuration.</returns>
    /// <remarks>
    /// This method ensures that options registered in the dependency injection container are automatically 
    /// validated using FluentValidation before being resolved.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// services.AddOptions<MyOptions>()
    ///         .Bind(configuration.GetSection("MySection"))
    ///         .ValidateFluently();
    /// </code>
    /// </example>
    public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder)
        where TOptions : class
    {
        optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
            sp => new FluentValidationValidateOptions<TOptions>(
                optionsBuilder.Name, sp.GetRequiredService<IValidator<TOptions>>()
            )
        );
        return optionsBuilder;
    }
}

/// <summary>
/// Implements FluentValidation-based validation for options classes registered in .NET's options system.
/// </summary>
/// <typeparam name="TOptions">The type of the options class to validate.</typeparam>
internal class FluentValidationValidateOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions> _validator;

    public FluentValidationValidateOptions(string? name, IValidator<TOptions> validator)
    {
        Name = name;
        _validator = validator;
    }
    public string? Name { get; }

    /// <summary>
    /// Validates the specified options instance using FluentValidation.
    /// </summary>
    /// <param name="name">The name of the options instance.</param>
    /// <param name="options">The options instance to validate.</param>
    /// <returns>
    /// A <see cref="ValidateOptionsResult"/> indicating success or failure with validation errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the options instance is null.</exception>
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
           $"options validations failed for ${err.PropertyName} with error {err.ErrorMessage}!"
        );
        return ValidateOptionsResult.Fail(errors);
    }
}