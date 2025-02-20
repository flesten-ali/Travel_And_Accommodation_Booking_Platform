using System.Security.Claims;

namespace TABP.Presentation.Extensions;
public static class ClaimPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var result) ? result : throw new ArgumentNullException();
    }
}
