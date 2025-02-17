using Microsoft.AspNetCore.Authorization;

namespace EventModsen.Authorization;

public class AgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }
    public string RequiredRole { get; }

    public AgeRequirement(int minimumAge, string requiredRole)
    {
        MinimumAge = minimumAge;
        RequiredRole = requiredRole;
    }
}
