using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RentACareGRPC.Authorization
{
    public class AdminHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            var roleName = requirement.RoleName;
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == roleName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
