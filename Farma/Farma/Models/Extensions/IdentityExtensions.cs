using System.Security.Claims;
using System.Security.Principal;

namespace Farma.Models.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserRole(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Role");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}