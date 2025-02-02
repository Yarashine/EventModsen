using Microsoft.AspNetCore.Authorization;

namespace EventModsen.Api.Authorization;

public class UserRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }
    public string RequiredRole { get; }

    public UserRequirement(int minimumAge, string requiredRole)
    {
        MinimumAge = minimumAge;
        RequiredRole = requiredRole;
    }
}
