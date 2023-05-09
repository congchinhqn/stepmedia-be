using System.Security;
using System.Security.Claims;
using System.Security.Principal;

namespace Metatrade.Core.Extensions
{
    public static class PrincipalExtensions
    {
        #region Methods

        public static string GetClaimValue(this ClaimsIdentity identity, string claimType)
        {
            return identity?.FindFirst(claimType)?.Value;
        }

        public static string GetClaimValue(this IIdentity identity, string claimType)
        {
            return identity is ClaimsIdentity claimsIdentity
                ? claimsIdentity.GetClaimValue(claimType)
                : null;
        }

        public static int GetClientId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Identity.GetExpectedClaimValue("cid"));
        }

        public static string GetExpectedClaimValue(this IIdentity identity, string claimType)
        {
            var str = GetClaimValue(identity, claimType);
            if (str == null)
                throw new SecurityException("invalid_claim_" + claimType);

            return str;
        }

        public static int GetPartnerId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Identity.GetExpectedClaimValue("pid"));
        }

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Identity.GetExpectedClaimValue("uid"));
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.Identity.GetExpectedClaimValue("preferred_username");
        }

        public static int GetWarehouseId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Identity.GetExpectedClaimValue("wid"));
        }

        #endregion
    }
}