using System.Security.Claims;

namespace TABP.Presentation.Extensions;

/// <summary>
/// Extension methods for <see cref="ClaimsPrincipal"/> to enhance functionality 
/// related to claims, specifically retrieving user-related information.
/// </summary>
public static class ClaimPrincipalExtensions
{
    /// <summary>
    /// Retrieves the User's ID from the ClaimsPrincipal object.
    /// This method expects the User ID to be stored as a claim with the <see cref="ClaimTypes.NameIdentifier"/> type.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> object that represents the current authenticated user.</param>
    /// <returns>
    /// A <see cref="Guid"/> representing the user's ID.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <see cref="ClaimTypes.NameIdentifier"/> claim is missing or invalid.
    /// </exception>
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var result) ? result : throw new ArgumentNullException();
    }
}
