using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RentACareGRPC.Authorization
{
    public class CEOHandler : AuthorizationHandler<CEORequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CEORequirement requirement)
        {
            var rolname = requirement.RoleName;
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == rolname))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
