using Microsoft.AspNetCore.Http;

namespace Utility
{
    public class AuthData(IHttpContextAccessor accessor)
    {
        public const string Super = "SUPER";
        public const string Admin = "ADMIN";

        public const string AdminIdField = "AdminId";
        public const string AdminNameField = "AdminName";
        public const string RoleField = "Role";

        public string AdminId => accessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == AdminIdField)?.Value ?? "";
        public string AdminName => accessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == AdminNameField)?.Value ?? "";
    }
}
