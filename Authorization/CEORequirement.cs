using Microsoft.AspNetCore.Authorization;

namespace RentACareGRPC.Authorization
{
    public class CEORequirement : IAuthorizationRequirement
    {
        public string RoleName { get; }
        public CEORequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}
