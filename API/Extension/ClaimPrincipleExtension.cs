using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extension
{
    public static class ClaimPrincipleExtension
    {
        public static string getUserName(this ClaimsPrincipal user ){
           return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}