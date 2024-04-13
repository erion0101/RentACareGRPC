using Microsoft.AspNetCore.Authorization;

namespace RentACareGRPC.Authorization
{
    public class AdminRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; }
        public AdminRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}
