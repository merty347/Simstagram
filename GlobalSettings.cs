using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simstagram2._0
{
    public class GlobalSettings : IGlobalSettings
    {
        private IHttpContextAccessor _contextAccessor;
        public GlobalSettings(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string Username //osoba, ktr jest zalogowana
        {
            get 
            {
                return GetValue(_contextAccessor.HttpContext.User, ClaimTypes.Name);
            }            
        }

        public string GetValue(ClaimsPrincipal claims, string key)
        {
            if (claims == null)
            {
                return null;
            }
            return claims.FindFirstValue(key);
        }
    }
}
