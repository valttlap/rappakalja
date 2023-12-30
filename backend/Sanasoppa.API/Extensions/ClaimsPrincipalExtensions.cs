using System.Security.Claims;

namespace Sanasoppa.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst("https://sanasoppa.com/username")?.Value;
    }
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
