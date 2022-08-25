using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simstagram2._0
{
    public interface IGlobalSettings 
    {
        public string GetValue(ClaimsPrincipal claims, string key);
        public string Username { get; }
    }
}
