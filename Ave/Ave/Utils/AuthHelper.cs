using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Ave.API.ViewModels;
using Ave.Common.Authentication;

namespace Ave.API.Utils
{
    public static class AuthHelper
    {
        public static UserData GetUserData(ICollection<Claim> claims)
        {
            var userData = new UserData
            {
                UserId = GetClaimValue(claims, CustomClaimName.UserId),
                UserName = GetClaimValue(claims, CustomClaimName.Name),
                Role = GetClaimValue(claims, CustomClaimName.Role),
                Id = GetClaimValue(claims, CustomClaimName.Id)
            };

            return userData;
        }

        private static string GetClaimValue(IEnumerable<Claim> claims, string claimName)
        {
            return claims
                .FirstOrDefault(x => x.Type.Equals(claimName, StringComparison.InvariantCultureIgnoreCase))
                ?.Value;
        }
    }
}
