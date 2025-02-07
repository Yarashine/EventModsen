using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Presentation.Authorization;

public class AgeHandler : AuthorizationHandler<AgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        var ageClaim = context.User.Claims.FirstOrDefault(c => c.Type == "age");
        var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if(ageClaim is null || roleClaim is null)
            return Task.CompletedTask;

        if (roleClaim.Value == "Admin")
            context.Succeed(requirement);

        if (int.TryParse(ageClaim.Value, out int age) && age >= requirement.MinimumAge)
        {
            if (roleClaim.Value == requirement.RequiredRole)
                context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
