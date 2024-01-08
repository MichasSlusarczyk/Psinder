using Psinder.API.Auth.Models.Common;
using Psinder.DB.Auth.Entities;

namespace Psinder.API.Common.Extensions
{
    public static class HttpContextExtension
    {
        public static string? GetUserClaimValue(this HttpContext context, string claimType)
        {
            return context.User.Claims.FirstOrDefault(claim => claim.Type.ToLower() == claimType.ToLower())?.Value;
        }

        public static long? GetUserClaimValueAsLong(this HttpContext context, string claimType)
        {
            var value = context.GetUserClaimValue(claimType);
            return value != null ? long.Parse(value) : null;
        }

        public static long? GetLoggedUserId(this HttpContext context)
        {
            return context.GetUserClaimValueAsLong(Enum.GetName(ClaimType.UserId)!);
        }

        public static long? GetLoggedLoginId(this HttpContext context)
        {
            return context.GetUserClaimValueAsLong(Enum.GetName(ClaimType.LoginId)!);
        }

        public static Role GetLoggedUserRole(this HttpContext context)
        {
            return (Role)Enum.Parse(typeof(Role), context.GetUserClaimValue(Enum.GetName(ClaimType.Role)!)!);
        }
    }
}
