using System;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user) // Name -> UniqueName
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        
        public static int GetUserId(this ClaimsPrincipal user) //NameIdentifier -> NameId
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
        }
    }
}