using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Shared.Utilities
{
    public static class ClaimsPrincipalExtention
    {
        public static string GetUserId(this ClaimsPrincipal claims)
        {
            return claims.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;
            //return "d5d8358d-e850-41a5-aba7-0b2b715b24db";
        }
    }
}
